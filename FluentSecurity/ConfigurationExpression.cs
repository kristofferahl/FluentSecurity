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
	public class ConfigurationExpression : Builder<IPolicyContainer>
	{
		internal Func<bool> IsAuthenticated { get; private set; }
		internal Func<IEnumerable<object>> Roles { get; private set; }
		internal ISecurityServiceLocator ExternalServiceLocator { get; private set; }
		internal bool ShouldIgnoreMissingConfiguration { get; private set; }
		
		private IPolicyAppender PolicyAppender { get; set; }

		public AdvancedConfiguration Advanced { get; set; }

		public ConfigurationExpression()
		{
			Advanced = new AdvancedConfiguration();
			PolicyAppender = new DefaultPolicyAppender();
		}

		public IPolicyContainer For<TController>(Expression<Func<TController, object>> propertyExpression) where TController : Controller
		{
			var controllerName = typeof(TController).GetControllerName();
			var actionName = propertyExpression.GetActionName();

			return AddPolicyContainerFor(controllerName, actionName);
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

		public IConventionPolicyContainer For<TController>() where TController : Controller
		{
			var controllerType = typeof(TController);
			var controllerTypes = new[] { controllerType };

			return CreateConventionPolicyContainerFor(controllerTypes, By.Controller);
		}

		public IConventionPolicyContainer For<TController>(Func<MethodInfo, bool> actionsFilter) where TController : Controller
		{
			var controllerType = typeof(TController);
			var controllerTypes = new[] { controllerType };

			return CreateConventionPolicyContainerFor(controllerTypes, By.Controller, actionsFilter);
		}

		public IConventionPolicyContainer For<TController>(IEnumerable<Expression<Func<TController, object>>> propertyExpressions) where TController : Controller
		{
			var controllerType = typeof(TController);
			var controllerTypes = new[] { controllerType };

			return CreateConventionPolicyContainerFor(controllerTypes, By.Controller,
														f => propertyExpressions.Any(s => AreMethodEquals(s.GetAction(),f)));
		}

		private static bool AreMethodEquals(MethodInfo left, MethodInfo right)
		{
			//http://ayende.com/blog/2658/method-equality
			//http://stackoverflow.com/questions/4168489/methodinfo-equality-for-declaring-type

			if(left.Equals(right)) return true;

			left = left.ReflectedType == left.DeclaringType ? left : left.DeclaringType.GetMethod(left.Name, left.GetParameters().Select(p => p.ParameterType).ToArray());
			right = right.ReflectedType == right.DeclaringType ? right : right.DeclaringType.GetMethod(right.Name, right.GetParameters().Select(p => p.ParameterType).ToArray());
			
			return left.Equals(right);
		}

		public IConventionPolicyContainer ForAllControllers()
		{
			var assemblyScanner = new AssemblyScanner();
			assemblyScanner.TheCallingAssembly();
			assemblyScanner.With<ControllerTypeScanner>();
			var controllerTypes = assemblyScanner.Scan();

			return CreateConventionPolicyContainerFor(controllerTypes);
		}

		public IConventionPolicyContainer ForAllControllersInAssembly(Assembly assembly)
		{
			var assemblyScanner = new AssemblyScanner();
			assemblyScanner.Assembly(assembly);
			assemblyScanner.With<ControllerTypeScanner>();
			var controllerTypes = assemblyScanner.Scan();

			return CreateConventionPolicyContainerFor(controllerTypes);
		}

		public IConventionPolicyContainer ForAllControllersInAssemblyContainingType<TType>()
		{
			var assembly = typeof (TType).Assembly;
			return ForAllControllersInAssembly(assembly);
		}

		public IConventionPolicyContainer ForAllControllersInNamespaceContainingType<TType>()
		{
			var assembly = typeof (TType).Assembly;

			var assemblyScanner = new AssemblyScanner();
			assemblyScanner.Assembly(assembly);
			assemblyScanner.With<ControllerTypeScanner>();
			assemblyScanner.IncludeNamespaceContainingType<TType>();
			var controllerTypes = assemblyScanner.Scan();

			return CreateConventionPolicyContainerFor(controllerTypes);
		}

		private IConventionPolicyContainer CreateConventionPolicyContainerFor(IEnumerable<Type> controllerTypes, By defaultCacheLevel = By.Policy)
		{
			return CreateConventionPolicyContainerFor(controllerTypes, defaultCacheLevel, null);
		}

		private IConventionPolicyContainer CreateConventionPolicyContainerFor(IEnumerable<Type> controllerTypes, By defaultCacheLevel, Func<MethodInfo, bool> actionsFilter)
		{
			var policyContainers = new List<IPolicyContainer>();
			foreach(var controllerType in controllerTypes)
			{
				var controllerName = controllerType.GetControllerName();
				var actionMethods = controllerType.GetActionMethods()
					.Where(m => actionsFilter == null || actionsFilter(m));

				policyContainers.AddRange(
					actionMethods.Select(actionMethod => AddPolicyContainerFor(controllerName, actionMethod.Name))
					);
			}

			return new ConventionPolicyContainer(policyContainers, defaultCacheLevel);
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