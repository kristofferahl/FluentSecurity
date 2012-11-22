using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	public class SecurityContext : ISecurityContext
	{
		private readonly ExpandoObject _data;
		private readonly SecurityModel _model;

		private SecurityContext(SecurityModel model)
		{
			_data = new ExpandoObject();
			_model = model;

			var modifyer = model.SecurityContextModifyer;
			if (modifyer != null) modifyer.Invoke(this);
		}

		public dynamic Data
		{
			get { return _data; }
		}

		public bool CurrentUserIsAuthenticated()
		{
			return _model.IsAuthenticated.Invoke();
		}

		public IEnumerable<object> CurrentUserRoles()
		{
			return _model.Roles != null ? _model.Roles.Invoke() : null;
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
				var externalServiceLocator = securityConfiguration.Model.ExternalServiceLocator;
				if (externalServiceLocator != null)
					context = externalServiceLocator.Resolve(typeof(ISecurityContext)) as ISecurityContext;

				if (context == null)
				{
					if (securityConfiguration.Model.IsAuthenticated == null)
						throw new ConfigurationErrorsException(
							@"
							The current configuration is invalid! Before using Fluent Security you must do one of the following.
							1) Specify how to get the authentication status using GetAuthenticationStatusFrom().
							2) Register an instance of ISecurityContext in your IoC-container and register your container using ResolveServicesUsing().
							");

					context = new SecurityContext(securityConfiguration.Model);
				}
			}
			
			return context;
		}
	}
}