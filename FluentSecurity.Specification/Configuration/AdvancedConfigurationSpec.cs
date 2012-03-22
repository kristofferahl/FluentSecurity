using FluentSecurity.Caching;
using FluentSecurity.Configuration;
using NUnit.Framework;

namespace FluentSecurity.Specification.Configuration
{
	[TestFixture]
	[Category("AdvancedConfigurationSpec")]
	public class When_creating_a_new_advanced_configuration
	{
		[Test]
		public void Should_have_default_policy_cache_lifecycle_set_to_DoNotCache()
		{
			// Arrange
			var advancedConfiguration = new AdvancedConfiguration();

			// Act & assert
			Assert.That(advancedConfiguration.DefaultResultsCacheLifecycle, Is.EqualTo(Cache.DoNotCache));
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
			advancedConfiguration.CacheResultsPerHttpSession();

			// Assert
			Assert.That(advancedConfiguration.DefaultResultsCacheLifecycle, Is.EqualTo(Cache.PerHttpSession));
		}

		[Test]
		public void Should_have_default_policy_cache_lifecycle_set_to_PerHttpRequest()
		{
			// Arrange
			var advancedConfiguration = new AdvancedConfiguration();

			// Act
			advancedConfiguration.CacheResultsPerHttpRequest();
			
			// Assert
			Assert.That(advancedConfiguration.DefaultResultsCacheLifecycle, Is.EqualTo(Cache.PerHttpRequest));
		}

		[Test]
		public void Should_have_default_policy_cache_lifecycle_set_to_DoNotCache()
		{
			// Arrange
			var advancedConfiguration = new AdvancedConfiguration();

			// Act
			advancedConfiguration.DoNotCacheCacheResults();

			// Assert
			Assert.That(advancedConfiguration.DefaultResultsCacheLifecycle, Is.EqualTo(Cache.DoNotCache));
		}
	}
}