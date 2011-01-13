using System;
using FluentSecurity.Policy;

namespace FluentSecurity.TestHelper.Expectations
{
	public class DoesNotHaveTypeExpectation<TSecurityPolicy> : DoesNotHaveTypeExpectation where TSecurityPolicy : ISecurityPolicy
	{
		public DoesNotHaveTypeExpectation() : base(typeof(TSecurityPolicy)) { }
	}

	public class DoesNotHaveTypeExpectation : IExpectation
	{
		public Type Type { get; private set; }

		protected DoesNotHaveTypeExpectation(Type type)
		{
			Type = type;
		}
	}
}