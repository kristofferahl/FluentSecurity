using System.Globalization;
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
			var manifest = new PolicyResultCacheManifest("AdminController", "Login", policy.GetType(), Cache.DoNotCache);
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
	public class When_creating_a_cache_key_for_BlogController_Post_BlogAdminPolicy
	{
		[Test]
		public void Should_be_PolicyResult_BlogController_Post_BlogAdminPolicy()
		{
			// Arrange
			var policy = new BlogAdminPolicy();
			var manifest = new PolicyResultCacheManifest("BlogController", "Post", policy.GetType(), Cache.DoNotCache);
			var context = TestDataFactory.CreateSecurityContext(true);

			// Act
			var cacheKey = PolicyResultCacheKeyBuilder.CreateFromManifest(manifest, policy, context);

			// Assert
			Assert.That(cacheKey, Is.EqualTo("PolicyResult_BlogController_Post_" + NameHelper.Policy<BlogAdminPolicy>()));
		}

		public class BlogAdminPolicy : ISecurityPolicy
		{
			public PolicyResult Enforce(ISecurityContext context)
			{
				throw new System.NotImplementedException();
			}
		}
	}

	[TestFixture]
	[Category("PolicyResultCacheKeyBuilderSpec")]
	public class When_creating_a_cache_key_for_a_policy_that_implements_ICacheKeyProvider
	{
		[Test]
		public void Should_be_PolicyResult_BlogController_Post_BlogEditorPolicy_1_when_custom_cache_key_is_1()
		{
			// Arrange
			var policy = new BlogEditorPolicy("1");
			var manifest = new PolicyResultCacheManifest("BlogController", "Post", policy.GetType(), Cache.DoNotCache);
			var context = TestDataFactory.CreateSecurityContext(true);

			// Act
			var cacheKey = PolicyResultCacheKeyBuilder.CreateFromManifest(manifest, policy, context);

			// Assert
			Assert.That(cacheKey, Is.EqualTo("PolicyResult_BlogController_Post_" + NameHelper.Policy<BlogEditorPolicy>() + "_1"));
		}

		[Test]
		public void Should_be_PolicyResult_BlogController_Post_BlogEditorPolicy_2_when_custom_cache_key_is_2()
		{
			// Arrange
			var policy = new BlogEditorPolicy("2");
			var manifest = new PolicyResultCacheManifest("BlogController", "Post", policy.GetType(), Cache.DoNotCache);
			var context = TestDataFactory.CreateSecurityContext(true);

			// Act
			var cacheKey = PolicyResultCacheKeyBuilder.CreateFromManifest(manifest, policy, context);

			// Assert
			Assert.That(cacheKey, Is.EqualTo("PolicyResult_BlogController_Post_" + NameHelper.Policy<BlogEditorPolicy>() + "_2"));
		}

		[Test]
		public void Should_be_PolicyResult_BlogController_Post_BlogEditorPolicy_3_when_custom_cache_key_is_whitespace_3_whitespace()
		{
			// Arrange
			var policy = new BlogEditorPolicy("  3  ");
			var manifest = new PolicyResultCacheManifest("BlogController", "Post", policy.GetType(), Cache.DoNotCache);
			var context = TestDataFactory.CreateSecurityContext(true);

			// Act
			var cacheKey = PolicyResultCacheKeyBuilder.CreateFromManifest(manifest, policy, context);

			// Assert
			Assert.That(cacheKey, Is.EqualTo("PolicyResult_BlogController_Post_" + NameHelper.Policy<BlogEditorPolicy>() + "_3"));
		}

		[Test]
		public void Should_be_PolicyResult_BlogController_Post_BlogEditorPolicy_when_custom_cache_key_is_whitespace()
		{
			// Arrange
			var policy = new BlogEditorPolicy("    ");
			var manifest = new PolicyResultCacheManifest("BlogController", "Post", policy.GetType(), Cache.DoNotCache);
			var context = TestDataFactory.CreateSecurityContext(true);

			// Act
			var cacheKey = PolicyResultCacheKeyBuilder.CreateFromManifest(manifest, policy, context);

			// Assert
			Assert.That(cacheKey, Is.EqualTo("PolicyResult_BlogController_Post_" + NameHelper.Policy<BlogEditorPolicy>()));
		}

		[Test]
		public void Should_be_PolicyResult_BlogController_Post_BlogEditorPolicy_when_custom_cache_key_is_empty()
		{
			// Arrange
			var policy = new BlogEditorPolicy("");
			var manifest = new PolicyResultCacheManifest("BlogController", "Post", policy.GetType(), Cache.DoNotCache);
			var context = TestDataFactory.CreateSecurityContext(true);

			// Act
			var cacheKey = PolicyResultCacheKeyBuilder.CreateFromManifest(manifest, policy, context);

			// Assert
			Assert.That(cacheKey, Is.EqualTo("PolicyResult_BlogController_Post_" + NameHelper.Policy<BlogEditorPolicy>()));
		}

		[Test]
		public void Should_be_PolicyResult_BlogController_Post_BlogEditorPolicy_when_custom_cache_key_is_null()
		{
			// Arrange
			var policy = new BlogEditorPolicy(null);
			var manifest = new PolicyResultCacheManifest("BlogController", "Post", policy.GetType(), Cache.DoNotCache);
			var context = TestDataFactory.CreateSecurityContext(true);

			// Act
			var cacheKey = PolicyResultCacheKeyBuilder.CreateFromManifest(manifest, policy, context);

			// Assert
			Assert.That(cacheKey, Is.EqualTo("PolicyResult_BlogController_Post_" + NameHelper.Policy<BlogEditorPolicy>()));
		}

		public class BlogEditorPolicy : ISecurityPolicy, ICacheKeyProvider
		{
			private readonly string _customCacheKey;

			public BlogEditorPolicy(string customCacheKey)
			{
				_customCacheKey = customCacheKey;
			}

			public PolicyResult Enforce(ISecurityContext context)
			{
				throw new System.NotImplementedException();
			}

			public string Get(ISecurityContext securityContext)
			{
				return _customCacheKey;
			}
		}
	}
}