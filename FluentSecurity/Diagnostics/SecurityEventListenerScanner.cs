using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentSecurity.Scanning.TypeScanners;

namespace FluentSecurity.Diagnostics
{
	public class SecurityEventListenerScanner : ITypeScanner
	{
		private readonly Func<Assembly, Type[]> _assemblyTypeProvider;
		private readonly bool _ignoreTypeLoadExceptions;

		public SecurityEventListenerScanner(bool ignoreTypeLoadExceptions) : this(ignoreTypeLoadExceptions, assembly => assembly.GetExportedTypes()) {}

		public SecurityEventListenerScanner(bool ignoreTypeLoadExceptions, Func<Assembly, Type[]> assemblyTypeProvider)
		{
			_assemblyTypeProvider = assemblyTypeProvider;
			_ignoreTypeLoadExceptions = ignoreTypeLoadExceptions;
		}

		public IEnumerable<Type> Scan(IEnumerable<Assembly> assemblies)
		{
			return assemblies.SelectMany(a =>
			{
				try
				{
					return _assemblyTypeProvider.Invoke(a);
				}
				catch (TypeLoadException exception)
				{
					Publish.ConfigurationEvent(() => exception.Message);

					if (!_ignoreTypeLoadExceptions)
						throw;

					return new Type[0];
				}
			}).Where(TypeIsExternalListener).ToList();
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