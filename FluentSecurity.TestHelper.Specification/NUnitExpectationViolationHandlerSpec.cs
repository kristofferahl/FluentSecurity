using NUnit.Framework;

namespace FluentSecurity.TestHelper.Specification
{
	[TestFixture]
	[Category("NUnitExpectationViolationHandlerSpec")]
	public class When_handling_violations_using_the_NUnitExpectationViolationHandler
	{
		[Test]
		public void Should_throw_assertion_exception()
		{
			// Arrange
			const string message = "Message";
			var expectationViolationHandler = new NUnitExpectationViolationHandler();

			// Act
			var exception = Assert.Throws<AssertionException>(() => expectationViolationHandler.Handle(message));

			// Assert
			Assert.That(exception.Message, Is.EqualTo(message));
		}
	}
}