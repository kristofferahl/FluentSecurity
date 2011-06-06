using System;
using System.Collections.Generic;
using System.Configuration;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	public class SecurityContext : ISecurityContext
	{
		private readonly Func<bool> _isAuthenticated;
		private readonly Func<IEnumerable<object>> _roles;

		private SecurityContext(Func<bool> isAuthenticated, Func<IEnumerable<object>> roles)
		{
			_isAuthenticated = isAuthenticated;
			_roles = roles;
		}

		public bool CurrenUserAuthenticated()
		{
			return _isAuthenticated();
		}

		public IEnumerable<object> CurrenUserRoles()
		{
			return _roles != null ? _roles() : null;
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
				var configurationExpression = securityConfiguration.Expression;
				var externalServiceLocator = configurationExpression.ExternalServiceLocator;
				if (externalServiceLocator != null)
					context = externalServiceLocator.Resolve(typeof(ISecurityContext)) as ISecurityContext;

				if (context == null)
				{
					if (CanCreateSecurityContextFromConfigurationExpression(configurationExpression) == false)
						throw new ConfigurationErrorsException(
							@"
							The current configuration is invalid! Before using Fluent Security you must do one of the following.
							1) Specify how to get the authentication status using GetAuthenticationStatusFrom().
							2) Register an instance of ISecurityContext in your IoC-container and register your container using ResolveServicesUsing().
							");

					context = new SecurityContext(configurationExpression.IsAuthenticated, configurationExpression.Roles);
				}
			}
			
			return context;
		}

		private static bool CanCreateSecurityContextFromConfigurationExpression(ConfigurationExpression expression)
		{
			return expression.IsAuthenticated != null;
		}
	}
}