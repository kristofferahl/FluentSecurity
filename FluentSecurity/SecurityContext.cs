using System;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	public class SecurityContext : ISecurityContext
	{
		private readonly Func<bool> _isAuthenticated;
		private readonly Func<object[]> _roles;

		private SecurityContext(Func<bool> isAuthenticated, Func<object[]> roles)
		{
			if (isAuthenticated == null) throw new ArgumentNullException("isAuthenticated");

			_isAuthenticated = isAuthenticated;
			_roles = roles;
		}

		public bool CurrenUserAuthenticated()
		{
			return _isAuthenticated();
		}

		public object[] CurrenUserRoles()
		{
			return _roles != null ? _roles() : null;
		}

		public static ISecurityContext Current()
		{
			return ServiceLocator.Current.Resolve<ISecurityContext>();
		}

		internal static ISecurityContext CreateFrom(ISecurityConfiguration configuration)
		{
			ISecurityContext context = null;

			var expressionExposer = configuration as IExposeConfigurationExpression;
			if (expressionExposer != null)
			{
				var expression = expressionExposer.GetExpression();
				var externalServiceLocator = expression.ExternalServiceLocator;
				if (externalServiceLocator != null)
					context = externalServiceLocator.Resolve(typeof(ISecurityContext)) as ISecurityContext;

				if (context == null)
					context = new SecurityContext(expression.IsAuthenticated, expression.Roles);
			}
			
			return context;
		}
	}
}