using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentSecurity.Configuration;

namespace FluentSecurity.Scanning
{
	internal class ProfileTypeScanner : ITypeScanner
	{
		public IEnumerable<Type> Scan(IEnumerable<Assembly> assemblies)
		{
			var results = new List<Type>();
			foreach (var assembly in assemblies)
			{
				var profileTypes = assembly.GetExportedTypes()
					.Where(type => typeof(SecurityProfile).IsAssignableFrom(type) && type != typeof(SecurityProfile))
					.ToList();

				results.AddRange(profileTypes);
			}
			return results;
		}
	}
}