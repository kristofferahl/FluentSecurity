using System;
using System.Collections.Generic;
using FluentSecurity.Policy;
using NUnit.Framework;

namespace FluentSecurity.TestHelper.Specification
{
	[TestFixture]
	[Category("PolicyExpectationsExtensionsTester")]
	public class When_calling_has_for_SampleController_Index
	{
		private PolicyExpectations _policyExpectations;

		[SetUp]
		public void SetUp()
		{
			// Arrange
			_policyExpectations = new PolicyExpectations(TestData.FluentSecurityFactory.CreatePolicyContainers());
			_policyExpectations.For("Sample", "Index");
		}

		[Test]
		public void Should_return_policy_expectations_for_DenyAuthenticatedAccessPolicy()
		{
			// Act
			var expectations = _policyExpectations.Has<DenyAuthenticatedAccessPolicy>();

			// Assert
			Assert.That(expectations, Is.EqualTo(_policyExpectations));
		}

		[Test]
		public void Should_throw_for_DenyAnonymousAccessPolicy()
		{
			// Act & Assert
			Assert.Throws<AssertionException>(() => _policyExpectations.Has<DenyAnonymousAccessPolicy>());
		}

		[Test]
		public void Should_throw_for_RequireRolePolicy()
		{
			// Act & Assert
			Assert.Throws<AssertionException>(() => _policyExpectations.Has<RequireRolePolicy>());
		}

		[Test]
		public void Should_throw_for_IgnorePolicy()
		{
			// Act & Assert
			Assert.Throws<AssertionException>(() => _policyExpectations.Has<IgnorePolicy>());
		}
	}

	[TestFixture]
	[Category("PolicyExpectationsExtensionsTester")]
	public class When_calling_does_not_have_for_SampleController_Index
	{
		private PolicyExpectations _policyExpectations;

		[SetUp]
		public void SetUp()
		{
			// Arrange
			_policyExpectations = new PolicyExpectations(TestData.FluentSecurityFactory.CreatePolicyContainers());
			_policyExpectations.For("Sample", "Index");
		}

		[Test]
		public void Should_throw_for_DenyAnonymousAccessPolicy()
		{
			// Act & Assert
			Assert.Throws<AssertionException>(() => _policyExpectations.DoesNotHave<DenyAuthenticatedAccessPolicy>());
		}

		[Test]
		public void Should_return_policy_expectations_for_DenyAnonymousAccessPolicy()
		{
			// Act
			var expectations = _policyExpectations.DoesNotHave<DenyAnonymousAccessPolicy>();

			// Assert
			Assert.That(expectations, Is.EqualTo(_policyExpectations));
		}

		[Test]
		public void Should_return_policy_expectations_for_RequireRolePolicy()
		{
			// Act
			var expectations = _policyExpectations.DoesNotHave<RequireRolePolicy>();

			// Assert
			Assert.That(expectations, Is.EqualTo(_policyExpectations));
		}

		[Test]
		public void Should_return_policy_expectations_for_IgnorePolicy()
		{
			// Act
			var expectations = _policyExpectations.DoesNotHave<IgnorePolicy>();

			// Assert
			Assert.That(expectations, Is.EqualTo(_policyExpectations));
		}
	}

	[TestFixture]
	[Category("PolicyExpectationsExtensionsTester")]
	public class When_verifying_expectations_for_controller_not_in_the_policycontainers_list
	{
		private PolicyExpectations _policyExpectations;

		private class SomePolicyNotImportantForThisTest : ISecurityPolicy
		{
			public void Enforce(bool isAuthenticated, object[] roles) { }

			public object[] RolesRequired
			{
				get { return null; }
			}
		}

		[SetUp]
		public void SetUp()
		{
			var policyContainers = new List<IPolicyContainer>();
			_policyExpectations = new PolicyExpectations(policyContainers);
			_policyExpectations.For("NonExistingController", "NonExistingAction");
		}

		[Test]
		public void Should_throw_when_calling_has()
		{
			// Act & Assert
			Assert.Throws<AssertionException>(() => _policyExpectations.Has<SomePolicyNotImportantForThisTest>());
		}

		[Test]
		public void Should_not_throw_when_calling_does_not_have()
		{
			// Act & Assert
			Assert.DoesNotThrow(() => _policyExpectations.DoesNotHave<SomePolicyNotImportantForThisTest>());
		}
	}
}