using System;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public class PolicyResult
	{
		private PolicyResult() {}

		protected PolicyResult(string message, bool violationOccured, Type policyType)
		{
			Message = message;
			ViolationOccured = violationOccured;
			PolicyType = policyType;
		}

		protected PolicyResult(string message, bool violationOccured, ISecurityPolicy policy) 
			: this(message, violationOccured, policy.GetType()) {}

		public bool Cached { get; internal set; }
		public bool ViolationOccured { get; private set; }
		public string Message { get; private set; }
		public Type PolicyType { get; private set; }

		public static PolicyResult CreateSuccessResult(ISecurityPolicy policy)
		{
			return new PolicyResult
			{
				PolicyType = policy.GetType(),
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
				PolicyType = policy.GetType(),
				ViolationOccured = true,
				Message = message
			};
		}
	}
}