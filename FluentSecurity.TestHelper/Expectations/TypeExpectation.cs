using System;
using FluentSecurity.Policy;

namespace FluentSecurity.TestHelper.Expectations
{
	public abstract class TypeExpectation : IExpectation
	{
		public Type Type { get; private set; }
		public bool IsPredicateExpectation { get; private set; }

		protected TypeExpectation(Type type, bool isPredicateExpectation)
		{
			Type = type;
			IsPredicateExpectation = isPredicateExpectation;
		}

		public bool IsMatch(ISecurityPolicy securityPolicy)
		{
			return EvaluatePredicate(securityPolicy);
		}

		protected abstract bool EvaluatePredicate(ISecurityPolicy securityPolicy);
		public abstract string GetPredicateDescription();
	}
}