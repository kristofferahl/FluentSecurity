using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	public class SecurityContext : ISecurityContext
	{
		private readonly ExpandoObject _data;
		private readonly Func<bool> _isAuthenticated;
		private readonly Func<IEnumerable<object>> _roles;

		// TODO: Create context from SecurityModel
		private SecurityContext(ConfigurationExpression configurationExpression)
		{
			_data = new ExpandoObject();
			_isAuthenticated = configurationExpression.Model.IsAuthenticated;
			_roles = configurationExpression.Model.Roles;

			var modifyer = configurationExpression.Model.SecurityContextModifyer;
			if (modifyer != null) modifyer.Invoke(this);
		}

		public dynamic Data
		{
			get { return _data; }
		}

		public bool CurrentUserIsAuthenticated()
		{
			return _isAuthenticated();
		}

		public IEnumerable<object> CurrentUserRoles()
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
				var externalServiceLocator = configurationExpression.Model.ExternalServiceLocator;
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

					context = new SecurityContext(configurationExpression);
				}
			}
			
			return context;
		}

		private static bool CanCreateSecurityContextFromConfigurationExpression(ConfigurationExpression expression)
		{
			return expression.Model.IsAuthenticated != null;
		}
	}
}