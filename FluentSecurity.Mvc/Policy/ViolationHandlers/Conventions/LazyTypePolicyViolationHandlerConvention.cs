using System;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public abstract class LazyTypePolicyViolationHandlerConvention<TPolicyViolationHandler> : PolicyViolationHandlerTypeConvention where TPolicyViolationHandler : class, IPolicyViolationHandler
	{
		public Func<PolicyResult, bool> Predicate { get; private set; }

		protected LazyTypePolicyViolationHandlerConvention() : this(pr => true) {}

		protected LazyTypePolicyViolationHandlerConvention(Func<PolicyResult, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException("predicate");
			Predicate = predicate;
		}

		public override Type GetHandlerTypeFor(PolicyViolationException exception)
		{
			return Predicate.Invoke(exception.PolicyResult) ? typeof (TPolicyViolationHandler) : null;
		}
	}
}