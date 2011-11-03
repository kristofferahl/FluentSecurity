using System;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	[Obsolete("Will be removed for the 2.0 release")]
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

		internal PolicyViolationException(PolicyResult policyResult) : base(policyResult.Message)
		{
			PolicyResult = policyResult;
			PolicyType = PolicyResult.PolicyType;
		}

		public PolicyResult PolicyResult { get; private set; }
		public Type PolicyType { get; private set; }
	}
}