using System;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public class PolicyViolationException<TSecurityPolicy> : PolicyViolationException where TSecurityPolicy : ISecurityPolicy
	{
		public PolicyViolationException(string message) : base(typeof(TSecurityPolicy), message) {}
	}

	public abstract class PolicyViolationException : Exception
	{
		protected PolicyViolationException(Type policyType, string message) : base(message)
		{
			PolicyType = policyType;
		}

		public Type PolicyType { get; private set; }
	}
}