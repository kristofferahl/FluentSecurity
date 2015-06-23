using System.Linq;
using System.Web.Http.Controllers;
using FluentSecurity.Caching;
using FluentSecurity.Core;
using FluentSecurity.Diagnostics;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
using FluentSecurity.Scanning;
using FluentSecurity.Scanning.TypeScanners;
using FluentSecurity.ServiceLocation;
using FluentSecurity.WebApi.Configuration;
using FluentSecurity.WebApi.Policy.ViolationHandlers;
using FluentSecurity.WebApi.Scanning;
using FluentSecurity.WebApi.Scanning.TypeScanners;

namespace FluentSecurity.WebApi.ServiceLocation
{
	public class WebApiRegistry : IRegistry
	{
		public void Configure(IContainer container)
		{
			container.Register<IAssemblyScanner>(ctx => new WebApiAssemblyScanner(), Lifecycle.Transient);
			container.Register<IProfileAssemblyScanner>(ctx => new WebApiProfileScanner(), Lifecycle.Transient);

			container.Register<ISecurityConfiguration>(ctx => SecurityConfiguration.Get<WebApiConfiguration>());
			container.Register<ISecurityHandler<object>>(ctx => new WebApiSecurityHandler(), Lifecycle.Singleton);

			container.Register<ISecurityContext>(ctx => ctx.Resolve<ISecurityConfiguration>().CreateContext());

			container.Register<ISecurityCache>(ctx => new SecurityCache(ctx.Resolve<ILifecycleResolver>()), Lifecycle.Singleton);
			container.Register<ITypeFactory>(ctx => new WebApiTypeFactory(), Lifecycle.Singleton);

			var controllerTypeScanner = new WebApiControllerTypeScanner();
			var controllerNameResolver = new WebApiControllerNameResolver();
			var actionNameResolver = new WebApiActionNameResolver();
			var actionResolver = new WebApiActionResolver(actionNameResolver);

			container.Register<IControllerTypeScanner>(ctx => controllerTypeScanner);
			container.Register<IControllerNameResolver<HttpActionContext>>(ctx => controllerNameResolver, Lifecycle.Singleton);
			container.Register<IControllerNameResolver>(ctx => controllerNameResolver, Lifecycle.Singleton);
			container.Register<IActionNameResolver<HttpActionContext>>(ctx => actionNameResolver, Lifecycle.Singleton);
			container.Register<IActionNameResolver>(ctx => actionNameResolver, Lifecycle.Singleton);
			container.Register<IActionResolver>(ctx => actionResolver, Lifecycle.Singleton);

			//container.Register<IWebApiPolicyViolationHandler>(ctx => new DelegatePolicyViolationHandler(ctx.ResolveAll<IPolicyViolationHandler>()), Lifecycle.Singleton);

			container.Register<IPolicyViolationHandlerSelector<object>>(ctx => new WebApiPolicyViolationHandlerSelector(
				ctx.Resolve<ISecurityConfiguration>().Runtime.Conventions.OfType<IPolicyViolationHandlerConvention>()
				));

			container.Register<IWhatDoIHaveBuilder>(ctx => new DefaultWhatDoIHaveBuilder(), Lifecycle.Singleton);
		}
	}
}