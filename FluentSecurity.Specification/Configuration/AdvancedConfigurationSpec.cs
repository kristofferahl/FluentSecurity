using System;
using System.Linq;
using FluentSecurity.Caching;
using FluentSecurity.Configuration;
using FluentSecurity.Policy;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification.Configuration
{
	[TestFixture]
	[Category("AdvancedConfigurationSpec")]
	public class When_creating_a_new_advanced_configuration
	{
		private AdvancedConfiguration _advancedConfiguration;

		[SetUp]
		public void SetUp()
		{
			_advancedConfiguration = new AdvancedConfiguration();
		}

		[Test]
		public void Should_not_ignore_missing_configurations()
		{
			Assert.That(_advancedConfiguration.ShouldIgnoreMissingConfiguration, Is.False);
		}

		[Test]
		public void Should_have_default_policy_cache_lifecycle_set_to_DoNotCache()
		{
			Assert.That(_advancedConfiguration.DefaultResultsCacheLifecycle, Is.EqualTo(Cache.DoNotCache));
		}

		[Test]
		public void Should_not_have_a_security_context_modifyer()
		{
			Assert.That(_advancedConfiguration.SecurityContextModifyer, Is.Null);
		}

		[Test]
		public void Should_have_conventions_for_default_PolicyViolationHandler_applied()
		{
			var conventions = _advancedConfiguration.Conventions.OfType<IPolicyViolationHandlerConvention>().ToList();
			Assert.That(conventions.ElementAtOrDefault(0), Is.TypeOf<FindByPolicyNameConvention>());
			Assert.That(conventions.ElementAtOrDefault(1), Is.TypeOf<FindDefaultPolicyViolationHandlerByNameConvention>());
		}
	}

	[TestFixture]
	[Category("AdvancedConfigurationSpec")]
	public class When_ignoring_missing_configuration
	{
		[Test]
		public void Should_ignore_missing_configurations()
		{
			// Arrange
			var advancedConfiguration = new AdvancedConfiguration();

			// Act
			advancedConfiguration.IgnoreMissingConfiguration();

			// Assert
			Assert.That(advancedConfiguration.ShouldIgnoreMissingConfiguration, Is.True);
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

	[TestFixture]
	[Category("AdvancedConfigurationSpec")]
	public class When_specifying_how_violations_are_handled
	{
		private AdvancedConfiguration _advancedConfiguration;

		[SetUp]
		public void SetUp()
		{
			_advancedConfiguration = new AdvancedConfiguration();
		}

		[Test]
		public void Should_throw_when_null_is_passed_to_Violations()
		{
			Assert.Throws<ArgumentNullException>(() => _advancedConfiguration.Violations(null));
		}

		[Test]
		public void Should_always_add_conventions_at_the_first_position_to_support_cascading_overrides()
		{
			// Act
			_advancedConfiguration.Violations(violations =>
			{
				violations.AddConvention(new Convention(expectedIndex: 2));
				violations.AddConvention(new Convention(expectedIndex: 1));
				violations.AddConvention(new Convention(expectedIndex: 0));
			});

			// Assert
			Assert.That(_advancedConfiguration.Conventions.ElementAt(0).As<Convention>().ExpectedIndex, Is.EqualTo(0));
			Assert.That(_advancedConfiguration.Conventions.ElementAt(1).As<Convention>().ExpectedIndex, Is.EqualTo(1));
			Assert.That(_advancedConfiguration.Conventions.ElementAt(2).As<Convention>().ExpectedIndex, Is.EqualTo(2));
		}

		[Test]
		public void Should_add_convention_for_IgnorePolicy_and_Handler1()
		{
			// Act
			_advancedConfiguration.Violations(violations => violations.Of<IgnorePolicy>().IsHandledBy<Handler1>());

			// Assert
			Assert.That(_advancedConfiguration.Conventions.First(), Is.InstanceOf<PolicyTypeToPolicyViolationHandlerTypeConvention<IgnorePolicy, Handler1>>());
		}

		[Test]
		public void Should_add_convention_for_IgnorePolicy_and_Handler2()
		{
			// Act
			_advancedConfiguration.Violations(violations => violations.Of<IgnorePolicy>().IsHandledBy(() => new Handler2()));

			// Assert
			Assert.That(_advancedConfiguration.Conventions.First(), Is.InstanceOf<PolicyTypeToPolicyViolationHandlerInstanceConvention<IgnorePolicy, Handler2>>());
		}

		public class Convention : IPolicyViolationHandlerConvention
		{
			public int ExpectedIndex { get; private set; }

			public Convention(int expectedIndex)
			{
				ExpectedIndex = expectedIndex;
			}

			public IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception)
			{
				throw new NotImplementedException();
			}
		}

		public class Handler1 : DefaultPolicyViolationHandler {}

		public class Handler2 : DefaultPolicyViolationHandler {}
	}
}