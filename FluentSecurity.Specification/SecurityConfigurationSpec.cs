using System;
using System.Linq;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("SecurityConfigurationSpec")]
	public class When_creating_a_new_SecurityConfiguration
	{
		private static SecurityConfiguration Because()
		{
			return new SecurityConfiguration();
		}

		[Test]
		public void Should_throw_when_getting_policycontainers()
		{
			// Act
			var builder = Because();

			// Assert
			Assert.Throws<InvalidOperationException>(() => { var x = builder.PolicyContainers; });
		}

		[Test]
		public void Should_throw_when_getting_value_of_ignore_missing_configuration()
		{
			// Act
			var builder = Because();

			// Assert
			Assert.Throws<InvalidOperationException>(() => { var x = builder.IgnoreMissingConfiguration; });
		}

		[Test]
		public void Should_throw_when_getting_policymanager()
		{
			// Act
			var builder = Because();

			// Assert
			Assert.Throws<InvalidOperationException>(() => { var x = builder.PolicyManager; });
		}

		[Test]
		public void Should_throw_when_getting_whatdoihavebuilder()
		{
			// Act
			var builder = Because();

			// Assert
			Assert.Throws<InvalidOperationException>(() => { var x = builder.WhatDoIHaveBuilder; });
		}

		[Test]
		public void Should_throw_when_calling_whatdoihave()
		{
			// Act
			var builder = Because();

			// Assert
			Assert.Throws<InvalidOperationException>(() => builder.WhatDoIHave());
		}
	}

	[TestFixture]
	[Category("SecurityConfigurationSpec")]
	public class When_calling_configure
	{
		private static SecurityConfiguration _securityConfiguration;

		[SetUp]
		public void SetUp()
		{
			_securityConfiguration = TestDataFactory.CreateValidSecurityConfiguration();
		}

		private static void Because()
		{
			_securityConfiguration.Configure(policy => { });
		}

		[Test]
		public void Should_not_contain_any_policycontainers()
		{
			// Act
			Because();

			// Assert
			var containers = _securityConfiguration.PolicyContainers.Count();
			Assert.That(containers, Is.EqualTo(0));
		}

		[Test]
		public void Should_not_ignore_missing_configurations()
		{
			// Act
			Because();

			// Assert
			Assert.That(_securityConfiguration.IgnoreMissingConfiguration, Is.False);
		}

		[Test]
		public void Should_have_PolicyManager_set_to_DefaultPolicyManager()
		{
			// Arrange
			var expectedPolicyManagerType = typeof(DefaultPolicyManager);

			// Act
			Because();

			// Assert
			Assert.That(_securityConfiguration.PolicyManager, Is.TypeOf(expectedPolicyManagerType));
		}

		[Test]
		public void Should_have_WhatDoIHaveBuilder_set_to_DefaultWhatDoIHaveBuilder()
		{
			// Arrange
			var expectedWhatDoIHaveBuilderType = typeof(DefaultWhatDoIHaveBuilder);

			// Act
			Because();

			// Assert
			Assert.That(_securityConfiguration.WhatDoIHaveBuilder, Is.TypeOf(expectedWhatDoIHaveBuilderType));
		}
	}

	[TestFixture]
	[Category("SecurityConfigurationSpec")]
	public class When_calling_configure_with_ignore_missing_configuration
	{
		private static SecurityConfiguration _securityConfiguration;

		[SetUp]
		public void SetUp()
		{
			_securityConfiguration = TestDataFactory.CreateValidSecurityConfiguration();
		}

		private static void Because()
		{
			_securityConfiguration.Configure(policy => policy.IgnoreMissingConfiguration());
		}

		[Test]
		public void Should_ignore_missing_configurations()
		{
			// Act
			Because();

			// Assert
			Assert.That(_securityConfiguration.IgnoreMissingConfiguration, Is.True);
		}
	}

	[TestFixture]
	[Category("SecurityConfigurationSpec")]
	public class When_calling_reset
	{
		private static SecurityConfiguration _securityConfiguration;

		[SetUp]
		public void SetUp()
		{
			_securityConfiguration = TestDataFactory.CreateValidSecurityConfiguration();
		}

		private static void Because()
		{
			_securityConfiguration.Reset();
		}

		[Test]
		public void Should_throw_when_getting_policycontainers()
		{
			// Act
			Because();

			// Assert
			Assert.Throws<InvalidOperationException>(() => { var x = _securityConfiguration.PolicyContainers; });
		}

		[Test]
		public void Should_throw_when_getting_value_of_ignore_missing_configuration()
		{
			// Act
			Because();

			// Assert
			Assert.Throws<InvalidOperationException>(() => { var x = _securityConfiguration.IgnoreMissingConfiguration; });
		}

		[Test]
		public void Should_throw_when_getting_policymanager()
		{
			// Act
			Because();

			// Assert
			Assert.Throws<InvalidOperationException>(() => { var x = _securityConfiguration.PolicyManager; });
		}

		[Test]
		public void Should_throw_when_getting_whatdoihavebuilder()
		{
			// Act
			Because();

			// Assert
			Assert.Throws<InvalidOperationException>(() => { var x = _securityConfiguration.WhatDoIHaveBuilder; });
		}

		[Test]
		public void Should_throw_when_calling_whatdoihave()
		{
			// Act
			Because();

			// Assert
			Assert.Throws<InvalidOperationException>(() => _securityConfiguration.WhatDoIHave());
		}
	}

	[TestFixture]
	[Category("SecurityConfigurationSpec")]
	public class When_I_check_what_I_have
	{
		[Test]
		public void Should_return_the_current_configuration()
		{
			var configuration = TestDataFactory.CreateValidSecurityConfiguration();

			// Arrange
			configuration.Configure(policy =>
			{
				policy.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);
				policy.IgnoreMissingConfiguration();
				policy.For<BlogController>(x => x.DeletePost(0)).DenyAnonymousAccess().RequireRole(UserRole.Owner, UserRole.Publisher);
				policy.For<BlogController>(x => x.Index()).DenyAnonymousAccess();
			});

			// Act
			var whatIHave = configuration.WhatDoIHave();

			// Assert
			Assert.That(whatIHave.Replace("\r\n", "|").Replace("\t", "%"), Is.EqualTo("Ignore missing configuration: True||------------------------------------------------------------------------------------|BlogController > DeletePost|%RequireRole (Owner or Publisher)|BlogController > Index|%DenyAnonymousAccess|------------------------------------------------------------------------------------"));
		}
	}
}