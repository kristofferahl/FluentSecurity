namespace FluentSecurity.TestHelper
{
	public class ExpectationResult
	{
		private ExpectationResult(bool expectationsMet, string message)
		{
			ExpectationsMet = expectationsMet;
			Message = message;
		}

		public static ExpectationResult CreateSuccessResult()
		{
			return new ExpectationResult(true, "");
		}

		public static ExpectationResult CreateFailureResult(string message)
		{
			return new ExpectationResult(false, message);
		}

		public bool ExpectationsMet { get; private set; }
		public string Message { get; private set; }

		public override string ToString()
		{
			return ExpectationsMet ? "Success" : "Failure";
		}
	}
}