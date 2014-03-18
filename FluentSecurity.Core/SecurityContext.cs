using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;

namespace FluentSecurity
{
	public class SecurityContext : ISecurityContext
	{
		private SecurityContext(ISecurityRuntime runtime)
		{
			Id = Guid.NewGuid();
			Data = new ExpandoObject();
			Runtime = runtime;

			var modifyer = runtime.SecurityContextModifyer;
			if (modifyer != null) modifyer.Invoke(this);
		}

		public Guid Id { get; private set; }
		public dynamic Data { get; private set; }
		public ISecurityRuntime Runtime { get; private set; }

		public bool CurrentUserIsAuthenticated()
		{
			return Runtime.IsAuthenticated.Invoke();
		}

		public IEnumerable<object> CurrentUserRoles()
		{
			return Runtime.Roles != null ? Runtime.Roles.Invoke() : null;
		}

		internal static ISecurityContext CreateFrom(ISecurityConfiguration configuration)
		{
			ISecurityContext context = null;

			if (configuration == null)
				throw new ArgumentNullException("configuration");

			var externalServiceLocator = configuration.Runtime.ExternalServiceLocator;
			if (externalServiceLocator != null)
				context = externalServiceLocator.Resolve(typeof(ISecurityContext)) as ISecurityContext;

			if (context == null)
			{
				if (configuration.Runtime.IsAuthenticated == null)
					throw new ConfigurationErrorsException(
						@"
						The current configuration is invalid! Before using Fluent Security you must do one of the following.
						1) Specify how to get the authentication status using GetAuthenticationStatusFrom().
						2) Register an instance of ISecurityContext in your IoC-container and register your container using ResolveServicesUsing().
						");

				context = new SecurityContext(configuration.Runtime);
			}

			return context;
		}
	}
}