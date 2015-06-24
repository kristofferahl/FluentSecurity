using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Http;
using FluentSecurity.Caching;
using FluentSecurity.Configuration;
using FluentSecurity.Core;
using FluentSecurity.Core.Internals;
using FluentSecurity.Diagnostics;
using FluentSecurity.Internals;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
using FluentSecurity.Scanning;
using FluentSecurity.Scanning.TypeScanners;
using FluentSecurity.WebApi.Policy.ViolationHandlers;

namespace FluentSecurity.WebApi.Configuration
{
	public abstract class WebApiConfigurationExpression : ConfigurationExpressionBase<WebApiSecurityRuntime, AdvancedConfiguration>
	{
		private Func<IControllerNameResolver> _controllerNameResolver;
		private Func<IActionNameResolver> _actionNameResolver;
		private Func<IActionResolver> _actionResolver;

		internal void Initialize(WebApiSecurityRuntime runtime)
		{
			Initialize(runtime, new AdvancedConfiguration(runtime));

			_controllerNameResolver = runtime.Container.Resolve<IControllerNameResolver>;
			_actionNameResolver = runtime.Container.Resolve<IActionNameResolver>;
			_actionResolver = runtime.Container.Resolve<IActionResolver>;
		}

		public IPolicyContainerConfiguration For<TController>(Expression<Func<TController, object>> actionExpression) where TController : ApiController
		{
			var controllerName = _controllerNameResolver().Resolve(typeof(TController));
			var actionName = _actionNameResolver().Resolve(actionExpression);

			return AddPolicyContainerFor(controllerName, actionName);
		}

		public IPolicyContainerConfiguration For<TController>(Expression<Action<TController>> actionExpression) where TController : ApiController
		{
			var controllerName = _controllerNameResolver().Resolve(typeof(TController));
			var actionName = _actionNameResolver().Resolve(actionExpression);

			return AddPolicyContainerFor(controllerName, actionName);
		}

		public IPolicyContainerConfiguration For<TController>() where TController : ApiController
		{
			var controllerType = typeof(TController);
			var controllerTypes = new[] { controllerType };

			return CreateConventionPolicyContainerFor(controllerTypes, defaultCacheLevel: By.Controller);
		}

		public virtual IPolicyContainerConfiguration ForAllControllers()
		{
			var assemblyScanner = Runtime.Container.Resolve<IAssemblyScanner>();
			var controllerTypeScanner = Runtime.Container.Resolve<IControllerTypeScanner>();

			assemblyScanner.TheCallingAssembly();
			assemblyScanner.With(controllerTypeScanner);
			var controllerTypes = assemblyScanner.Scan();

			return CreateConventionPolicyContainerFor(controllerTypes);
		}

		public IPolicyContainerConfiguration ForAllControllersInAssembly(params Assembly[] assemblies)
		{
			var assemblyScanner = Runtime.Container.Resolve<IAssemblyScanner>();
			var controllerTypeScanner = Runtime.Container.Resolve<IControllerTypeScanner>();

			assemblyScanner.Assemblies(assemblies);
			assemblyScanner.With(controllerTypeScanner);
			var controllerTypes = assemblyScanner.Scan();

			return CreateConventionPolicyContainerFor(controllerTypes);
		}

		public IPolicyContainerConfiguration ForAllControllersInAssemblyContainingType<TType>()
		{
			var assembly = typeof (TType).Assembly;
			return ForAllControllersInAssembly(assembly);
		}

		public IPolicyContainerConfiguration ForAllControllersInheriting<TController>(Expression<Func<TController, object>> actionExpression, params Assembly[] assemblies) where TController : ApiController
		{
			if (actionExpression == null) throw new ArgumentNullException("actionExpression");
			var actionName = _actionNameResolver().Resolve(actionExpression);
			return ForAllControllersInheriting<TController>(action => action == actionName, assemblies);
		}

		public IPolicyContainerConfiguration ForAllControllersInheriting<TController>(params Assembly[] assemblies) where TController : ApiController
		{
			Func<string, bool> actionFilter = actionName => true;
			return ForAllControllersInheriting<TController>(actionFilter, assemblies);
		}

