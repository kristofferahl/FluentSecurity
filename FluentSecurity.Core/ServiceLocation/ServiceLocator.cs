using System;
using System.Collections.Generic;

namespace FluentSecurity.ServiceLocation
{
	public class ServiceLocator : IServiceLocator
	{
		private readonly IContainer _container;

		public ServiceLocator(IContainer container)
		{
			_container = container;
		}

		public object Resolve(Type typeToResolve)
		{
			return _container.Resolve(typeToResolve);
		}

		public TTypeToResolve Resolve<TTypeToResolve>()
		{
			return _container.Resolve<TTypeToResolve>();
		}

		public IEnumerable<object> ResolveAll(Type typeToResolve)
		{
			return _container.ResolveAll(typeToResolve);
		}

		public IEnumerable<TTypeToResolve> ResolveAll<TTypeToResolve>()
		{
			return _container.ResolveAll<TTypeToResolve>();
		}
	}
}