using System;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public abstract class LazyInstancePolicyViolationHandlerConvention<TPolicyViolationHandler> : IPolicyViolationHandlerConvention where TPolicyViolationHandler : class, IPolicyViolationHandler
	{
		private readonly Func<TPolicyViolationHandler> _policyViolationHandlerFactory;

		public Func<PolicyResult, bool> Predicate { get; private set; }

		protected LazyInstancePolicyViolationHandlerConvention(Func<TPolicyViolationHandler> policyViolationHandlerFactory) : this(policyViolationHandlerFactory, pr => true) {}

		protected LazyInstancePolicyViolationHandlerConvention(Func<TPolicyViolationHandler> policyViolationHandlerFactory, Func<PolicyResult, bool> predicate)
		{
			if (policyViolationHandlerFactory == null) throw new ArgumentNullException("policyViolationHandlerFactory");
			if (predicate == null) throw new ArgumentNullException("predicate");

			_policyViolationHandlerFactory = policyViolationHandlerFactory;
			Predicate = predicate;
		}

		public IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception)
		{
			return Predicate.Invoke(exception.PolicyResult) ? _policyViolationHandlerFactory.Invoke() : null;
		}
	}
}