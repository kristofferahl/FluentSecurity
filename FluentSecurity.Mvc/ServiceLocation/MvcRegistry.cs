using System.Linq;
using System.Web.Mvc;
using FluentSecurity.Diagnostics;
using FluentSecurity.Policy.ViolationHandlers;
using FluentSecurity.Policy.ViolationHandlers.Conventions;

namespace FluentSecurity.ServiceLocation
{
	public class MvcRegistry : IRegistry
	{
		public IContainer Configure()
		{
			IContainer container = new Container(new MvcLifecycleResolver());

			container.Register<ISecurityConfiguration>(ctx => SecurityConfiguration.Current);
			container.Register<ISecurityHandler<ActionResult>>(ctx => new SecurityHandler(), Lifecycle.Singleton);

			container.Register<ISecurityContext>(ctx => SecurityContext.CreateFrom(ctx.Resolve<ISecurityConfiguration>()));

			container.Register<IPolicyViolationHandler>(ctx => new DelegatePolicyViolationHandler(ctx.ResolveAll<IPolicyViolationHandler>()), Lifecycle.Singleton);

			container.Register<IPolicyViolationHandlerSelector<ActionResult>>(ctx => new PolicyViolationHandlerSelector(
				ctx.Resolve<ISecurityConfiguration>().Runtime.Conventions.OfType<IPolicyViolationHandlerConvention>()
				));

			container.Register<IWhatDoIHaveBuilder>(ctx => new DefaultWhatDoIHaveBuilder(), Lifecycle.Singleton);

			container.SetPrimarySource(ctx => ctx.Resolve<ISecurityConfiguration>().Runtime.ExternalServiceLocator);

			return container;
		}
	}
}