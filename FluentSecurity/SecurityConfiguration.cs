using System;
using System.Collections.Generic;

namespace FluentSecurity
{
	public class SecurityConfiguration : ISecurityConfiguration
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
			return WhatDoIHaveBuilder.WhatDoIHave(_configuration);
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

		public IWhatDoIHaveBuilder WhatDoIHaveBuilder
		{
			get
			{
				EnsureConfigured();
				return _configuration.WhatDoIHaveBuilder;
			}
		}

		private void EnsureConfigured()
		{
			if (_configuration == null)
				throw new InvalidOperationException("You must configure the builder before calling GetPolicyContainers");
		}
	}
}