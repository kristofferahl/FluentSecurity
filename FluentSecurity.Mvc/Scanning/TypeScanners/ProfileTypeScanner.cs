using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentSecurity.Configuration;
using FluentSecurity.Core.Internals;

namespace FluentSecurity.Scanning.TypeScanners
{
	internal class ProfileTypeScanner<TProfileType> : ITypeScanner where TProfileType : ISecurityProfile
	{
		private readonly Type _profileType = typeof(TProfileType);

		public IEnumerable<Type> Scan(IEnumerable<Assembly> assemblies)
		{
			var results = new List<Type>();
			foreach (var assembly in assemblies)
			{
				var profileTypes = assembly.GetLoadableExportedTypes()
					.Where(type => _profileType.IsAssignableFrom(type) && type != _profileType)
					.ToList();

				results.AddRange(profileTypes);
			}
			return results;
		}
	}
}