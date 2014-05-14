using System;
using FluentSecurity.Core;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public abstract class PolicyViolationHandlerTypeConvention : IPolicyViolationHandlerConvention, IServiceLocatorDependent
	{
		private Func<Type, object> _policyViolationHandlerProvider;

		public abstract Type GetHandlerTypeFor(PolicyViolationException exception);

		public object GetHandlerFor<TViolationHandlerType>(PolicyViolationException exception) where TViolationHandlerType : ISecurityPolicyViolationHandler
		{
			var type = GetHandlerTypeFor(exception);
			if (type != null)
			{
				var instance = _policyViolationHandlerProvider.Invoke(type) as ISecurityPolicyViolationHandler;
				if (instance != null && !(instance is TViolationHandlerType))
				{
					// TODO: Create and throw custom exception related to violation handler type conversion
					throw new Exception(String.Format("The violation handler {0} does not implement the interface {1}!", instance.GetType(), typeof(TViolationHandlerType).FullName));
				}
				return instance;
			}
			return null;
		}

		public void Inject(IServiceLocator serviceLocator)
		{
			_policyViolationHandlerProvider = serviceLocator.Resolve;
		}
	}
}