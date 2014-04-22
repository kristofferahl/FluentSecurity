using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FluentSecurity.Configuration;
using FluentSecurity.Core;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	public abstract class ConfigurationExpressionBase<TSecurityRuntime, TAdvancedConfiguration>
		where TSecurityRuntime : SecurityRuntimeBase
		where TAdvancedConfiguration : AdvancedConfigurationBase<TSecurityRuntime>
	{
		// TODO: Should advanced configuration be available for ISecurityProfile's???
		public TAdvancedConfiguration Advanced { get; protected set; }

		public TSecurityRuntime Runtime { get; protected set; }
		public IPolicyAppender PolicyAppender { get; protected set; }

		protected void Initialize(TSecurityRuntime runtime, TAdvancedConfiguration advanced)
		{
			Advanced = advanced;
			Runtime = runtime;
			PolicyAppender = new DefaultPolicyAppender();
		}

		public void GetAuthenticationStatusFrom(Func<bool> authenticationExpression)
		{
			if (authenticationExpression == null)
				throw new ArgumentNullException("authenticationExpression");

			Runtime.IsAuthenticated = authenticationExpression;
		}

		public void GetRolesFrom(Func<IEnumerable<object>> rolesExpression)
		{
			if (rolesExpression == null)
				throw new ArgumentNullException("rolesExpression");

			if (Runtime.PolicyContainers.Any())
				throw new ConfigurationErrorsException("You must set the rolesfunction before adding policies.");

			Runtime.Roles = rolesExpression;
		}

		public void SetPolicyAppender(IPolicyAppender policyAppender)
		{
			if (policyAppender == null)
				throw new ArgumentNullException("policyAppender");

			PolicyAppender = policyAppender;
		}

		public void ResolveServicesUsing(Func<Type, IEnumerable<object>> servicesLocator, Func<Type, object> singleServiceLocator = null)
		{
			if (servicesLocator == null)
				throw new ArgumentNullException("servicesLocator");

			ResolveServicesUsing(new ExternalServiceLocator(servicesLocator, singleServiceLocator));
		}

		public void ResolveServicesUsing(ISecurityServiceLocator securityServiceLocator)
		{
			if (securityServiceLocator == null)
				throw new ArgumentNullException("securityServiceLocator");

			Runtime.ExternalServiceLocator = securityServiceLocator;
		}
	}
}