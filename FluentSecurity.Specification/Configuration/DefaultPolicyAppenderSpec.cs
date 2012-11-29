using System;
using System.Collections.Generic;
using FluentSecurity.Configuration;
using FluentSecurity.Policy;
using NUnit.Framework;

namespace FluentSecurity.Specification.Configuration
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

		protected override void Context()
		{
			// Arrange
			_denyAnonymousAccessPolicy = new DenyAnonymousAccessPolicy();
			_ignorePolicy = new IgnorePolicy();
			Policies = new List<ISecurityPolicy>
			{
				_denyAnonymousAccessPolicy
			};
		}

		[Test]
		public void Should_remove_all_existing_policies()
		{
			// Act
			PolicyAppender.UpdatePolicies(_ignorePolicy, Policies);

			// Assert
			Assert.That(Policies.Contains(_denyAnonymousAccessPolicy), Is.False);
			Assert.That(Policies.Count, Is.EqualTo(1));
		}

		[Test]
		public void Should_add_ignorepolicy()
		{
			// Act
			PolicyAppender.UpdatePolicies(_ignorePolicy, Policies);

			// Assert
			Assert.That(Policies.Contains(_ignorePolicy), Is.True);
		}
	}

	[TestFixture]
	[Category("DefaultPolicyAppenderSpec")]
	public class When_updating_policies_with_a_DenyAnonymousAccessPolicy : with_DefaultPolicyAppender
	{
		private DenyAuthenticatedAccessPolicy _denyAuthenticatedAccessPolicy;
		private DenyAnonymousAccessPolicy _denyAnonymousAccessPolicy;

		protected override void Context()
		{
			// Arrange
			_denyAuthenticatedAccessPolicy = new DenyAuthenticatedAccessPolicy();
			_denyAnonymousAccessPolicy = new DenyAnonymousAccessPolicy();
			Policies = new List<ISecurityPolicy>
			{
				_denyAuthenticatedAccessPolicy
			};
		}

		[Test]
		public void Should_remove_all_existing_policies()
		{
			// Act
			PolicyAppender.UpdatePolicies(_denyAnonymousAccessPolicy, Policies);

			// Assert
			Assert.That(Policies.Contains(_denyAuthenticatedAccessPolicy), Is.False);
			Assert.That(Policies.Count, Is.EqualTo(1));
		}

		[Test]
		public void Should_add_DenyAnonymousAccessPolicy()
		{
			// Act
			PolicyAppender.UpdatePolicies(_denyAnonymousAccessPolicy, Policies);

			// Assert
			Assert.That(Policies.Contains(_denyAnonymousAccessPolicy), Is.True);
		}
	}

    [TestFixture]
	[Category("DefaultPolicyAppenderSpec")]
	public class When_updating_policies_with_a_DenyAuthenticatedAccessPolicy : with_DefaultPolicyAppender
	{
		private DenyAuthenticatedAccessPolicy _denyAuthenticatedAccessPolicy;
		private DenyAnonymousAccessPolicy _denyAnonymousAccessPolicy;

		protected override void Context()
		{
			// Arrange
			_denyAuthenticatedAccessPolicy = new DenyAuthenticatedAccessPolicy();
			_denyAnonymousAccessPolicy = new DenyAnonymousAccessPolicy();
			Policies = new List<ISecurityPolicy>
			{
				_denyAnonymousAccessPolicy
			};
		}

		[Test]
		public void Should_remove_all_existing_policies()
		{
			// Act
			PolicyAppender.UpdatePolicies(_denyAuthenticatedAccessPolicy, Policies);

			// Assert
			Assert.That(Policies.Contains(_denyAnonymousAccessPolicy), Is.False);
			Assert.That(Policies.Count, Is.EqualTo(1));
		}

		[Test]
		public void Should_add_DenyAuthenticatedAccessPolicy()
		{
			// Act
			PolicyAppender.UpdatePolicies(_denyAuthenticatedAccessPolicy, Policies);

			// Assert
			Assert.That(Policies.Contains(_denyAuthenticatedAccessPolicy), Is.True);
		}
	}

	[TestFixture]
	[Category("DefaultPolicyAppenderSpec")]
	public class When_updating_policies_with_a_RequireRolePolicy : with_DefaultPolicyAppender
	{
		private RequireRolePolicy _requireRolePolicy;
		private DenyAnonymousAccessPolicy _denyAnonymousAccessPolicy;

		protected override void Context()
		{
			// Arrange
			_requireRolePolicy = new RequireRolePolicy("Administrator");
			_denyAnonymousAccessPolicy = new DenyAnonymousAccessPolicy();
			Policies = new List<ISecurityPolicy>
			{
				_denyAnonymousAccessPolicy
			};
		}

		[Test]
		public void Should_remove_all_existing_policies()
		{
			// Act
			PolicyAppender.UpdatePolicies(_requireRolePolicy, Policies);

			// Assert
			Assert.That(Policies.Contains(_denyAnonymousAccessPolicy), Is.False);
			Assert.That(Policies.Count, Is.EqualTo(1));
		}

		[Test]
		public void Should_add_RequireRolePolicy()
		{
			// Act
			PolicyAppender.UpdatePolicies(_requireRolePolicy, Policies);

			// Assert
			Assert.That(Policies.Contains(_requireRolePolicy), Is.True);
		}
	}

	[TestFixture]
	[Category("DefaultPolicyAppenderSpec")]
	public class When_updating_policies_with_a_RequireAnyRolePolicy : with_DefaultPolicyAppender
	{
		private RequireAnyRolePolicy _requireAnyRolePolicy;
		private DenyAnonymousAccessPolicy _denyAnonymousAccessPolicy;

		protected override void Context()
		{
			// Arrange
			_requireAnyRolePolicy = new RequireAnyRolePolicy("Administrator");
			_denyAnonymousAccessPolicy = new DenyAnonymousAccessPolicy();
			Policies = new List<ISecurityPolicy>
			{
				_denyAnonymousAccessPolicy
			};
		}

		[Test]
		public void Should_remove_all_existing_policies()
		{
			// Act
			PolicyAppender.UpdatePolicies(_requireAnyRolePolicy, Policies);

			// Assert
			Assert.That(Policies.Contains(_denyAnonymousAccessPolicy), Is.False);
			Assert.That(Policies.Count, Is.EqualTo(1));
		}

		[Test]
		public void Should_add_RequireAnyRolePolicy()
		{
			// Act
			PolicyAppender.UpdatePolicies(_requireAnyRolePolicy, Policies);

			// Assert
			Assert.That(Policies.Contains(_requireAnyRolePolicy), Is.True);
		}
	}

	[TestFixture]
	[Category("DefaultPolicyAppenderSpec")]
	public class When_updating_policies_with_a_RequireAllRolesPolicy : with_DefaultPolicyAppender
	{
		private RequireAllRolesPolicy _requireAllRolesPolicy;
		private DenyAnonymousAccessPolicy _denyAnonymousAccessPolicy;

		protected override void Context()
		{
			// Arrange
			_requireAllRolesPolicy = new RequireAllRolesPolicy("Administrator");
			_denyAnonymousAccessPolicy = new DenyAnonymousAccessPolicy();
			Policies = new List<ISecurityPolicy>
			{
				_denyAnonymousAccessPolicy
			};
		}

		[Test]
		public void Should_remove_all_existing_policies()
		{
			// Act
			PolicyAppender.UpdatePolicies(_requireAllRolesPolicy, Policies);

			// Assert
			Assert.That(Policies.Contains(_denyAnonymousAccessPolicy), Is.False);
			Assert.That(Policies.Count, Is.EqualTo(1));
		}

		[Test]
		public void Should_add_RequireAllRolesPolicy()
		{
			// Act
			PolicyAppender.UpdatePolicies(_requireAllRolesPolicy, Policies);

			// Assert
			Assert.That(Policies.Contains(_requireAllRolesPolicy), Is.True);
		}
	}

	public abstract class with_DefaultPolicyAppender
	{
		protected List<ISecurityPolicy> Policies;
		protected IPolicyAppender PolicyAppender;

		[SetUp]
		public virtual void SetUp()
		{
			Policies = new List<ISecurityPolicy>();
			PolicyAppender = new DefaultPolicyAppender();

			Context();
		}

		protected abstract void Context();
	}
}