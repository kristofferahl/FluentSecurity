using System;
using FluentSecurity.Core;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public abstract class PolicyViolationHandlerTypeConvention : IPolicyViolationHandlerConvention, IServiceLocatorDependent
	{
		private Func<Type, object> _policyViolationHandlerProvider;

		public abstract Type GetHandlerTypeFor(PolicyViolationException exception);

		public object GetHandlerFor(PolicyViolationException exception)
		{
			var type = GetHandlerTypeFor(exception);
			if (type != null)
			{
				if (typeof(ISecurityPolicyViolationHandler).IsAssignableFrom(type))
					return _policyViolationHandlerProvider.Invoke(type) as ISecurityPolicyViolationHandler;
			}
			return null;
		}


		public void Inject(IServiceLocator serviceLocator)
		{
			_policyViolationHandlerProvider = serviceLocator.Resolve;
		}
	}
}