using System;

namespace FluentSecurity
{
	public class PolicyViolationException : Exception
	{
		internal PolicyViolationException(PolicyResult policyResult) : base(policyResult.Message)
		{
			PolicyResult = policyResult;
			PolicyType = PolicyResult.PolicyType;
		}

		public PolicyResult PolicyResult { get; private set; }
		public Type PolicyType { get; private set; }
	}
}