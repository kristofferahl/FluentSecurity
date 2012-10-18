using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace FluentSecurity.Scanning
{
	internal class AssemblyScanner
	{
		private readonly List<Assembly> _assemblies = new List<Assembly>();
		private readonly List<ITypeScanner> _scanners = new List<ITypeScanner>();
		private readonly IList<Func<Type, bool>> _filters = new List<Func<Type, bool>>();

		public void Assembly(Assembly assembly)
		{
			if (assembly == null) throw new ArgumentNullException("assembly");
			_assemblies.Add(assembly);
		}

		public void TheCallingAssembly()
		{
			var callingAssembly = FindCallingAssembly();
			if (callingAssembly != null) _assemblies.Add(callingAssembly);
		}

		private static Assembly FindCallingAssembly()
		{
			var thisAssembly = System.Reflection.Assembly.GetExecutingAssembly();
			Assembly callingAssembly = null;

			var trace = new StackTrace(false);
			for (var i = 0; i < trace.FrameCount; i++)
			{
				var frame = trace.GetFrame(i);
				var assembly = frame.GetMethod().DeclaringType.Assembly;
				if (assembly != thisAssembly)
				{
					callingAssembly = assembly;
					break;
				}
			}
			return callingAssembly;
		}

		public void With(ITypeScanner typeScanner)
		{
			_scanners.Add(typeScanner);
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
			_filters.Add(predicate);
		}

		public IEnumerable<Type> Scan()
		{
			var results = new List<Type>();
			_scanners.Each(scanner => scanner.Scan(_assemblies).Where(type =>
				_filters.Any() == false || _filters.Any(filter => filter.Invoke(type))).Each(results.Add)
				);
			return results;
		}

		public IEnumerable<Type> Scan<TController>()
			where TController : IController
		{
			var results = new List<Type>();
			_scanners.Each(scanner => scanner.Scan<TController>(_assemblies).Where(type =>
				_filters.Any() == false || _filters.Any(filter => filter.Invoke(type))).Each(results.Add)
				);
			return results;
		}
	}
}