using System;
using System.Configuration;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace FluentSecurity
{
	public class PolicyBuilder : Builder<IPolicyContainer>
	{
		private Func<bool> _isAuthenticatedFunction;
		private Func<object[]> _rolesFunction;

		public IPolicyContainer For<TController>(Expression<Func<TController, object>> propertyExpression) where TController : Controller
		{
			if (_isAuthenticatedFunction == null)
				throw new ConfigurationErrorsException("You must specify a function returning authenticationstatus before adding policies.");

			var controllerName = typeof(TController).GetControllerName();
			var actionName = propertyExpression.GetActionName();

			var existingContainer = _itemValues.GetContainerFor(controllerName, actionName);
			if (existingContainer != null)
				throw new ConfigurationErrorsException("A policycontainer for {0} {1} has already been added.".FormatWith(controllerName, actionName));

			return AddPolicyContainerFor(controllerName, actionName);
		}

		private IPolicyContainer AddPolicyContainerFor(string controllerName, string actionName)
		{
			var policyContainer = new PolicyContainer(controllerName, actionName, _isAuthenticatedFunction, _rolesFunction);
			
			_itemValues.Add(policyContainer);

			return policyContainer;
		}

		public void RemovePoliciesFor<TController>(Expression<Func<TController, object>> propertyExpression) where TController : Controller
		{
			var controllerName = typeof(TController).GetControllerName();
			var actionName = propertyExpression.GetActionName();

			var policyContainer = _itemValues.GetContainerFor(controllerName, actionName);
			if (policyContainer != null)
			{
				_itemValues.Remove(policyContainer);
			}
		}

		public void GetAuthenticationStatusFrom(Func<bool> isAuthenticatedFunction)
		{
			if (isAuthenticatedFunction == null)
				throw new ArgumentNullException("isAuthenticatedFunction");

			_isAuthenticatedFunction = isAuthenticatedFunction;
		}

		public void GetRolesFrom(Func<object[]> rolesFunction)
		{
			if (rolesFunction == null)
				throw new ArgumentNullException("rolesFunction");

			if (_itemValues.Count > 0)
				throw new ConfigurationErrorsException("You must set the rolesfunction before adding policies.");

			_rolesFunction = rolesFunction;
		}

		public void IgnoreMissingConfiguration()
		{
			ShouldIgnoreMissingConfiguration = true;
		}

		public bool ShouldIgnoreMissingConfiguration { get; private set; }
	}
}