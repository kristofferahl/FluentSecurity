using System;
using FluentSecurity.Policy;

namespace FluentSecurity.TestHelper.Expectations
{
	public class HasTypeExpectation<TSecurityPolicy> : HasTypeExpectation where TSecurityPolicy : class, ISecurityPolicy
	{
		public Func<TSecurityPolicy, bool> Predicate { get; private set; }

		public HasTypeExpectation() : base(typeof(TSecurityPolicy))
		{
			Predicate = securityPolicy => securityPolicy.GetType() == Type;
		}

		public HasTypeExpectation(Func<TSecurityPolicy, bool> predicate) : base(typeof(TSecurityPolicy))
		{
			Predicate = predicate;
		}

		protected override bool EvaluatePredicate(ISecurityPolicy securityPolicy)
		{
			var policy = securityPolicy as TSecurityPolicy;
			return policy != null && Predicate.Invoke(policy);
		}
	}

	public abstract class HasTypeExpectation : IExpectation
	{
		public Type Type { get; private set; }

		protected HasTypeExpectation(Type type)
		{
			Type = type;
		}

		public bool IsMatch(ISecurityPolicy securityPolicy)
		{
			return EvaluatePredicate(securityPolicy);
		}

		protected abstract bool EvaluatePredicate(ISecurityPolicy securityPolicy);
	}
}