using System;
using System.Collections.Generic;

namespace FluentSecurity
{
	public class SecurityConfiguration : ISecurityConfiguration, IExposeConfigurationExpression
	{
		private ConfigurationExpression _configuration;

		public ISecurityConfiguration Configure(Action<ConfigurationExpression> configurationExpression)
		{
			var configuration = new ConfigurationExpression();
			configurationExpression(configuration);
			_configuration = configuration;
			return this;
		}

		public ISecurityConfiguration Reset()
		{
			_configuration = null;
			return this;
		}

		public string WhatDoIHave()
		{
			EnsureConfigured();
			return ServiceLocation.ServiceLocator.Current.Resolve<IWhatDoIHaveBuilder>().WhatDoIHave(_configuration);
		}

		public IEnumerable<IPolicyContainer> PolicyContainers
		{
			get
			{
				EnsureConfigured();
				return GetPolicyContainers();
			}
		}

		private IEnumerable<IPolicyContainer> GetPolicyContainers()
		{
			foreach (var policyContainer in _configuration)
				yield return policyContainer;
		}

		public bool IgnoreMissingConfiguration
		{
			get
			{
				EnsureConfigured();
				return _configuration.ShouldIgnoreMissingConfiguration;
			}
		}

		public IPolicyAppender PolicyAppender
		{
			get
			{
				EnsureConfigured();
				return _configuration.PolicyAppender;
			}
		}

		public ISecurityServiceLocator ExternalServiceLocator
		{
			get
			{
				EnsureConfigured();
				return _configuration.ExternalServiceLocator;
			}
		}

		private void EnsureConfigured()
		{
			if (_configuration == null) throw new InvalidOperationException("Security has not been configured!");
		}

		public ConfigurationExpression GetExpression()
		{
			EnsureConfigured();
			return _configuration;
		}
	}
}