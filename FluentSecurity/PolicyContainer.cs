using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public class PolicyContainer : IPolicyContainer
	{
		private readonly IList<ISecurityPolicy> _policies;

		public PolicyContainer(string areaName, string controllerName, string actionName, IPolicyAppender policyAppender)
		{
			if (controllerName.IsNullOrEmpty())
				throw new ArgumentException("Controllername must not be null or empty!", "controllerName");

			if (actionName.IsNullOrEmpty())
				throw new ArgumentException("Actionname must not be null or empty!", "actionName");

			if (policyAppender == null)
				throw new ArgumentNullException("policyAppender");

			_policies = new List<ISecurityPolicy>();

			AreaName = areaName;
			ControllerName = controllerName;
			ActionName = actionName;
			
			PolicyAppender = policyAppender;
		}

		public string AreaName { get; private set; }
		public string ControllerName { get; private set; }
		public string ActionName { get; private set; }
		public IPolicyAppender PolicyAppender { get; private set; }

		public IEnumerable<PolicyResult> EnforcePolicies(ISecurityContext context)
		{
			if (_policies.Count.Equals(0))
				throw new ConfigurationErrorsException("You must add at least 1 policy for area {0} controller {1} action {2}.".FormatWith(AreaName, ControllerName, ActionName));

			return _policies.Select(policy => policy.Enforce(context)).ToArray();
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