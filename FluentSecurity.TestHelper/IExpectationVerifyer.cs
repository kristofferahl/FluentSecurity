using System.Collections.Generic;

namespace FluentSecurity.TestHelper
{
	public interface IExpectationVerifyer
	{
		IEnumerable<ExpectationResult> VerifyExpectationsOf(IEnumerable<ExpectationGroup> expectationGroups);
	}
}