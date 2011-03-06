using System;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public class PolicyViolationException<TSecurityPolicy> : PolicyViolationException where TSecurityPolicy : ISecurityPolicy
	{
		public PolicyViolationException(string message) : base(typeof(TSecurityPolicy), message) {}
	}

	public class PolicyViolationException : Exception
	{
		internal PolicyViolationException(Type policyType, string message) : base(message)
		{
			PolicyType = policyType;
		}

		public Type PolicyType { get; private set; }
	}
}