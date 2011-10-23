using System;
using System.Collections.Generic;
using FluentSecurity.Policy;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy
{
	[TestFixture]
	[Category("DelegatePolicySpec")]
	public abstract class DelegatePolicyTestBase
	{
		protected const string ValidPolicyName = "DelegatePolicyName";
		protected Func<DelegatePolicy.DelegateSecurityContext, PolicyResult> ValidDelegate = c => PolicyResult.CreateSuccessResult(c.Policy);
	}

	public class When_passing_null_as_name_to_the_constructor_of_DelegatePolicy : DelegatePolicyTestBase
	{
		[Test]
		public void Should_throw_ArgumentException()
		{
			// Assert
			Assert.Throws<ArgumentException>(() => new DelegatePolicy(null, ValidDelegate));
		}
	}

	public class When_passing_empty_string_as_name_to_the_constructor_of_DelegatePolicy : DelegatePolicyTestBase
	{
		[Test]
		public void Should_throw_ArgumentException()
		{
			// Assert
			Assert.Throws<ArgumentException>(() => new DelegatePolicy("", ValidDelegate));
		}
	}

	public class When_getting_the_name_of_a_DelegatePolicy : DelegatePolicyTestBase
	{
		[Test]
		public void Should_return_expected_name()
		{
			// Arrange
			const string expectedName = "DelegatePolicyName";
			var policy = new DelegatePolicy(expectedName, ValidDelegate);

			// Act
			var name = policy.Name;

			// Assert
			Assert.That(name, Is.EqualTo(expectedName));
		}
	}

	public class When_passing_null_as_delegate_to_the_constructor_of_DelegatePolicy : DelegatePolicyTestBase
	{
		[Test]
		public void Should_throw_ArgumentNullException()
		{
			// Assert
			Assert.Throws<ArgumentNullException>(() => new DelegatePolicy(ValidPolicyName, null));
		}
	}

	public class When_getting_the_delegate_of_a_DelegatePolicy : DelegatePolicyTestBase
	{
		[Test]
		public void Should_return_expected_delegate()
		{
			// Arrange
			var expectedDelegate = ValidDelegate;
			var policy = new DelegatePolicy(ValidPolicyName, expectedDelegate);

			// Act
			var policyDelegate = policy.PolicyDelegate;

			// Assert
			Assert.That(policyDelegate, Is.EqualTo(expectedDelegate));
		}
	}

	public class When_enforcing_security_for_a_DelegatePolicy : DelegatePolicyTestBase
	{
		[Test]
		public void Should_not_be_successful_when_delegate_returns_failure()
		{
			// Arrange
			Func<DelegatePolicy.DelegateSecurityContext, PolicyResult> failureDelegate = c => PolicyResult.CreateFailureResult(c.Policy, "Access denied");
			var policy = new DelegatePolicy(ValidPolicyName, failureDelegate);
			var context = TestDataFactory.CreateSecurityContext(true);

			// Act
			var result = policy.Enforce(context);

			// Assert
			Assert.That(result.ViolationOccured, Is.True);
			Assert.That(result.Message, Is.EqualTo("Access denied"));
		}

		[Test]
		public void Should_be_successful_when_delegate_returns_success()
		{
			// Arrange
			Func<DelegatePolicy.DelegateSecurityContext, PolicyResult> successDelegate = c => PolicyResult.CreateSuccessResult(c.Policy);
			var policy = new DelegatePolicy(ValidPolicyName, successDelegate);
			var context = TestDataFactory.CreateSecurityContext(true);

			// Act
			var result = policy.Enforce(context);

			// Assert
			Assert.That(result.ViolationOccured, Is.False);
		}

		[Test]
		public void Should_pass_wrapped_security_context_to_delegate()
		{
			// Arrange
			DelegatePolicy.DelegateSecurityContext delegateContext = null;
			Func<DelegatePolicy.DelegateSecurityContext, PolicyResult> successDelegate = c =>
			{
				delegateContext = c;
			    return PolicyResult.CreateSuccessResult(c.Policy);
			};
			
			var policy = new DelegatePolicy(ValidPolicyName, successDelegate);
			var context = TestDataFactory.CreateSecurityContext(true);

			// Act
			var result = policy.Enforce(context);

			// Assert
			Assert.That(typeof(ISecurityContext).IsAssignableFrom(delegateContext.GetType()), Is.True);
			Assert.That(delegateContext.CurrenUserAuthenticated(), Is.EqualTo(context.CurrenUserAuthenticated()));
			Assert.That(delegateContext.CurrenUserRoles(), Is.EqualTo(context.CurrenUserRoles()));
		}
	}
}