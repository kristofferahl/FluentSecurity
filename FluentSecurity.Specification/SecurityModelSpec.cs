using System.Linq;
using FluentSecurity.Caching;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("SecurityModelSpec")]
	public class When_creating_a_new_security_model
	{
		private SecurityRuntime _runtime;

		[SetUp]
		public void SetUp()
		{
			_runtime = TestDataFactory.CreateSecurityRuntime();
		}

		[Test]
		public void Should_not_have_any_policy_containers()
		{
			Assert.That(_runtime.Profiles.Count(), Is.EqualTo(0));
		}

		[Test]
		public void Should_not_have_any_profiles()
		{
			Assert.That(_runtime.Profiles.Count(), Is.EqualTo(0));
		}

		[Test]
		public void Should_not_have_any_conventions()
		{
			Assert.That(_runtime.Conventions.Count(), Is.EqualTo(0));
		}

		[Test]
		public void Should_not_ignore_missing_configurations()
		{
			Assert.That(_runtime.ShouldIgnoreMissingConfiguration, Is.False);
		}

		[Test]
		public void Should_have_default_policy_cache_lifecycle_set_to_DoNotCache()
		{
			Assert.That(_runtime.DefaultResultsCacheLifecycle, Is.EqualTo(Cache.DoNotCache));
		}

		[Test]
		public void Should_not_have_a_security_context_modifyer()
		{
			Assert.That(_runtime.SecurityContextModifyer, Is.Null);
		}
	}
}