		private IPolicyContainerConfiguration ForAllControllersInheriting<TController>(Func<string, bool> actionFilter, IEnumerable<Assembly> assemblies) where TController : ApiController
		{
			var assemblyScanner = Runtime.Container.Resolve<IAssemblyScanner>();
			var controllerTypeScanner = Runtime.Container.Resolve<IControllerTypeScanner>();

			var controllerType = typeof (TController);
			controllerTypeScanner.SetControllerType(controllerType);

			var assembliesToScan = assemblies.ToList();
			if (!assembliesToScan.Any())
				assembliesToScan.Add(controllerType.Assembly);

			assemblyScanner.Assemblies(assembliesToScan);
			assemblyScanner.With(controllerTypeScanner);
			var controllerTypes = assemblyScanner.Scan();

			Func<ControllerActionInfo, bool> filter = info => actionFilter.Invoke(info.ActionName);

			return CreateConventionPolicyContainerFor(controllerTypes, filter);
		}

		public IPolicyContainerConfiguration ForAllControllersInNamespaceContainingType<TType>()
		{
			var assemblyScanner = Runtime.Container.Resolve<IAssemblyScanner>();
			var controllerTypeScanner = Runtime.Container.Resolve<IControllerTypeScanner>();

			var assembly = typeof (TType).Assembly;

			assemblyScanner.Assembly(assembly);
			assemblyScanner.With(controllerTypeScanner);
			assemblyScanner.IncludeNamespaceContainingType<TType>();
			var controllerTypes = assemblyScanner.Scan();

			return CreateConventionPolicyContainerFor(controllerTypes);
		}

		public IPolicyContainerConfiguration ForActionsMatching(Func<ControllerActionInfo, bool> actionFilter, params Assembly[] assemblies)
		{
			var assemblyScanner = Runtime.Container.Resolve<IAssemblyScanner>();
			var controllerTypeScanner = Runtime.Container.Resolve<IControllerTypeScanner>();

			var assembliesToScan = assemblies.ToList();

			if (assembliesToScan.Any())
				assemblyScanner.Assemblies(assemblies);
			else
				assemblyScanner.TheCallingAssembly();

			assemblyScanner.With(controllerTypeScanner);
			var controllerTypes = assemblyScanner.Scan();

			return CreateConventionPolicyContainerFor(controllerTypes, actionFilter);
		}

		public void Scan(Action<IProfileAssemblyScannerConfiguration> scan)
		{
			var profileScanner = Runtime.Container.Resolve<IProfileAssemblyScanner>();
			Publish.ConfigurationEvent(() => String.Format("Scanning for profiles using {0}.", profileScanner.GetType().FullName));
			scan.Invoke(profileScanner);
			var profiles = profileScanner.Scan().ToList();
			profiles.ForEach(ApplyProfile);
		}

		public void ApplyProfile<TSecurityProfile>() where TSecurityProfile : WebApiSecurityProfile, new()
		{
			var profile = new TSecurityProfile();
			Publish.ConfigurationEvent(() => "Applying profile {0}.".FormatWith(profile.GetType().FullName));
			Runtime.ApplyConfiguration(profile);
		}

		private void ApplyProfile(Type profileType)
		{
			var profile = Activator.CreateInstance(profileType) as WebApiSecurityProfile;
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
				var controllerName = _controllerNameResolver().Resolve(controllerType);
				var actionMethods = _actionResolver().Resolve(controllerType, actionFilter);

				policyContainers.AddRange(
					actionMethods.Select(actionMethod => AddPolicyContainerFor(controllerName, _actionNameResolver().Resolve(actionMethod)))
					);
			}

			var policyContainer = new ConventionPolicyContainer(policyContainers, Runtime.LazySecurityPolicyFactory, defaultCacheLevel);
			return policyContainer;
		}

		private IPolicyContainerConfiguration AddPolicyContainerFor(string controllerName, string actionName)
		{
			var policyContainer = new PolicyContainer(controllerName, actionName, PolicyAppender, Runtime.LazySecurityPolicyFactory);
			return Runtime.AddPolicyContainer(policyContainer);
		}

		public void DefaultPolicyViolationHandlerIs<TPolicyViolationHandler>() where TPolicyViolationHandler : class, IWebApiPolicyViolationHandler
		{
			RemoveDefaultPolicyViolationHandlerConventions();
			Advanced.Conventions(conventions =>
				conventions.Add(new DefaultPolicyViolationHandlerIsOfTypeConvention<TPolicyViolationHandler>())
				);
		}

		public void DefaultPolicyViolationHandlerIs<TPolicyViolationHandler>(Func<TPolicyViolationHandler> policyViolationHandler) where TPolicyViolationHandler : class, IWebApiPolicyViolationHandler
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