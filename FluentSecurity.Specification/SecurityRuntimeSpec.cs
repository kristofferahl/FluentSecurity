using FluentSecurity.Caching;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("SecurityRuntimeSpec")]
	public class When_creating_a_SecurityRuntime
	{
		private SecurityRuntime _runtime;

		[SetUp]
		public void SetUp()
		{
			_runtime = new SecurityRuntime();
		}

		[Test]
		public void Should_not_ignore_missing_configuration()
		{
			Assert.That(_runtime.ShouldIgnoreMissingConfiguration, Is.False);
		}

		[Test]
		public void Should_not_cache_results_by_default()
		{
			Assert.That(_runtime.DefaultResultsCacheLifecycle, Is.EqualTo(Cache.DoNotCache));
		}

		[Test]
		public void Should_not_have_any_profiles()
		{
			Assert.That(_runtime.Profiles, Is.Empty);
		}

		[Test]
		public void Should_not_have_any_policy_containers()
		{
			Assert.That(_runtime.PolicyContainers, Is.Empty);
		}

		[Test]
		public void Should_not_have_any_conventions()
		{
			Assert.That(_runtime.Conventions, Is.Empty);
		}
	}
}