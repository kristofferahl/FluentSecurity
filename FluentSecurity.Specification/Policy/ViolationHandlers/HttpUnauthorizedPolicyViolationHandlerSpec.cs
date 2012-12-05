using System;
using System.Net;
using System.Web.Mvc;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy.ViolationHandlers
{
	[TestFixture]
	[Category("HttpUnauthorizedPolicyViolationHandlerSpec")]
	public class When_handling_a_violation_with_HttpUnauthorizedPolicyViolationHandler
	{
		[Test]
		public void Should_throw_when_exception_is_null()
		{
			// Arrange
			PolicyViolationException exception = null;
			var handler = new HttpUnauthorizedPolicyViolationHandler();

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => handler.Handle(exception));
		}

		[Test]
		public void Should_return_HttpUnauthorizedResult()
		{
			// Arrange
			var policyResult = TestDataFactory.CreatePolicyResultFailure();
			var exception = TestDataFactory.CreatePolicyViolationException(policyResult);
			var handler = new HttpUnauthorizedPolicyViolationHandler();

			// Act
			var result = handler.Handle(exception).As<HttpUnauthorizedResult>();

			// Assert
			Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.Unauthorized));
			Assert.That(result.StatusDescription, Is.EqualTo(exception.Message));
		}
	}
}