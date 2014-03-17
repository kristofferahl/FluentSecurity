using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentSecurity.Core.Internals;
using FluentSecurity.Scanning.TypeScanners;

namespace FluentSecurity.ServiceLocation
{
	internal class RegistryTypeScanner : ITypeScanner
	{
		public static readonly Type RegistryType = typeof(IRegistry);

		public IEnumerable<Type> Scan(IEnumerable<Assembly> assemblies)
		{
			var results = new List<Type>();
			foreach (var assembly in assemblies)
			{
				var registryTypes = assembly.GetLoadableExportedTypes()
					.Where(type => RegistryType.IsAssignableFrom(type) && type != RegistryType)
					.ToList();

				results.AddRange(registryTypes);
			}
			return results;
		}
	}
}