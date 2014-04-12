using System;
using FluentSecurity.Caching;
using FluentSecurity.Configuration;
using FluentSecurity.ServiceLocation;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	public class When_creating_a_SecurityRuntime : SecurityRuntimeSpecification
	{
		[Test]
		public void Should_not_ignore_missing_configuration()
		{
			Assert.That(Runtime.ShouldIgnoreMissingConfiguration, Is.False);
		}

		[Test]
		public void Should_not_cache_results_by_default()
		{
			Assert.That(Runtime.DefaultResultsCacheLifecycle, Is.EqualTo(Cache.DoNotCache));
		}

		[Test]
		public void Should_not_have_any_profiles()
		{
			Assert.That(Runtime.Profiles, Is.Empty);
		}

		[Test]
		public void Should_not_have_any_policy_containers()
		{
			Assert.That(Runtime.PolicyContainers, Is.Empty);
		}

		[Test]
		public void Should_not_have_any_conventions()
		{
			Assert.That(Runtime.Conventions, Is.Empty);
		}
	}

	public class When_applying_ConventionConfiguration_to_SecurityRuntime : SecurityRuntimeSpecification
	{
		[Test]
		public void Should_throw_when_null()
		{
			// Arrange
			Action<ConventionConfiguration> configuration = null;

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => Runtime.ApplyConfiguration(configuration));
		}
	}

	public class When_applying_ViolationConfiguration_to_SecurityRuntime : SecurityRuntimeSpecification
	{
		[Test]
		public void Should_throw_when_null()
		{
			// Arrange
			Action<ViolationConfiguration> configuration = null;

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => Runtime.ApplyConfiguration(configuration));
		}
	}

	public class When_applying_a_SecurityProfile_to_SecurityRuntime : SecurityRuntimeSpecification
	{
		[Test]
		public void Should_throw_when_null()
		{
			// Arrange
			SecurityProfile configuration = null;

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => Runtime.ApplyConfiguration(configuration));
		}
	}

	public class When_adding_a_PolicyContainer_to_SecurityRuntime : SecurityRuntimeSpecification
	{
		[Test]
		public void Should_throw_when_null()
		{
			// Arrange
			PolicyContainer policyContainer = null;

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => Runtime.AddPolicyContainer(policyContainer));
		}
	}

	[TestFixture]
	[Category("SecurityRuntimeSpec")]
	public abstract class SecurityRuntimeSpecification
	{
		internal SecurityRuntime Runtime;

		[SetUp]
		public void SetUp()
		{
			Runtime = new SecurityRuntime(new SecurityCache(new MvcLifecycleResolver()), new MvcTypeFactory());
		}
	}
}