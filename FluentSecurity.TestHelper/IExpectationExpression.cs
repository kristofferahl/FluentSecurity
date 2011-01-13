using System;
using System.Collections.Generic;
using FluentSecurity.TestHelper.Expectations;

namespace FluentSecurity.TestHelper
{
	public interface IExpectationExpression
	{
		Type Controller { get; }
		string Action { get; }
		IEnumerable<IExpectation> Expectations { get; }
		IExpectationExpression Add(IExpectation expectation);
	}
}