using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentSecurity.Core.Internals;
using FluentSecurity.Scanning.TypeScanners;

namespace FluentSecurity.Diagnostics
{
	public class SecurityEventListenerScanner : ITypeScanner
	{
		public IEnumerable<Type> Scan(IEnumerable<Assembly> assemblies)
		{
			return assemblies.SelectMany(a => a.GetLoadableExportedTypes()).Where(TypeIsExternalListener).ToList();
		}
		
		private static bool TypeIsExternalListener(Type type)
		{
			var interfaceType = typeof (ISecurityEventListener);
			return
				interfaceType.IsAssignableFrom(type) &&
				!type.IsInterface &&
				!type.Assembly.Equals(interfaceType.Assembly);
		}
	}
}