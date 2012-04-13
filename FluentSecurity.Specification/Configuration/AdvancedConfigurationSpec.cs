using System;
using FluentSecurity.Caching;
using FluentSecurity.Configuration;
using NUnit.Framework;

namespace FluentSecurity.Specification.Configuration
{
	[TestFixture]
	[Category("AdvancedConfigurationSpec")]
	public class When_creating_a_new_advanced_configuration
	{
		private IAdvancedConfiguration advancedConfiguration;

		[SetUp]
		public void SetUp()
		{
			advancedConfiguration = new AdvancedConfiguration();
		}

		[Test]
		public void Should_have_default_policy_cache_lifecycle_set_to_DoNotCache()
		{
			Assert.That(advancedConfiguration.DefaultResultsCacheLifecycle, Is.EqualTo(Cache.DoNotCache));
		}

		[Test]
		public void Should_not_have_a_security_context_modifyer()
		{
			Assert.That(advancedConfiguration.SecurityContextModifyer, Is.Null);
		}
	}

	[TestFixture]
	[Category("AdvancedConfigurationSpec")]
	public class When_setting_the_default_policy_results_cache_lifecycle
	{
		[Test]
		public void Should_have_default_policy_cache_lifecycle_set_to_PerHttpSession()
		{
			// Arrange
			var advancedConfiguration = new AdvancedConfiguration();

			// Act
			advancedConfiguration.SetDefaultResultsCacheLifecycle(Cache.PerHttpSession);

			// Assert
			Assert.That(advancedConfiguration.DefaultResultsCacheLifecycle, Is.EqualTo(Cache.PerHttpSession));
		}

		[Test]
		public void Should_have_default_policy_cache_lifecycle_set_to_PerHttpRequest()
		{
			// Arrange
			var advancedConfiguration = new AdvancedConfiguration();

			// Act
			advancedConfiguration.SetDefaultResultsCacheLifecycle(Cache.PerHttpRequest);
			
			// Assert
			Assert.That(advancedConfiguration.DefaultResultsCacheLifecycle, Is.EqualTo(Cache.PerHttpRequest));
		}

		[Test]
		public void Should_have_default_policy_cache_lifecycle_set_to_DoNotCache()
		{
			// Arrange
			var advancedConfiguration = new AdvancedConfiguration();

			// Act
			advancedConfiguration.SetDefaultResultsCacheLifecycle(Cache.DoNotCache);

			// Assert
			Assert.That(advancedConfiguration.DefaultResultsCacheLifecycle, Is.EqualTo(Cache.DoNotCache));
		}
	}

	[TestFixture]
	[Category("AdvancedConfigurationSpec")]
	public class When_specifying_a_security_context_modifyer
	{
		private Action<ISecurityContext> _expectedModifyer;

		[SetUp]
		public void SetUp()
		{
			// Arrange
			_expectedModifyer = c =>
			{
				c.Data.Property1 = "Value1";
				c.Data.Property2 = "Value2";
			};

			// Act
			SecurityConfigurator.Configure(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(() => true);
				configuration.Advanced.ModifySecurityContext(_expectedModifyer);
			});
		}

		[Test]
		public void Should_set_the_modifyer()
		{
			Assert.That(SecurityConfiguration.Current.Advanced.SecurityContextModifyer, Is.EqualTo(_expectedModifyer));
		}

		[Test]
		public void Should_not_throw_when_setting_the_modifyer_to_null()
		{
			Assert.DoesNotThrow(() => SecurityConfigurator.Configure(configuration => configuration.Advanced.ModifySecurityContext(null)));
		}

		[Test]
		public void Should_modify_context_on_creation()
		{
			// Assert
			var context = SecurityContext.Current;
			Assert.That(context.Data.Property1, Is.EqualTo("Value1"));
			Assert.That(context.Data.Property2, Is.EqualTo("Value2"));
		}
	}
}