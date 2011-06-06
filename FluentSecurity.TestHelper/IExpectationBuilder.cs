using System.Collections.Generic;

namespace FluentSecurity.TestHelper
{
	public interface IExpectationGroupBuilder
	{
		IEnumerable<ExpectationGroup> Build(IEnumerable<ExpectationExpression> expectationsExpressions);
	}
}