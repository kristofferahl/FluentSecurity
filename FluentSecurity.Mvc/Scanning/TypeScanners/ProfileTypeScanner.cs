using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentSecurity.Configuration;
using FluentSecurity.Core.Internals;

namespace FluentSecurity.Scanning.TypeScanners
{
	internal class ProfileTypeScanner : ITypeScanner
	{
		public static readonly Type ProfileType = typeof(SecurityProfile);

		public IEnumerable<Type> Scan(IEnumerable<Assembly> assemblies)
		{
			var results = new List<Type>();
			foreach (var assembly in assemblies)
			{
				var profileTypes = assembly.GetLoadableExportedTypes()
					.Where(type => ProfileType.IsAssignableFrom(type) && type != ProfileType)
					.ToList();

				results.AddRange(profileTypes);
			}
			return results;
		}
	}
}