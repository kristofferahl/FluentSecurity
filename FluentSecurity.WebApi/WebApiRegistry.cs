using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Diagnostics;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity.WebApi
{
	public class WebApiRegistry : IRegistry
	{
		public void Configure(IContainer container)
		{
			container.Register<ISecurityConfiguration>(ctx => SecurityConfiguration.Get<WebApiConfiguration>());
			container.Register<ISecurityHandler<object>>(ctx => new WebApiSecurityHandler(), Lifecycle.Singleton);

			container.Register<ISecurityContext>(ctx => ctx.Resolve<ISecurityConfiguration>().CreateContext());

			//container.Register<IWebApiPolicyViolationHandler>(ctx => new DelegatePolicyViolationHandler(ctx.ResolveAll<IPolicyViolationHandler>()), Lifecycle.Singleton);

			container.Register<IPolicyViolationHandlerSelector<object>>(ctx => new WebApiPolicyViolationHandlerSelector(
				//ctx.Resolve<ISecurityConfiguration>().Runtime.Conventions.OfType<IPolicyViolationHandlerConvention>()
				));

			container.Register<IWhatDoIHaveBuilder>(ctx => new DefaultWhatDoIHaveBuilder(), Lifecycle.Singleton);
		}
	}
}