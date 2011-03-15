using System.Collections.Generic;

namespace FluentSecurity.ServiceLocation
{
	public class ServiceLocator
	{
		private static ServiceLocator _serviceLocator;

		public ServiceLocator()
		{
			IContainer container = new Container();
			
			container.Register<ISecurityConfiguration>(ctx => SecurityConfigurator.CurrentConfiguration);
			container.Register<ISecurityHandler>(ctx => new SecurityHandler());
			
			container.Register<ISecurityContext>(ctx => SecurityContext.CreateFrom(ctx.Resolve<ISecurityConfiguration>()));

			container.Register<IPolicyViolationHandlerSelector>(ctx => new PolicyViolationHandlerSelector(
				ctx.Resolve<ISecurityConfiguration>(),
				ctx.ResolveAll<IPolicyViolationHandler>()
				));

			container.Register<IWhatDoIHaveBuilder>(ctx => new DefaultWhatDoIHaveBuilder(), LifeCycle.Singleton);

			container.SetPrimarySource(ctx => ctx.Resolve<ISecurityConfiguration>().ExternalServiceLocator);

			Container = container;
		}

		private IContainer Container { get; set; }

		internal static ServiceLocator Current
		{
			get { return _serviceLocator ?? (_serviceLocator = new ServiceLocator()); }
		}

		public static void Reset()
		{
			_serviceLocator = null;
		}

		public TTypeToResolve Resolve<TTypeToResolve>()
		{
			return Container.Resolve<TTypeToResolve>();
		}

		public IEnumerable<TTypeToResolve> ResolveAll<TTypeToResolve>()
		{
			return Container.ResolveAll<TTypeToResolve>();
		}
	}
}