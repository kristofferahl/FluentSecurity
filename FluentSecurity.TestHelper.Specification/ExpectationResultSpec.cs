using NUnit.Framework;

namespace FluentSecurity.TestHelper.Specification
{
	[TestFixture]
	[Category("ExpectationResultSpec")]
	public class When_creating_a_ExpectationResult
	{
		[Test]
		public void Should_create_failure()
		{
			// Arrange
			const string expectedMessage = "Failure";

			// Act
			var result = ExpectationResult.CreateFailureResult(expectedMessage);

			// Assert
			Assert.That(result.ExpectationsMet, Is.False);
			Assert.That(result.Message, Is.EqualTo(expectedMessage));
			Assert.That(result.ToString(), Is.EqualTo("Failure"));
		}

		[Test]
		public void Should_create_success()
		{
			// Act
			var result = ExpectationResult.CreateSuccessResult();

			// Assert
			Assert.That(result.ExpectationsMet, Is.True);
			Assert.That(result.Message, Is.Empty);
			Assert.That(result.ToString(), Is.EqualTo("Success"));
		}
	}
}