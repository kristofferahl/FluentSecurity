using System;
using System.Collections.Generic;
using FluentSecurity.Policy;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("DefaultPolicyAppenderSpec")]
	public class When_updating_policies
	{
		[Test]
		public void Should_throw_ArgumentNullException_when_policy_to_add_is_null()
		{
			// Arrange
			var policyAppender = new DefaultPolicyAppender();

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() =>
				policyAppender.UpdatePolicies(null, new List<ISecurityPolicy>())
			);
		}

		[Test]
		public void Should_throw_ArgumentNullException_when_policies_is_null()
		{
			// Arrange
			var policyAppender = new DefaultPolicyAppender();
			
			// Act & Assert
			Assert.Throws<ArgumentNullException>(() =>
				policyAppender.UpdatePolicies(new IgnorePolicy(), null)
			);
		}
	}

	[TestFixture]
	[Category("DefaultPolicyAppenderSpec")]
	public class When_updating_policies_with_an_IgnorePolicy : with_DefaultPolicyAppender
	{
		private DenyAnonymousAccessPolicy _denyAnonymousAccessPolicy;
		private IgnorePolicy _ignorePolicy;
		private List<ISecurityPolicy> _policies;

		[SetUp]
		public override void SetUp()
		{
			base.SetUp();

			// Arrange
			_denyAnonymousAccessPolicy = new DenyAnonymousAccessPolicy();
			_ignorePolicy = new IgnorePolicy();
			_policies = new List<ISecurityPolicy>
			{
				_denyAnonymousAccessPolicy
			};
		}

		[Test]
		public void Should_remove_all_existing_policies()
		{
			// Act
			PolicyAppender.UpdatePolicies(_ignorePolicy, _policies);

			// Assert
			Assert.That(_policies.Contains(_denyAnonymousAccessPolicy), Is.False);
			Assert.That(_policies.Count, Is.EqualTo(1));
		}

		[Test]
		public void Should_add_ignorepolicy()
		{
			// Act
			PolicyAppender.UpdatePolicies(_ignorePolicy, _policies);

			// Assert
			Assert.That(_policies.Contains(_ignorePolicy), Is.True);
		}
	}

	[TestFixture]
	[Category("DefaultPolicyAppenderSpec")]
	public class When_updating_policies_with_a_DenyAnonymousAccessPolicy : with_DefaultPolicyAppender
	{
		private DenyAuthenticatedAccessPolicy _denyAuthenticatedAccessPolicy;
		private DenyAnonymousAccessPolicy _denyAnonymousAccessPolicy;
		private List<ISecurityPolicy> _policies;

		[SetUp]
		public override void SetUp()
		{
			base.SetUp();

			// Arrange
			_denyAuthenticatedAccessPolicy = new DenyAuthenticatedAccessPolicy();
			_denyAnonymousAccessPolicy = new DenyAnonymousAccessPolicy();
			_policies = new List<ISecurityPolicy>
				{
					_denyAuthenticatedAccessPolicy
				};
		}

		[Test]
		public void Should_remove_all_existing_policies()
		{
			// Act
			PolicyAppender.UpdatePolicies(_denyAnonymousAccessPolicy, _policies);

			// Assert
			Assert.That(_policies.Contains(_denyAuthenticatedAccessPolicy), Is.False);
			Assert.That(_policies.Count, Is.EqualTo(1));
		}

		[Test]
		public void Should_add_DenyAnonymousAccessPolicy()
		{
			// Act
			PolicyAppender.UpdatePolicies(_denyAnonymousAccessPolicy, _policies);

			// Assert
			Assert.That(_policies.Contains(_denyAnonymousAccessPolicy), Is.True);
		}
	}

    [TestFixture]
	[Category("DefaultPolicyAppenderSpec")]
	public class When_updating_policies_with_a_DenyAuthenticatedAccessPolicy : with_DefaultPolicyAppender
	{
		private DenyAuthenticatedAccessPolicy _denyAuthenticatedAccessPolicy;
		private DenyAnonymousAccessPolicy _denyAnonymousAccessPolicy;
		private List<ISecurityPolicy> _policies;

		[SetUp]
		public override void SetUp()
		{
			base.SetUp();

			// Arrange
			_denyAuthenticatedAccessPolicy = new DenyAuthenticatedAccessPolicy();
			_denyAnonymousAccessPolicy = new DenyAnonymousAccessPolicy();
			_policies = new List<ISecurityPolicy>
				{
					_denyAnonymousAccessPolicy
				};
		}

		[Test]
		public void Should_remove_all_existing_policies()
		{
			// Act
			PolicyAppender.UpdatePolicies(_denyAuthenticatedAccessPolicy, _policies);

			// Assert
			Assert.That(_policies.Contains(_denyAnonymousAccessPolicy), Is.False);
			Assert.That(_policies.Count, Is.EqualTo(1));
		}

		[Test]
		public void Should_add_DenyAuthenticatedAccessPolicy()
		{
			// Act
			PolicyAppender.UpdatePolicies(_denyAuthenticatedAccessPolicy, _policies);

			// Assert
			Assert.That(_policies.Contains(_denyAuthenticatedAccessPolicy), Is.True);
		}
	}

	[TestFixture]
	[Category("DefaultPolicyAppenderSpec")]
	public class When_updating_policies_with_a_RequireRolePolicy : with_DefaultPolicyAppender
	{
		private RequireRolePolicy _requireRolePolicy;
		private DenyAnonymousAccessPolicy _denyAnonymousAccessPolicy;
		private List<ISecurityPolicy> _policies;

		[SetUp]
		public override void SetUp()
		{
			base.SetUp();

			// Arrange
			_requireRolePolicy = new RequireRolePolicy(new List<object> { "Administrator" }.ToArray());
			_denyAnonymousAccessPolicy = new DenyAnonymousAccessPolicy();
			_policies = new List<ISecurityPolicy>
				{
					_denyAnonymousAccessPolicy
				};
		}

		[Test]
		public void Should_remove_all_existing_policies()
		{
			// Act
			PolicyAppender.UpdatePolicies(_requireRolePolicy, _policies);

			// Assert
			Assert.That(_policies.Contains(_denyAnonymousAccessPolicy), Is.False);
			Assert.That(_policies.Count, Is.EqualTo(1));
		}

		[Test]
		public void Should_add_RequireRolePolicy()
		{
			// Act
			PolicyAppender.UpdatePolicies(_requireRolePolicy, _policies);

			// Assert
			Assert.That(_policies.Contains(_requireRolePolicy), Is.True);
		}
	}

	public abstract class with_DefaultPolicyAppender
	{
		protected IPolicyAppender PolicyAppender;

		[SetUp]
		public virtual void SetUp()
		{
			PolicyAppender = new DefaultPolicyAppender();
		}
	}
}