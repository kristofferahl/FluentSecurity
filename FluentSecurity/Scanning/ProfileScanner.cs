using System;
using System.IO;
using System.Linq;
using System.Reflection;

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
	}
}