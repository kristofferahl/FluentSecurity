using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using FluentSecurity.Caching;
using FluentSecurity.Configuration;
using FluentSecurity.Diagnostics;
using FluentSecurity.Internals;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
using FluentSecurity.Scanning;
using FluentSecurity.Scanning.TypeScanners;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	public abstract class ConfigurationExpression
	{
		public AdvancedConfiguration Advanced { get; private set; }

		internal SecurityRuntime Runtime { get; private set; }
		
		internal IPolicyAppender PolicyAppender { get; set; }

		internal void Initialize(SecurityRuntime runtime)
		{
			Runtime = runtime;
			Advanced = new AdvancedConfiguration(Runtime);
			PolicyAppender = new DefaultPolicyAppender();
		}

		public IPolicyContainerConfiguration For<TController>(Expression<Func<TController, object>> actionExpression) where TController : Controller
		{
			var controllerName = typeof(TController).GetControllerName();
			var actionName = actionExpression.GetActionName();

			return AddPolicyContainerFor(controllerName, actionName);
		}

		public IPolicyContainerConfiguration For<TController>(Expression<Action<TController>> actionExpression) where TController : Controller
		{
			var controllerName = typeof(TController).GetControllerName();
			var actionName = actionExpression.GetActionName();

			return AddPolicyContainerFor(controllerName, actionName);
		}

		public IPolicyContainerConfiguration For<TController>() where TController : Controller
		{
			var controllerType = typeof(TController);
			var controllerTypes = new[] { controllerType };

			return CreateConventionPolicyContainerFor(controllerTypes, defaultCacheLevel: By.Controller);
		}

		public virtual IPolicyContainerConfiguration ForAllControllers()
		{
			var assemblyScanner = new AssemblyScanner();
			assemblyScanner.TheCallingAssembly();
			assemblyScanner.With<ControllerTypeScanner>();
			var controllerTypes = assemblyScanner.Scan();

			return CreateConventionPolicyContainerFor(controllerTypes);
		}

		public IPolicyContainerConfiguration ForAllControllersInAssembly(params Assembly[] assemblies)
		{
			var assemblyScanner = new AssemblyScanner();
			assemblyScanner.Assemblies(assemblies);
			assemblyScanner.With<ControllerTypeScanner>();
			var controllerTypes = assemblyScanner.Scan();

			return CreateConventionPolicyContainerFor(controllerTypes);
		}

		public IPolicyContainerConfiguration ForAllControllersInAssemblyContainingType<TType>()
		{
			var assembly = typeof (TType).Assembly;
			return ForAllControllersInAssembly(assembly);
		}

		public IPolicyContainerConfiguration ForAllControllersInheriting<TController>(Expression<Func<TController, object>> actionExpression, params Assembly[] assemblies) where TController : Controller
		{
			if (actionExpression == null) throw new ArgumentNullException("actionExpression");
			var actionName = actionExpression.GetActionName();
			return ForAllControllersInheriting<TController>(action => action == actionName, assemblies);
		}

		public IPolicyContainerConfiguration ForAllControllersInheriting<TController>(params Assembly[] assemblies) where TController : Controller
		{
			Func<string, bool> actionFilter = actionName => true;
			return ForAllControllersInheriting<TController>(actionFilter, assemblies);
		}

		private IPolicyContainerConfiguration ForAllControllersInheriting<TController>(Func<string, bool> actionFilter, IEnumerable<Assembly> assemblies) where TController : Controller
		{
			var controllerType = typeof (TController);

			var assembliesToScan = assemblies.ToList();
			if (!assembliesToScan.Any())
				assembliesToScan.Add(controllerType.Assembly);

			var assemblyScanner = new AssemblyScanner();
			assemblyScanner.Assemblies(assembliesToScan);
			assemblyScanner.With(new ControllerTypeScanner(controllerType));
			var controllerTypes = assemblyScanner.Scan();

			Func<ControllerActionInfo, bool> filter = info => actionFilter.Invoke(info.ActionName);

			return CreateConventionPolicyContainerFor(controllerTypes, filter);
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

		public IPolicyContainerConfiguration ForActionsMatching(Func<ControllerActionInfo, bool> actionFilter, params Assembly[] assemblies)
		{
			var assemblyScanner = new AssemblyScanner();
			var assembliesToScan = assemblies.ToList();
			
			if (assembliesToScan.Any()) 
				assemblyScanner.Assemblies(assemblies);
			else
				assemblyScanner.TheCallingAssembly();

			assemblyScanner.With<ControllerTypeScanner>();
			var controllerTypes = assemblyScanner.Scan();

			return CreateConventionPolicyContainerFor(controllerTypes, actionFilter);
		}

		public void Scan(Action<ProfileScanner> scan)
		{
			Publish.ConfigurationEvent(() => "Scanning for profiles");
			var profileScanner = new ProfileScanner();
			scan.Invoke(profileScanner);
			var profiles = profileScanner.Scan().ToList();
			profiles.ForEach(ApplyProfile);
		}

		public void ApplyProfile<TSecurityProfile>() where TSecurityProfile : SecurityProfile, new()
		{
			var profile = new TSecurityProfile();
			Publish.ConfigurationEvent(() => "Applying profile {0}.".FormatWith(profile.GetType().FullName));
			Runtime.ApplyConfiguration(profile);
		}

		private void ApplyProfile(Type profileType)
		{
			var profile = Activator.CreateInstance(profileType) as SecurityProfile;
			if (profile != null)
			{
				Publish.ConfigurationEvent(() => "Applying profile {0}.".FormatWith(profile.GetType().FullName));
				Runtime.ApplyConfiguration(profile);
			}
		}

		internal IPolicyContainerConfiguration CreateConventionPolicyContainerFor(IEnumerable<Type> controllerTypes, Func<ControllerActionInfo, bool> actionFilter = null, By defaultCacheLevel = By.Policy)
		{
			var policyContainers = new List<IPolicyContainerConfiguration>();
			foreach (var controllerType in controllerTypes)
			{
				var controllerName = controllerType.GetControllerName();
				var actionMethods = controllerType.GetActionMethods(actionFilter);

				policyContainers.AddRange(
					actionMethods.Select(actionMethod => AddPolicyContainerFor(controllerName, actionMethod.GetActionName()))
					);
			}

			return new ConventionPolicyContainer(policyContainers, defaultCacheLevel);
		}

		private PolicyContainer AddPolicyContainerFor(string controllerName, string actionName)
		{
			return Runtime.AddPolicyContainer(new PolicyContainer(controllerName, actionName, PolicyAppender));
		}

		public void GetAuthenticationStatusFrom(Func<bool> authenticationExpression)
		{
			if (authenticationExpression == null)
				throw new ArgumentNullException("authenticationExpression");

			Runtime.IsAuthenticated = authenticationExpression;
		}

		public void GetRolesFrom(Func<IEnumerable<object>> rolesExpression)
		{
			if (rolesExpression == null)
				throw new ArgumentNullException("rolesExpression");

			if (Runtime.PolicyContainers.Any())
				throw new ConfigurationErrorsException("You must set the rolesfunction before adding policies.");

			Runtime.Roles = rolesExpression;
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

			ResolveServicesUsing(new ExternalServiceLocator(servicesLocator, singleServiceLocator));
		}

		public void ResolveServicesUsing(ISecurityServiceLocator securityServiceLocator)
		{
			if (securityServiceLocator == null)
				throw new ArgumentNullException("securityServiceLocator");

			Runtime.ExternalServiceLocator = securityServiceLocator;
		}

		public void DefaultPolicyViolationHandlerIs<TPolicyViolationHandler>() where TPolicyViolationHandler : class, IPolicyViolationHandler
		{
			RemoveDefaultPolicyViolationHandlerConventions();
			Advanced.Conventions(conventions =>
				conventions.Add(new DefaultPolicyViolationHandlerIsOfTypeConvention<TPolicyViolationHandler>())
				);
		}

		public void DefaultPolicyViolationHandlerIs<TPolicyViolationHandler>(Func<TPolicyViolationHandler> policyViolationHandler) where TPolicyViolationHandler : class, IPolicyViolationHandler
		{
			RemoveDefaultPolicyViolationHandlerConventions();
			Advanced.Conventions(conventions =>
				conventions.Add(new DefaultPolicyViolationHandlerIsInstanceConvention<TPolicyViolationHandler>(policyViolationHandler))
				);
		}

		private void RemoveDefaultPolicyViolationHandlerConventions()
		{
			Advanced.Conventions(conventions =>
			{
				conventions.RemoveAll(c => c is FindDefaultPolicyViolationHandlerByNameConvention);
				conventions.RemoveAll(c => c.IsMatchForGenericType(typeof (DefaultPolicyViolationHandlerIsOfTypeConvention<>)));
				conventions.RemoveAll(c => c.IsMatchForGenericType(typeof (DefaultPolicyViolationHandlerIsInstanceConvention<>)));
			});
		}
	}
}