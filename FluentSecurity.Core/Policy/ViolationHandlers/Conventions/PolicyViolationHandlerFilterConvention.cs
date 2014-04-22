using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Core;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public abstract class PolicyViolationHandlerFilterConvention : IPolicyViolationHandlerConvention, IServiceLocatorDependent
	{
		private Func<IEnumerable<ISecurityPolicyViolationHandler>> _policyViolationHandlerProvider;

		public abstract ISecurityPolicyViolationHandler GetHandlerFor(PolicyViolationException exception, IEnumerable<ISecurityPolicyViolationHandler> policyViolationHandlers);
		
		public object GetHandlerFor(PolicyViolationException exception)
		{
			var policyViolationHandlers = _policyViolationHandlerProvider.Invoke().ToList();
			return GetHandlerFor(exception, policyViolationHandlers);
		}

		public void Inject(IServiceLocator serviceLocator)
		{
			_policyViolationHandlerProvider = serviceLocator.ResolveAll<ISecurityPolicyViolationHandler>;
		}
	}
}