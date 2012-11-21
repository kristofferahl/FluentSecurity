using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentSecurity.Configuration;

namespace FluentSecurity.Scanning
{
	public class ProfileScanner : AssemblyScanner
	{
		public void AssembliesFromApplicationBaseDirectory()
		{
			AssembliesFromApplicationBaseDirectory(a => true);
		}

		public void AssembliesFromApplicationBaseDirectory(Predicate<Assembly> assemblyFilter)
		{
			var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			AssembliesFromPath(baseDirectory, assemblyFilter);
		}

		private void AssembliesFromPath(string path, Predicate<Assembly> assemblyFilter)
		{
			var assemblyPaths = Directory.GetFiles(path).Where(file =>
			{
				var extension = Path.GetExtension(file);
				return extension != null && (
					extension.Equals(".exe", StringComparison.OrdinalIgnoreCase) ||
					extension.Equals(".dll", StringComparison.OrdinalIgnoreCase)
					);
			}).ToList();

			foreach (var assemblyPath in assemblyPaths)
			{
				var assembly = System.Reflection.Assembly.LoadFrom(assemblyPath);
				if (assembly != null && assemblyFilter.Invoke(assembly))
					Assembly(assembly);
			}
		}

		public void LookForProfiles()
		{
			With<ProfileTypeScanner>();
		}

		private class ProfileTypeScanner : ITypeScanner
		{
			public IEnumerable<Type> Scan(IEnumerable<Assembly> assemblies)
			{
				var results = new List<Type>();
				foreach (var assembly in assemblies)
				{
					var profileTypes = assembly.GetExportedTypes()
						.Where(type => typeof (SecurityProfile).IsAssignableFrom(type) && type != typeof (SecurityProfile))
						.ToList();
					
					results.AddRange(profileTypes);
				}
				return results;
			}
		}
	}
}