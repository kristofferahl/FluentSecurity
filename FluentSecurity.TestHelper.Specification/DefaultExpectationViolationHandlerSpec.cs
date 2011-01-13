using NUnit.Framework;

namespace FluentSecurity.TestHelper.Specification
{
	[TestFixture]
	[Category("DefaultExpectationViolationHandlerSpec")]
	public class When_handling_expectations_violations_with_the_default_handler
	{
		[Test]
		public void Should_return_failure_result_with_message()
		{
			// Arrange
			const string expectedMessage = "The message";
			var handler = new DefaultExpectationViolationHandler();

			// Act
			var expectationResult = handler.Handle(expectedMessage);

			// Assert
			Assert.That(expectationResult.ExpectationsMet, Is.False);
			Assert.That(expectationResult.Message, Is.EqualTo(expectedMessage));
		}
	}
}