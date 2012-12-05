using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	public class SecurityContext : ISecurityContext
	{
		private readonly ExpandoObject _data;
		private readonly ISecurityRuntime _runtime;

		private SecurityContext(ISecurityRuntime runtime)
		{
			_data = new ExpandoObject();
			_runtime = runtime;

			var modifyer = runtime.SecurityContextModifyer;
			if (modifyer != null) modifyer.Invoke(this);
		}

		public dynamic Data
		{
			get { return _data; }
		}

		public bool CurrentUserIsAuthenticated()
		{
			return _runtime.IsAuthenticated.Invoke();
		}

		public IEnumerable<object> CurrentUserRoles()
		{
			return _runtime.Roles != null ? _runtime.Roles.Invoke() : null;
		}

		public ISecurityRuntime Runtime
		{
			get { return _runtime; }
		}

		public static ISecurityContext Current
		{
			get
			{
				return ServiceLocator.Current.Resolve<ISecurityContext>();
			}
		}

		internal static ISecurityContext CreateFrom(ISecurityConfiguration configuration)
		{
			ISecurityContext context = null;

			var securityConfiguration = configuration as SecurityConfiguration;
			if (securityConfiguration != null)
			{
				var externalServiceLocator = securityConfiguration.Runtime.ExternalServiceLocator;
				if (externalServiceLocator != null)
					context = externalServiceLocator.Resolve(typeof(ISecurityContext)) as ISecurityContext;

				if (context == null)
				{
					if (securityConfiguration.Runtime.IsAuthenticated == null)
						throw new ConfigurationErrorsException(
							@"
							The current configuration is invalid! Before using Fluent Security you must do one of the following.
							1) Specify how to get the authentication status using GetAuthenticationStatusFrom().
							2) Register an instance of ISecurityContext in your IoC-container and register your container using ResolveServicesUsing().
							");

					context = new SecurityContext(securityConfiguration.Runtime);
				}
			}
			
			return context;
		}
	}
}