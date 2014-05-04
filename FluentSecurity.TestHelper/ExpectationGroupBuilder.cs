using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Core;

namespace FluentSecurity.TestHelper
{
	public class ExpectationGroupBuilder : IExpectationGroupBuilder
	{
		private readonly IActionResolver _actionResolver;

		public ExpectationGroupBuilder(IActionResolver actionResolver)
		{
			_actionResolver = actionResolver;
		}

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
						expectationExpression.Expectations.ToList().ForEach(expectationGroup.ApplyExpectation);
				}
				else
				{
					var expectationGroups = CreateExpectationGroupsFor(expectationExpression);
					groups.AddRange(expectationGroups);
				}
			}

			return groups;
		}

		private IEnumerable<ExpectationGroup> CreateExpectationGroupsFor(ExpectationExpression expectationExpression)
		{
			var groups = new List<ExpectationGroup>();
			if (expectationExpression.Action == null)
			{
				var actionMethods = _actionResolver.ActionMethods(expectationExpression.Controller);
				foreach (var expectationGroup in actionMethods.Select(actionMethod => new ExpectationGroup(expectationExpression.Controller, actionMethod)))
				{
					expectationExpression.Expectations.ToList().ForEach(expectationGroup.ApplyExpectation);
					groups.Add(expectationGroup);
				}
			}
			else
			{
				var expectationGroup = new ExpectationGroup(expectationExpression.Controller, expectationExpression.Action);
				expectationExpression.Expectations.ToList().ForEach(expectationGroup.ApplyExpectation);
				groups.Add(expectationGroup);
			}

			return groups;
		}

		private static IEnumerable<ExpectationGroup> GetMatchingExpectationGroups(IEnumerable<ExpectationGroup> expectations, ExpectationExpression expectationExpression)
		{
			if (expectationExpression.Action == null)
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