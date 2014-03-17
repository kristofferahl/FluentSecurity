using System;

namespace FluentSecurity
{
	public class PolicyViolationException : Exception
	{
		public PolicyViolationException(PolicyResult policyResult, ISecurityContext securityContext) : base(policyResult.Message)
		{
			PolicyResult = policyResult;
			SecurityContext = securityContext;
			PolicyType = PolicyResult.PolicyType;
		}

		public PolicyResult PolicyResult { get; private set; }
		public Type PolicyType { get; private set; }
		public ISecurityContext SecurityContext { get; private set; }
	}
}