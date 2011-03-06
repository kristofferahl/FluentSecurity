using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public class PolicyContainer : IPolicyContainer
	{
		private readonly IList<ISecurityPolicy> _policies;
		private readonly Func<bool> _isAuthenticatedExpression;
		private readonly Func<object[]> _rolesExpression;

		public PolicyContainer(string controllerName, string actionName, Func<bool> isAuthenticatedExpression, Func<object[]> rolesExpression, IPolicyAppender policyAppender)
		{
			if (controllerName.IsNullOrEmpty())
				throw new ArgumentException("Controllername must not be null or empty!", "controllerName");

			if (actionName.IsNullOrEmpty())
				throw new ArgumentException("Actionname must not be null or empty!", "actionName");

			if (isAuthenticatedExpression == null)
				throw new ArgumentNullException("isAuthenticatedExpression");

			if (policyAppender == null)
				throw new ArgumentNullException("policyAppender");

			_policies = new List<ISecurityPolicy>();

			ControllerName = controllerName;
			ActionName = actionName;
			
			_isAuthenticatedExpression = isAuthenticatedExpression;
			_rolesExpression = rolesExpression;

			PolicyAppender = policyAppender;
		}

		public string ControllerName { get; private set; }
		public string ActionName { get; private set; }
		public IPolicyAppender PolicyAppender { get; private set; }

		public IEnumerable<PolicyResult> EnforcePolicies()
		{
			if (_policies.Count.Equals(0))
				throw new ConfigurationErrorsException("You must add at least 1 policy for controller {0} action {1}.".FormatWith(ControllerName, ActionName));

			var context = new SecurityContext(_isAuthenticatedExpression, _rolesExpression);
			var results = new List<PolicyResult>();
			
			foreach (var policy in _policies)
				results.Add(policy.Enforce(context));
			
			return results.ToArray();
		}

		public IPolicyContainer AddPolicy(ISecurityPolicy securityPolicy)
		{
			PolicyAppender.UpdatePolicies(securityPolicy, _policies);

			return this;
		}

		public IEnumerable<ISecurityPolicy> GetPolicies()
		{
			return new ReadOnlyCollection<ISecurityPolicy>(_policies);
		}
	}
}