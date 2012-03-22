using FluentSecurity.Caching;
using FluentSecurity.Policy;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("PolicyResultCacheKeyBuilderSpec")]
	public class When_creating_a_cache_key_for_AdminController_Login_TestPolicy
	{
		[Test]
		public void Should_be_PolicyResult_AdminController_Login_TestPolicy()
		{
			// Arrange
			var policy = new TestPolicy();
			var manifest = new CacheManifest("AdminController", "Login", policy.GetType(), Cache.DoNotCache);
			var context = TestDataFactory.CreateSecurityContext(true);

			// Act
			var cacheKey = PolicyResultCacheKeyBuilder.CreateFromManifest(manifest, policy, context);

			// Assert
			Assert.That(cacheKey, Is.EqualTo("PolicyResult_AdminController_Login_" + NameHelper.Policy<TestPolicy>()));
		}

		public class TestPolicy : ISecurityPolicy
		{
			public PolicyResult Enforce(ISecurityContext context)
			{
				throw new System.NotImplementedException();
			}
		}
	}

	[TestFixture]
	[Category("PolicyResultCacheKeyBuilderSpec")]
	public class When_creating_a_cache_key_for_BlogController_Post_TestPolicy
	{
		[Test]
		public void Should_be_PolicyResult_BlogController_Post_TestPolicy()
		{
			// Arrange
			var policy = new TestPolicy();
			var manifest = new CacheManifest("BlogController", "Post", policy.GetType(), Cache.DoNotCache);
			var context = TestDataFactory.CreateSecurityContext(true);

			// Act
			var cacheKey = PolicyResultCacheKeyBuilder.CreateFromManifest(manifest, policy, context);

			// Assert
			Assert.That(cacheKey, Is.EqualTo("PolicyResult_BlogController_Post_" + NameHelper.Policy<TestPolicy>()));
		}

		public class TestPolicy : ISecurityPolicy
		{
			public PolicyResult Enforce(ISecurityContext context)
			{
				throw new System.NotImplementedException();
			}
		}
	}
}