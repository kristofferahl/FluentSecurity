using System;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public abstract class LazyInstancePolicyViolationHandlerConvention<TPolicyViolationHandler> : IPolicyViolationHandlerConvention where TPolicyViolationHandler : class, ISecurityPolicyViolationHandler
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

		public object GetHandlerFor<TViolationHandlerType>(PolicyViolationException exception) where TViolationHandlerType : ISecurityPolicyViolationHandler
		{
			var instance = Predicate.Invoke(exception.PolicyResult) ? _policyViolationHandlerFactory.Invoke() : null;
			if (instance != null && !(instance is TViolationHandlerType))
			{
				// TODO: Create and throw custom exception related to violation handler type conversion
				throw new Exception(String.Format("The violation handler {0} does not implement the interface {1}!", instance.GetType(), typeof(TViolationHandlerType).FullName));
			}
			return instance;
		}
	}
}