using System.Collections.Generic;
using NUnit.Framework;

namespace FluentSecurity.TestHelper.Specification
{
	[TestFixture]
	[Category("ExpectationResultsExtensionsSpec")]
	public class When_checking_the_errors_for_a_set_of_expectation_results
	{
		[Test]
		public void Should_not_have_any_errors()
		{
			// Arrange
			var results = new List<ExpectationResult>
			{
				ExpectationResult.CreateSuccessResult(),
				ExpectationResult.CreateSuccessResult(),
				ExpectationResult.CreateSuccessResult()
			};

			// Act
			var errorMessages = results.ErrorMessages();

			// Assert
			Assert.That(results.Valid(), Is.True);
			Assert.That(errorMessages, Is.EqualTo(""));
		}

		[Test]
		public void Should_have_errors()
		{
			// Arrange
			var results = new List<ExpectationResult>
			{
				ExpectationResult.CreateSuccessResult(),
				ExpectationResult.CreateFailureResult("Expected this but was that for X"),
				ExpectationResult.CreateFailureResult("Expected that but was this for Y"),
				ExpectationResult.CreateSuccessResult(),
				ExpectationResult.CreateFailureResult("Expected this but was that for Z")
			};

			// Act
			var errorMessages = results.ErrorMessages();

			// Assert
			Assert.That(results.Valid(), Is.False);
			Assert.That(errorMessages, Is.EqualTo("Expected this but was that for X\r\nExpected that but was this for Y\r\nExpected this but was that for Z"));
		}
	}
}