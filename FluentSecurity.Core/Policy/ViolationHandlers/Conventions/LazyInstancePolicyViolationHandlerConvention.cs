using System;
using FluentSecurity.Core.Policy.ViolationHandlers;

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
				throw new PolicyViolationHandlerConversionException(instance.GetType(), typeof(TViolationHandlerType));

			return instance;
		}
	}
}