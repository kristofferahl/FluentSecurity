using System;
using FluentSecurity.Core;
using FluentSecurity.Core.Policy.ViolationHandlers;
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
					throw new PolicyViolationHandlerConversionException(instance.GetType(), typeof(TViolationHandlerType));

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