using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using FluentSecurity.Scanning;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	public class ConfigurationExpression : Builder<IPolicyContainer>
	{
		internal Func<bool> IsAuthenticated { get; private set; }
		internal Func<IEnumerable<object>> Roles { get; private set; }
		internal ISecurityServiceLocator ExternalServiceLocator { get; private set; }
		internal bool ShouldIgnoreMissingConfiguration { get; private set; }
		private IPolicyAppender PolicyAppender { get; set; }

		public ConfigurationExpression()
		{
			PolicyAppender = new DefaultPolicyAppender();
		}

		public IPolicyContainer For<TController>(Expression<Func<TController, object>> propertyExpression) where TController : Controller
		{
            var controllerType = typeof(TController);
            var areaName = controllerType.GetAreaName();
            var controllerName = controllerType.GetControllerName();
			var actionName = propertyExpression.GetActionName();

            return AddPolicyContainerFor(areaName, controllerName, actionName);
		}

		public IConventionPolicyContainer For<TController>() where TController : Controller
		{
			var controllerType = typeof(TController);
			var areaName = controllerType.GetAreaName();
			var controllerName = controllerType.GetControllerName();
			var actionMethods = controllerType.GetActionMethods();

			var policyContainers = new List<IPolicyContainer>();
			foreach (var actionMethod in actionMethods)
			{
				var actionName = actionMethod.Name;
                var policyContainer = AddPolicyContainerFor(areaName, controllerName, actionName);
				policyContainers.Add(policyContainer);
			}

			return new ConventionPolicyContainer(policyContainers);
		}

		public IConventionPolicyContainer ForAllControllers()
		{
			var assemblyScanner = new AssemblyScanner();
			assemblyScanner.TheCallingAssembly();
			assemblyScanner.With<ControllerTypeScanner>();
			var controllerTypes = assemblyScanner.Scan();

			var policyContainers = new List<IPolicyContainer>();
			foreach (var controllerType in controllerTypes)
			{
				var areaName = controllerType.GetAreaName();
				var controllerName = controllerType.GetControllerName();
				var actionMethods = controllerType.GetActionMethods();

				policyContainers.AddRange(
                    actionMethods.Select(actionMethod => AddPolicyContainerFor(areaName, controllerName, actionMethod.Name))
					);
			}

			return new ConventionPolicyContainer(policyContainers);
		}

		public IConventionPolicyContainer ForAllControllersInAssembly(Assembly assembly)
		{
			var assemblyScanner = new AssemblyScanner();
			assemblyScanner.Assembly(assembly);
			assemblyScanner.With<ControllerTypeScanner>();
			var controllerTypes = assemblyScanner.Scan();

			var policyContainers = new List<IPolicyContainer>();
			foreach (var controllerType in controllerTypes)
			{
				var areaName = controllerType.GetAreaName();
				var controllerName = controllerType.GetControllerName();
				var actionMethods = controllerType.GetActionMethods();

				policyContainers.AddRange(
                    actionMethods.Select(actionMethod => AddPolicyContainerFor(areaName, controllerName, actionMethod.Name))
					);
			}

			return new ConventionPolicyContainer(policyContainers);
		}

		public IConventionPolicyContainer ForAllControllersInAssemblyContainingType<TType>()
		{
			var assembly = typeof (TType).Assembly;
			return ForAllControllersInAssembly(assembly);
		}

		private IPolicyContainer AddPolicyContainerFor(string areaName, string controllerName, string actionName)
		{
			IPolicyContainer policyContainer;

            var existingContainer = _itemValues.GetContainerFor(areaName, controllerName, actionName);
			if (existingContainer != null)
			{
				policyContainer = existingContainer;
			}
			else
			{
                policyContainer = new PolicyContainer(areaName, controllerName, actionName, PolicyAppender);
				_itemValues.Add(policyContainer);
			}

			return policyContainer;
		}

		public void RemovePoliciesFor<TController>(Expression<Func<TController, object>> actionExpression) where TController : Controller
		{
		    var controllerType = typeof (TController);
            var areaName = controllerType.GetAreaName();
            var controllerName = controllerType.GetControllerName();
			var actionName = actionExpression.GetActionName();

            var policyContainer = _itemValues.GetContainerFor(areaName, controllerName, actionName);
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

		public void GetRolesFrom(Func<IEnumerable<object>> rolesFunction)
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