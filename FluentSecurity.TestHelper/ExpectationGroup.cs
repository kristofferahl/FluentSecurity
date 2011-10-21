using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.TestHelper.Expectations;

namespace FluentSecurity.TestHelper
{
	public class ExpectationGroup
	{
		private readonly IList<IExpectation> _expectations;

		public ExpectationGroup(Type controller, string action)
		{
			Controller = controller;
			Action = action;

			_expectations = new List<IExpectation>();
		}

		public Type Controller { get; private set; }
		public string Action { get; private set; }

		public IEnumerable<IExpectation> Expectations
		{
			get { return _expectations; }
		}

		public void ApplyExpectation(IExpectation expectation)
		{
			if (expectation is HasTypeExpectation)
			{
				HandleHasTypeExpectation(expectation as HasTypeExpectation);
				return;
			}

			if (expectation is HasInstanceExpectation)
			{
				HandleHasInstanceExpectation(expectation as HasInstanceExpectation);
				return;
			}
			
			if (expectation is DoesNotHaveTypeExpectation)
			{
				HandleDoesNotHaveTypeExpectation(expectation as DoesNotHaveTypeExpectation);
				return;
			}

			if (expectation is DoesNotHaveInstanceExpectation)
			{
				HandleDoesNotHaveInstanceExpectation(expectation as DoesNotHaveInstanceExpectation);
				return;
			}

			// TODO: Open/Closed priciple!?
			// TODO: Refactor to Expectation applyer!? Use extension methods?
			
			throw new ArgumentOutOfRangeException("expectation");
		}

		private void HandleHasTypeExpectation(HasTypeExpectation expectation)
		{
			var doesNotHaveTypeExpectations = _expectations.OfType<DoesNotHaveTypeExpectation>().Where(x => x.Type == expectation.Type);
			for (var i = 0; i < doesNotHaveTypeExpectations.Count(); i++)
			{
				var doesNotHaveTypeExpectation = doesNotHaveTypeExpectations.ElementAt(i);
				_expectations.Remove(doesNotHaveTypeExpectation);
			}

			var hasTypeExpectations = _expectations.OfType<HasTypeExpectation>();
			if (hasTypeExpectations.Any(x => x.Type == expectation.Type)) return;

			_expectations.Add(expectation);
		}

		private void HandleHasInstanceExpectation(HasInstanceExpectation expectation)
		{
			var doesNotHaveInstanceExpectations = _expectations.OfType<DoesNotHaveInstanceExpectation>().Where(x => x.Instance.Equals(expectation.Instance));
			for (var i = 0; i < doesNotHaveInstanceExpectations.Count(); i++)
			{
				var doesNotHaveInstanceExpectation = doesNotHaveInstanceExpectations.ElementAt(i);
				_expectations.Remove(doesNotHaveInstanceExpectation);
			}

			var hasInstanceExpectations = _expectations.OfType<HasInstanceExpectation>();
			if (hasInstanceExpectations.Any(x => x.Instance.Equals(expectation.Instance))) return;

			_expectations.Add(expectation);
		}

		private void HandleDoesNotHaveTypeExpectation(DoesNotHaveTypeExpectation expectation)
		{
			var hasTypeExpectations = _expectations.OfType<HasTypeExpectation>().Where(x => x.Type == expectation.Type);
			for (var i = 0; i < hasTypeExpectations.Count(); i++)
			{
				var hasTypeExpectation = hasTypeExpectations.ElementAt(i);
				_expectations.Remove(hasTypeExpectation);
			}

			var doesNotHaveTypeExpectations = _expectations.OfType<DoesNotHaveTypeExpectation>();
			if (doesNotHaveTypeExpectations.Any(x => x.Type == expectation.Type)) return;

			_expectations.Add(expectation);
		}

		private void HandleDoesNotHaveInstanceExpectation(DoesNotHaveInstanceExpectation expectation)
		{
			var hasInstanceExpectations = _expectations.OfType<HasInstanceExpectation>().Where(x => x.Instance.Equals(expectation.Instance));
			for (var i = 0; i < hasInstanceExpectations.Count(); i++)
			{
				var hasInstanceExpectation = hasInstanceExpectations.ElementAt(i);
				_expectations.Remove(hasInstanceExpectation);
			}

			var doesNotHaveTypeExpectations = _expectations.OfType<DoesNotHaveInstanceExpectation>();
			if (doesNotHaveTypeExpectations.Any(x => x.Instance.Equals(expectation.Instance))) return;

			_expectations.Add(expectation);
		}
	}
}