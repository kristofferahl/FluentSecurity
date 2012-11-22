using System.Linq;
using FluentSecurity.Caching;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("SecurityModelSpec")]
	public class When_creating_a_new_security_model
	{
		private SecurityModel _securityModel;

		[SetUp]
		public void SetUp()
		{
			_securityModel = new SecurityModel();
		}

		[Test]
		public void Should_not_have_any_policy_containers()
		{
			Assert.That(_securityModel.Profiles.Count(), Is.EqualTo(0));
		}

		[Test]
		public void Should_not_have_any_profiles()
		{
			Assert.That(_securityModel.Profiles.Count(), Is.EqualTo(0));
		}

		[Test]
		public void Should_not_have_any_conventions()
		{
			Assert.That(_securityModel.Conventions.Count(), Is.EqualTo(0));
		}

		[Test]
		public void Should_not_ignore_missing_configurations()
		{
			Assert.That(_securityModel.ShouldIgnoreMissingConfiguration, Is.False);
		}

		[Test]
		public void Should_have_default_policy_cache_lifecycle_set_to_DoNotCache()
		{
			Assert.That(_securityModel.DefaultResultsCacheLifecycle, Is.EqualTo(Cache.DoNotCache));
		}

		[Test]
		public void Should_not_have_a_security_context_modifyer()
		{
			Assert.That(_securityModel.SecurityContextModifyer, Is.Null);
		}
	}
}