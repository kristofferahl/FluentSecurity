using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Scanning;

namespace FluentSecurity.ServiceLocation
{
	public sealed class ServiceLocator
	{
		private static readonly object LockObject = new object();
		private static volatile ServiceLocator _serviceLocator;

		private readonly Dictionary<string, IContainer> _containers;
		private IContainer Container { get { return _containers.First().Value; } }

		public ServiceLocator()
		{
			_containers = new Dictionary<string, IContainer>();
			var scanner = new CoreAssemblyScanner();
			scanner.AssembliesFromApplicationBaseDirectory(x => x.FullName.StartsWith("FluentSecurity."));
			scanner.With<RegistryTypeScanner>();
			foreach (var registryType in scanner.Scan())
			{
				var registry = (IRegistry) Activator.CreateInstance(registryType);
				var container = registry.Configure();
				_containers.Add(registry.GetType().Assembly.FullName, container);
			}
		}

		public static ServiceLocator Current
		{
			get
			{
				if (_serviceLocator == null)
				{
					lock (LockObject)
					{
						_serviceLocator = new ServiceLocator();
					}
				}
				return _serviceLocator;
			}
		}

		public static void Reset()
		{
			lock (LockObject)
			{
				_serviceLocator = null;
			}
		}

		public object Resolve(Type typeToResolve)
		{
			return Container.Resolve(typeToResolve);
		}

		public TTypeToResolve Resolve<TTypeToResolve>()
		{
			return Container.Resolve<TTypeToResolve>();
		}

		public IEnumerable<object> ResolveAll(Type typeToResolve)
		{
			return Container.ResolveAll(typeToResolve);
		}

		public IEnumerable<TTypeToResolve> ResolveAll<TTypeToResolve>()
		{
			return Container.ResolveAll<TTypeToResolve>();
		}
	}
}