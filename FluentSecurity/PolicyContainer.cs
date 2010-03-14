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
		private readonly Func<bool> _isAuthenticatedFunction;
		private readonly Func<object[]> _rolesFunction;

		public PolicyContainer(string controllerName, string actionName, Func<bool> isAuthenticatedFunction, Func<object[]> rolesFunction, IPolicyManager policyManager)
		{
			if (controllerName.IsNullOrEmpty())
				throw new ArgumentException("Controllername must not be null or empty!", "controllerName");

			if (actionName.IsNullOrEmpty())
				throw new ArgumentException("Actionname must not be null or empty!", "actionName");

			if (isAuthenticatedFunction == null)
				throw new ArgumentNullException("isAuthenticatedFunction");

			if (policyManager == null)
				throw new ArgumentNullException("policyManager");

			_policies = new List<ISecurityPolicy>();

			ControllerName = controllerName;
			ActionName = actionName;
			
			_isAuthenticatedFunction = isAuthenticatedFunction;
			_rolesFunction = rolesFunction;

			Manager = policyManager;
		}

		public string ControllerName { get; private set; }
		public string ActionName { get; private set; }
		public IPolicyManager Manager { get; private set; }

		public void EnforcePolicies()
		{
			if (_policies.Count.Equals(0))
				throw new ConfigurationErrorsException("You must add at least 1 policy for controller {0} action {1}.".FormatWith(ControllerName, ActionName));

			var isAuthenticated = _isAuthenticatedFunction.Invoke();
			
			object[] roles = null;
			if (_rolesFunction != null)
			{
				roles = _rolesFunction.Invoke();
			}
			
			foreach (var policy in _policies)
			{
				policy.Enforce(isAuthenticated, roles);
			}
		}

		public IPolicyContainer AddPolicy(ISecurityPolicy securityPolicy)
		{
			Manager.UpdatePolicies(securityPolicy, _policies);

			return this;
		}

		public IEnumerable<ISecurityPolicy> GetPolicies()
		{
			return new ReadOnlyCollection<ISecurityPolicy>(_policies);
		}
	}
}