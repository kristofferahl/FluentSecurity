using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentSecurity.TestHelper
{
	public static class ExpectationResultsExtensions
	{
		public static string ErrorMessages(this IEnumerable<ExpectationResult> expectationResults)
		{
			var errorMessages = new StringBuilder();

			foreach (var failure in expectationResults.Where(x => x.ExpectationsMet == false))
				errorMessages.AppendLine(failure.Message);

			return errorMessages.ToString().TrimEnd('\n').TrimEnd('\r');
		}

		public static bool Valid(this IEnumerable<ExpectationResult> expectationResults)
		{
			return expectationResults.All(x => x.ExpectationsMet);
		}
	}
}