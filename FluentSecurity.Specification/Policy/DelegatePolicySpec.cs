using System;
using System.Web.Mvc;
using FluentSecurity.Policy;
using FluentSecurity.Policy.Contexts;
using FluentSecurity.Policy.Results;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy
{
	[TestFixture]
	[Category("DelegatePolicySpec")]
	public abstract class DelegatePolicyTestBase
	{
		protected const string ValidPolicyName = "DelegatePolicyName";
		protected Func<DelegateSecurityContext, PolicyResult> ValidPolicyDelegate = c => PolicyResult.CreateSuccessResult(c.Policy);
		protected Func<PolicyViolationException, ActionResult> ValidViolationHandlerDelegate = e => new EmptyResult();
	}

	public class When_passing_null_as_name_to_the_constructor_of_DelegatePolicy : DelegatePolicyTestBase
	{
		[Test]
		public void Should_throw_ArgumentException()
		{
			// Assert
			Assert.Throws<ArgumentException>(() => new DelegatePolicy(null, ValidPolicyDelegate, ValidViolationHandlerDelegate));
		}
	}

	public class When_passing_empty_string_as_name_to_the_constructor_of_DelegatePolicy : DelegatePolicyTestBase
	{
		[Test]
		public void Should_throw_ArgumentException()
		{
			// Assert
			Assert.Throws<ArgumentException>(() => new DelegatePolicy("", ValidPolicyDelegate, ValidViolationHandlerDelegate));
		}
	}

	public class When_getting_the_name_of_a_DelegatePolicy : DelegatePolicyTestBase
	{
		[Test]
		public void Should_return_expected_name()
		{
			// Arrange
			const string expectedName = "DelegatePolicyName";
			var policy = new DelegatePolicy(expectedName, ValidPolicyDelegate, ValidViolationHandlerDelegate);

			// Act
			var name = policy.Name;

			// Assert
			Assert.That(name, Is.EqualTo(expectedName));
		}
	}

	public class When_passing_null_as_policy_delegate_to_the_constructor_of_DelegatePolicy : DelegatePolicyTestBase
	{
		[Test]
		public void Should_throw_ArgumentNullException()
		{
			// Assert
			Assert.Throws<ArgumentNullException>(() => new DelegatePolicy(ValidPolicyName, null, ValidViolationHandlerDelegate));
		}
	}

	public class When_setting_the_policy_delegate_of_a_DelegatePolicy : DelegatePolicyTestBase
	{
		[Test]
		public void Should_have_expected_delegate()
		{
			// Arrange
			var expectedDelegate = ValidPolicyDelegate;

			// Act
			var policy = new DelegatePolicy(ValidPolicyName, expectedDelegate, ValidViolationHandlerDelegate);

			// Assert
			Assert.That(policy.Policy, Is.EqualTo(expectedDelegate));
		}
	}

	public class When_passing_null_as_violation_handler_delegate_to_the_constructor_of_DelegatePolicy : DelegatePolicyTestBase
	{
		[Test]
		public void Should_not_have_delegate_set_for_violation_handler()
		{
			// Act
			var policy = new DelegatePolicy(ValidPolicyName, ValidPolicyDelegate, null);

			// Assert
			Assert.That(policy.ViolationHandler, Is.Null);
		}
	}

	public class When_setting_the_violation_handler_delegate_of_a_DelegatePolicy : DelegatePolicyTestBase
	{
		[Test]
		public void Should_have_expected_delegate()
		{
			// Arrange
			var expectedDelegate = ValidViolationHandlerDelegate;

			// Act
			var policy = new DelegatePolicy(ValidPolicyName, ValidPolicyDelegate, expectedDelegate);

			// Assert
			Assert.That(policy.ViolationHandler, Is.EqualTo(expectedDelegate));
		}
	}

	public class When_enforcing_security_for_a_DelegatePolicy : DelegatePolicyTestBase
	{
		[Test]
		public void Should_not_be_successful_when_delegate_returns_failure()
		{
			// Arrange
			Func<DelegateSecurityContext, PolicyResult> failureDelegate = c => PolicyResult.CreateFailureResult(c.Policy, "Access denied");
			var policy = new DelegatePolicy(ValidPolicyName, failureDelegate, ValidViolationHandlerDelegate);
			var context = TestDataFactory.CreateSecurityContext(true);

			// Act
			var result = policy.Enforce(context);

			// Assert
			Assert.That(result, Is.TypeOf<DelegatePolicyResult>());
			Assert.That(result.ViolationOccured, Is.True);
			Assert.That(result.Message, Is.EqualTo("Access denied"));
		}

		[Test]
		public void Should_be_successful_when_delegate_returns_success()
		{
			// Arrange
			Func<DelegateSecurityContext, PolicyResult> successDelegate = c => PolicyResult.CreateSuccessResult(c.Policy);
			var policy = new DelegatePolicy(ValidPolicyName, successDelegate, ValidViolationHandlerDelegate);
			var context = TestDataFactory.CreateSecurityContext(true);

			// Act
			var result = policy.Enforce(context);

			// Assert
			Assert.That(result, Is.TypeOf<DelegatePolicyResult>());
			Assert.That(result.ViolationOccured, Is.False);
		}

		[Test]
		public void Should_pass_wrapped_security_context_to_delegate()
		{
			// Arrange
			DelegateSecurityContext delegateContext = null;
			Func<DelegateSecurityContext, PolicyResult> successDelegate = c =>
			{
				delegateContext = c;
			    return PolicyResult.CreateSuccessResult(c.Policy);
			};

			var policy = new DelegatePolicy(ValidPolicyName, successDelegate, ValidViolationHandlerDelegate);
			var context = TestDataFactory.CreateSecurityContext(true);

			// Act
			policy.Enforce(context);

			// Assert
			Assert.That(typeof(ISecurityContext).IsAssignableFrom(delegateContext.GetType()), Is.True);
			Assert.That(delegateContext.CurrentUserIsAuthenticated(), Is.EqualTo(context.CurrentUserIsAuthenticated()));
			Assert.That(delegateContext.CurrentUserRoles(), Is.EqualTo(context.CurrentUserRoles()));
		}
	}
}