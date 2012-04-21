using System;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public abstract class LazyInstancePolicyViolationHandlerConvention<TPolicyViolationHandler> : IPolicyViolationHandlerConvention where TPolicyViolationHandler : class, IPolicyViolationHandler
	{
		private readonly Func<TPolicyViolationHandler> _policyViolationHandlerFactory;

		protected LazyInstancePolicyViolationHandlerConvention(Func<TPolicyViolationHandler> policyViolationHandlerFactory)
		{
			_policyViolationHandlerFactory = policyViolationHandlerFactory;
		}

		public IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception)
		{
			return _policyViolationHandlerFactory.Invoke();
		}
	}
}