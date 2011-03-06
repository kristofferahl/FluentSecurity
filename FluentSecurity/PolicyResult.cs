using System;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public class PolicyResult
	{
		private PolicyResult() {}

		public bool ViolationOccured { get; private set; }
		public string Message { get; private set; }
		public ISecurityPolicy Policy { get; private set; }

		public static PolicyResult CreateSuccessResult(ISecurityPolicy policy)
		{
			return new PolicyResult
			{
				Policy = policy,
				ViolationOccured = false,
				Message = null
			};
		}

		public static PolicyResult CreateFailureResult(ISecurityPolicy policy, string message)
		{
			if (policy == null) throw new ArgumentNullException("policy", "A policy must be provided.");
			if (String.IsNullOrEmpty(message)) throw new ArgumentNullException("message", "A failure message must be provided.");
			
			return new PolicyResult
			{
				Policy = policy,
				ViolationOccured = true,
				Message = message
			};
		}
	}
}