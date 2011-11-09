using System;
using FluentSecurity.Policy;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("PolicyResultSpec")]
	public class When_creating_a_policy_result
	{
		[Test]
		public void Should_be_successful_and_have_no_message()
		{
			// Arrange
			var policy = new DenyAnonymousAccessPolicy();

			// Act
			var result = PolicyResult.CreateSuccessResult(policy);

			// Assert
			Assert.That(result.ViolationOccured, Is.False);
			Assert.That(result.Message, Is.Null);
			Assert.That(result.PolicyType, Is.EqualTo(policy.GetType()));
		}

		[Test]
		public void Should_be_unsuccessful_and_have_a_message()
		{
			// Arrange
			var message = "Failure";
			var policy = new DenyAnonymousAccessPolicy();

			// Act
			var result = PolicyResult.CreateFailureResult(policy, message);

			// Assert
			Assert.That(result.ViolationOccured, Is.True);
			Assert.That(result.Message, Is.EqualTo(message));
			Assert.That(result.PolicyType, Is.EqualTo(policy.GetType()));

		}

		[Test]
		public void Should_throw_when_null_is_passed()
		{
			var validPolicy = new IgnorePolicy();
			const string validMessage = "Some message";

			// Act & assert
			Assert.Throws<ArgumentNullException>(() => PolicyResult.CreateFailureResult(null, validMessage));
			Assert.Throws<ArgumentNullException>(() => PolicyResult.CreateFailureResult(validPolicy, null));
		}
	}

	[TestFixture]
	[Category("PolicyResultSpec")]
	public class When_deriving_from_policy_result
	{
		private static readonly IgnorePolicy AnyPolicy = new IgnorePolicy();

		public class DerivedSuccessPolicyResult : PolicyResult
		{
			public DerivedSuccessPolicyResult() : base("Success message", false, AnyPolicy) {}
		}

		public class DerivedFailurePolicyResult : PolicyResult
		{
			public DerivedFailurePolicyResult() : base("Failure message", true, AnyPolicy) {}
		}

		[Test]
		public void Should_be_successful_and_have_no_message()
		{
			// Act
			var result = new DerivedSuccessPolicyResult();

			// Assert
			Assert.That(result.ViolationOccured, Is.False);
			Assert.That(result.Message, Is.EqualTo("Success message"));
			Assert.That(result.PolicyType, Is.EqualTo(AnyPolicy.GetType()));
		}

		[Test]
		public void Should_be_unsuccessful_and_have_a_message()
		{
			// Act
			var result = new DerivedFailurePolicyResult();

			// Assert
			Assert.That(result.ViolationOccured, Is.True);
			Assert.That(result.Message, Is.EqualTo("Failure message"));
			Assert.That(result.PolicyType, Is.EqualTo(AnyPolicy.GetType()));
		}
	}
}