using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("PolicyExecutionModeSpec")]
	public class When_checking_the_default_PolicyExecutionMode
	{
		[Test]
		public void Should_return_false_for_should_stop_on_first_violation()
		{
			// Act & assert
			Assert.False(PolicyExecutionMode.ShouldStopOnFirstViolation);
		}
	}

	[TestFixture]
	[Category("PolicyExecutionModeSpec")]
	public class When_checking_the_PolicyExecutionMode
	{
		[Test]
		public void Should_return_true_for_should_stop_on_first_violation()
		{
			// Act
			PolicyExecutionMode.StopOnFirstViolation(true);

			// Assert
			Assert.True(PolicyExecutionMode.ShouldStopOnFirstViolation);
		}

		[Test]
		public void Should_return_false_for_should_stop_on_first_violation()
		{
			// Act
			PolicyExecutionMode.StopOnFirstViolation(false);

			// Assert
			Assert.False(PolicyExecutionMode.ShouldStopOnFirstViolation);
		}
	}
}