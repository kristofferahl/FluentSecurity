using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq.Expressions;
using System.Web.Mvc;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	public class ConfigurationExpression : Builder<IPolicyContainer>
	{
		public Func<bool> IsAuthenticated { get; private set; }
		public Func<object[]> Roles { get; private set; }
		public ISecurityServiceLocator ExternalServiceLocator { get; private set; }
		public bool ShouldIgnoreMissingConfiguration { get; private set; }
		public IPolicyAppender PolicyAppender { get; private set; }

		public ConfigurationExpression()
		{
			PolicyAppender = new DefaultPolicyAppender();
		}

		public IPolicyContainer For<TController>(Expression<Func<TController, object>> propertyExpression) where TController : Controller
		{
			var controllerName = typeof(TController).GetControllerName();
			var actionName = propertyExpression.GetActionName();

			return AddPolicyContainerFor(controllerName, actionName);
		}

		public IConventionPolicyContainer For<TController>() where TController : Controller
		{
			var controllerType = typeof(TController);
			var controllerName = controllerType.GetControllerName();
			var actionMethods = controllerType.GetActionMethods();

			var policyContainers = new List<IPolicyContainer>();
			foreach (var actionMethod in actionMethods)
			{
				var actionName = actionMethod.Name;
				var policyContainer = AddPolicyContainerFor(controllerName, actionName);
				policyContainers.Add(policyContainer);
			}

			return new ConventionPolicyContainer(policyContainers);
		}

		private IPolicyContainer AddPolicyContainerFor(string controllerName, string actionName)
		{
			IPolicyContainer policyContainer;

			var existingContainer = _itemValues.GetContainerFor(controllerName, actionName);
			if (existingContainer != null)
			{
				policyContainer = existingContainer;
			}
			else
			{
				policyContainer = new PolicyContainer(controllerName, actionName, PolicyAppender);
				_itemValues.Add(policyContainer);
			}

			return policyContainer;
		}

		public void RemovePoliciesFor<TController>(Expression<Func<TController, object>> actionExpression) where TController : Controller
		{
			var controllerName = typeof(TController).GetControllerName();
			var actionName = actionExpression.GetActionName();

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

			IsAuthenticated = isAuthenticatedFunction;
		}

		public void GetRolesFrom(Func<object[]> rolesFunction)
		{
			if (rolesFunction == null)
				throw new ArgumentNullException("rolesFunction");

			if (_itemValues.Count > 0)
				throw new ConfigurationErrorsException("You must set the rolesfunction before adding policies.");

			Roles = rolesFunction;
		}

		public void IgnoreMissingConfiguration()
		{
			ShouldIgnoreMissingConfiguration = true;
		}

		public void SetPolicyAppender(IPolicyAppender policyAppender)
		{
			if (policyAppender == null)
				throw new ArgumentNullException("policyAppender");
			
			PolicyAppender = policyAppender;
		}

		public void ResolveServicesUsing(Func<Type, IEnumerable<object>> servicesLocator, Func<Type, object> singleServiceLocator = null)
		{
			if (servicesLocator == null)
				throw new ArgumentNullException("servicesLocator");

			ExternalServiceLocator = new ExternalServiceLocator(servicesLocator, singleServiceLocator);
		}

		public void ResolveServicesUsing(ISecurityServiceLocator securityServiceLocator)
		{
			if (securityServiceLocator == null)
				throw new ArgumentNullException("securityServiceLocator");

			ExternalServiceLocator = securityServiceLocator;
		}
	}
}