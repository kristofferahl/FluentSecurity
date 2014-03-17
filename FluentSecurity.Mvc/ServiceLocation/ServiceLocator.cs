using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentSecurity.Diagnostics;
using FluentSecurity.Policy.ViolationHandlers;
using FluentSecurity.Policy.ViolationHandlers.Conventions;

namespace FluentSecurity.ServiceLocation
{
	public sealed class ServiceLocator
	{
		private static readonly object LockObject = new object();
		private static volatile ServiceLocator _serviceLocator;

		public ServiceLocator()
		{
			IContainer container = new Container(new MvcLifecycleResolver());
			
			container.Register<ISecurityConfiguration>(ctx => SecurityConfiguration.Current);
			container.Register<ISecurityHandler>(ctx => new SecurityHandler(), Lifecycle.Singleton);
			
			container.Register<ISecurityContext>(ctx => SecurityContext.CreateFrom(ctx.Resolve<ISecurityConfiguration>()));

			container.Register<IPolicyViolationHandler>(ctx => new DelegatePolicyViolationHandler(ctx.ResolveAll<IPolicyViolationHandler>()), Lifecycle.Singleton);

			container.Register<IPolicyViolationHandlerSelector<ActionResult>>(ctx => new PolicyViolationHandlerSelector(
				ctx.Resolve<ISecurityConfiguration>().Runtime.Conventions.OfType<IPolicyViolationHandlerConvention>()
				));

			container.Register<IWhatDoIHaveBuilder>(ctx => new DefaultWhatDoIHaveBuilder(), Lifecycle.Singleton);

			container.SetPrimarySource(ctx => ctx.Resolve<ISecurityConfiguration>().Runtime.ExternalServiceLocator);

			Container = container;
		}

		private IContainer Container { get; set; }

		internal static ServiceLocator Current
		{
			get
			{
				if (_serviceLocator == null)
				{
					lock (LockObject)
					{
						_serviceLocator = new ServiceLocator();
					}
				}
				return _serviceLocator;
			}
		}

		public static void Reset()
		{
			lock (LockObject)
			{
				_serviceLocator = null;
			}
		}

		public object Resolve(Type typeToResolve)
		{
			return Container.Resolve(typeToResolve);
		}

		public TTypeToResolve Resolve<TTypeToResolve>()
		{
			return Container.Resolve<TTypeToResolve>();
		}

		public IEnumerable<object> ResolveAll(Type typeToResolve)
		{
			return Container.ResolveAll(typeToResolve);
		}

		public IEnumerable<TTypeToResolve> ResolveAll<TTypeToResolve>()
		{
			return Container.ResolveAll<TTypeToResolve>();
		}
	}
}