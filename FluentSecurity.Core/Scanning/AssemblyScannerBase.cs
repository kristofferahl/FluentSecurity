using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentSecurity.Scanning.TypeScanners;

namespace FluentSecurity.Scanning
{
	public abstract class AssemblyScannerBase
	{
		public ScannerContext Context { get; private set; }

		protected AssemblyScannerBase()
		{
			Context = new ScannerContext();
		}

		public void Assembly(Assembly assembly)
		{
			Context.AddAssembly(assembly);
		}

		public void Assemblies(IEnumerable<Assembly> assemblies)
		{
			if (assemblies == null) throw new ArgumentNullException("assemblies");

			var assembliesToScan = assemblies.ToList();
			if (assembliesToScan.Any(a => a == null)) throw new ArgumentException("Assemblies must not contain null values.", "assemblies");

			assembliesToScan.ForEach(Assembly);
		}

		public void TheCallingAssembly()
		{
			var callingAssembly = FindCallingAssembly();
			if (callingAssembly != null) Assembly(callingAssembly);
		}

		private Assembly FindCallingAssembly()
		{
			var thisAssembly = System.Reflection.Assembly.GetExecutingAssembly();
			var implementingAssembly = GetType().Assembly;
			Assembly callingAssembly = null;

			var trace = new StackTrace(false);
			for (var i = 0; i < trace.FrameCount; i++)
			{
				var frame = trace.GetFrame(i);
				var assembly = frame.GetMethod().DeclaringType.Assembly;
				if (assembly != thisAssembly && assembly != implementingAssembly)
				{
					callingAssembly = assembly;
					break;
				}
			}
			return callingAssembly;
		}

		public void AssembliesFromApplicationBaseDirectory()
		{
			AssembliesFromApplicationBaseDirectory(a => true);
		}

		public void AssembliesFromApplicationBaseDirectory(Predicate<Assembly> assemblyFilter)
		{
			var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			AssembliesFromPath(baseDirectory, assemblyFilter);

			var binDirectory = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
			AssembliesFromPath(binDirectory, assemblyFilter);
		}

		public void AssembliesFromPath(string path, Predicate<Assembly> assemblyFilter)
		{
			if (!Directory.Exists(path)) return;

			var assemblyPaths = Directory.GetFiles(path).Where(Context.FiltersMatchFile).ToList();

			foreach (var assemblyPath in assemblyPaths)
			{
				var assembly = System.Reflection.Assembly.LoadFrom(assemblyPath);
				if (assembly != null && assemblyFilter.Invoke(assembly))
					Assembly(assembly);
			}
		}

		public void IncludeAssembly(Func<string, bool> filePredicate)
		{
			Func<string, bool> predicate = filePredicate.Invoke;
			Context.AddMatchOneFileFilter(predicate);
		}

		public void ExcludeAssembly(Func<string, bool> filePredicate)
		{
			Func<string, bool> predicate = file => !filePredicate.Invoke(file);
			Context.AddMatchAllFileFilter(predicate);
		}

		public void With(ITypeScanner typeScanner)
		{
			Context.AddTypeScanner(typeScanner);
		}

		public void With<TTypeScanner>() where TTypeScanner : ITypeScanner, new()
		{
			With(new TTypeScanner());
		}

		public void IncludeNamespaceContainingType<T>()
		{
			Func<Type, bool> predicate = type =>
			{
				var currentNamespace = type.Namespace ?? "";
				var expectedNamespace = typeof (T).Namespace ?? "";
				return currentNamespace.StartsWith(expectedNamespace);
			};
			Context.AddMatchOneTypeFilter(predicate);
		}

		public void ExcludeNamespaceContainingType<T>()
		{
			Func<Type, bool> predicate = type =>
			{
				var currentNamespace = type.Namespace ?? "";
				var expectedNamespace = typeof(T).Namespace ?? "";
				return !currentNamespace.StartsWith(expectedNamespace);
			};
			Context.AddMatchAllTypeFilter(predicate);
		}

		public IEnumerable<Type> Scan()
		{
			var results = new List<Type>();
			Context.TypeScanners.ToList().ForEach(scanner => scanner
				.Scan(Context.AssembliesToScan)
				.Where(Context.FiltersMatchType)
				.ToList()
				.ForEach(results.Add)
				);
			return results;
		}
	}
}