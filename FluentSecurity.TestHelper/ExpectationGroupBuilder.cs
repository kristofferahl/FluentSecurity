using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentSecurity.TestHelper
{
	public class ExpectationGroupBuilder : IExpectationGroupBuilder
	{
		public IEnumerable<ExpectationGroup> Build(IEnumerable<ExpectationExpression> expectationsExpressions)
		{
			if (expectationsExpressions == null) throw new ArgumentNullException("expectationsExpressions");
		
			var groups = new List<ExpectationGroup>();

			foreach (var expectationExpression in expectationsExpressions)
			{
				var matchingExpectationGroups = GetMatchingExpectationGroups(groups, expectationExpression);
				if (matchingExpectationGroups.Any())
				{
					foreach (var expectationGroup in matchingExpectationGroups)
						expectationExpression.Expectations.Each(expectationGroup.ApplyExpectation);
				}
				else
				{
					var expectationGroups = CreateExpectationGroupsFor(expectationExpression);
					groups.AddRange(expectationGroups);
				}
			}

			return groups;
		}

		private static IEnumerable<ExpectationGroup> CreateExpectationGroupsFor(ExpectationExpression expectationExpression)
		{
			var groups = new List<ExpectationGroup>();
			if (String.IsNullOrEmpty(expectationExpression.Action))
			{
				var actionMethods = expectationExpression.Controller.GetActionMethods();
				foreach (var expectationGroup in actionMethods.Select(actionMethod => new ExpectationGroup(expectationExpression.Controller, actionMethod.GetActionName())))
				{
					expectationExpression.Expectations.Each(expectationGroup.ApplyExpectation);
					groups.Add(expectationGroup);
				}
			}
			else
			{
				var expectationGroup = new ExpectationGroup(expectationExpression.Controller, expectationExpression.Action);
				expectationExpression.Expectations.Each(expectationGroup.ApplyExpectation);
				groups.Add(expectationGroup);
			}

			return groups;
		}

		private static IEnumerable<ExpectationGroup> GetMatchingExpectationGroups(IEnumerable<ExpectationGroup> expectations, ExpectationExpression expectationExpression)
		{
			if (String.IsNullOrEmpty(expectationExpression.Action))
			{
				return expectations.Where(expectation =>
					expectation.Controller == expectationExpression.Controller
				);
			}

			return expectations.Where(expectation =>
				expectation.Action == expectationExpression.Action &&
				expectation.Controller == expectationExpression.Controller
			);
		}
	}
}