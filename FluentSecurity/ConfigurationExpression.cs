using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using FluentSecurity.Caching;
using FluentSecurity.Configuration;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
using FluentSecurity.Scanning;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	public class ConfigurationExpression
	{
		internal IList<IPolicyContainer> PolicyContainers { get; private set; } 
		internal Func<bool> IsAuthenticated { get; private set; }
		internal Func<IEnumerable<object>> Roles { get; private set; }
		internal ISecurityServiceLocator ExternalServiceLocator { get; private set; }
		
		private IPolicyAppender PolicyAppender { get; set; }

		public AdvancedConfiguration Advanced { get; set; }

		public ConfigurationExpression()
		{
			PolicyContainers = new List<IPolicyContainer>();
			Advanced = new AdvancedConfiguration();
			PolicyAppender = new DefaultPolicyAppender();
		}

		public IPolicyContainerConfiguration For<TController>(Expression<Func<TController, object>> actionExpression) where TController : Controller
		{
			var controllerName = typeof(TController).GetControllerName();
			var actionName = actionExpression.GetActionName();

			return AddPolicyContainerFor(controllerName, actionName);
		}

		public IPolicyContainerConfiguration For<TController>() where TController : Controller
		{
			var controllerType = typeof(TController);
			var controllerTypes = new[] { controllerType };

			return CreateConventionPolicyContainerFor(controllerTypes, By.Controller);
		}

		public IPolicyContainerConfiguration ForAllControllers()
		{
			var assemblyScanner = new AssemblyScanner();
			assemblyScanner.TheCallingAssembly();
			assemblyScanner.With<ControllerTypeScanner>();
			var controllerTypes = assemblyScanner.Scan();

			return CreateConventionPolicyContainerFor(controllerTypes);
		}

		public IPolicyContainerConfiguration ForAllControllersInAssembly(Assembly assembly)
		{
			var assemblyScanner = new AssemblyScanner();
			assemblyScanner.Assembly(assembly);
			assemblyScanner.With<ControllerTypeScanner>();
			var controllerTypes = assemblyScanner.Scan();

			return CreateConventionPolicyContainerFor(controllerTypes);
		}

		public IPolicyContainerConfiguration ForAllControllersInAssemblyContainingType<TType>()
		{
			var assembly = typeof (TType).Assembly;
			return ForAllControllersInAssembly(assembly);
		}

		public IPolicyContainerConfiguration ForAllControllersInNamespaceContainingType<TType>()
		{
			var assembly = typeof (TType).Assembly;

			var assemblyScanner = new AssemblyScanner();
			assemblyScanner.Assembly(assembly);
			assemblyScanner.With<ControllerTypeScanner>();
			assemblyScanner.IncludeNamespaceContainingType<TType>();
			var controllerTypes = assemblyScanner.Scan();

			return CreateConventionPolicyContainerFor(controllerTypes);
		}

		private IPolicyContainerConfiguration CreateConventionPolicyContainerFor(IEnumerable<Type> controllerTypes, By defaultCacheLevel = By.Policy)
		{
			var policyContainers = new List<IPolicyContainerConfiguration>();
			foreach (var controllerType in controllerTypes)
			{
				var controllerName = controllerType.GetControllerName();
				var actionMethods = controllerType.GetActionMethods();

				policyContainers.AddRange(
					actionMethods.Select(actionMethod => AddPolicyContainerFor(controllerName, actionMethod.Name))
					);
			}

			return new ConventionPolicyContainer(policyContainers, defaultCacheLevel);
		}

		private PolicyContainer AddPolicyContainerFor(string controllerName, string actionName)
		{
			PolicyContainer policyContainer;

			var existingContainer = PolicyContainers.GetContainerFor(controllerName, actionName);
			if (existingContainer != null)
			{
				policyContainer = (PolicyContainer) existingContainer;
			}
			else
			{
				policyContainer = new PolicyContainer(controllerName, actionName, PolicyAppender);
				PolicyContainers.Add(policyContainer);
			}

			return policyContainer;
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

			if (PolicyContainers.Count > 0)
				throw new ConfigurationErrorsException("You must set the rolesfunction before adding policies.");

			Roles = rolesFunction;
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

		public void DefaultPolicyViolationHandlerIs<TPolicyViolationHandler>() where TPolicyViolationHandler : class, IPolicyViolationHandler
		{
			RemoveDefaultPolicyViolationHandlerConventions();
			Advanced.Conventions.Add(new DefaultPolicyViolationHandlerIsOfTypeConvention<TPolicyViolationHandler>());
		}

		public void DefaultPolicyViolationHandlerIs<TPolicyViolationHandler>(Func<TPolicyViolationHandler> policyViolationHandler) where TPolicyViolationHandler : class, IPolicyViolationHandler
		{
			RemoveDefaultPolicyViolationHandlerConventions();
			Advanced.Conventions.Add(new DefaultPolicyViolationHandlerIsInstanceConvention<TPolicyViolationHandler>(policyViolationHandler));
		}

		private void RemoveDefaultPolicyViolationHandlerConventions()
		{
			Advanced.Conventions.RemoveAll(c => c is FindDefaultPolicyViolationHandlerByNameConvention);
			Advanced.Conventions.RemoveAll(c => c.IsMatchForGenericType(typeof(DefaultPolicyViolationHandlerIsOfTypeConvention<>)));
			Advanced.Conventions.RemoveAll(c => c.IsMatchForGenericType(typeof(DefaultPolicyViolationHandlerIsInstanceConvention<>)));
		}
	}
}