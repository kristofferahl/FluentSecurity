using System;
using FluentSecurity.Policy;

namespace FluentSecurity.TestHelper.Expectations
{
	public class HasTypeExpectation<TSecurityPolicy> : HasTypeExpectation where TSecurityPolicy : ISecurityPolicy
	{
		public HasTypeExpectation() : base(typeof(TSecurityPolicy)) {}
	}

	public class HasTypeExpectation : IExpectation
	{
		public Type Type { get; private set; }

		protected HasTypeExpectation(Type type)
		{
			Type = type;
		}
	}
}