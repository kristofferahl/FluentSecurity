using System;
using System.Collections.Generic;
using FluentSecurity.TestHelper.Expectations;

namespace FluentSecurity.TestHelper
{
	public class ExpectationVerifyer : IExpectationVerifyer
	{
		private readonly ISecurityConfiguration _configuration;
		private readonly IExpectationViolationHandler _expectationViolationHandler;

		public ExpectationVerifyer(ISecurityConfiguration configuration, IExpectationViolationHandler expectationViolationHandler)
		{
			if (configuration == null) throw new ArgumentNullException("configuration");
			if (expectationViolationHandler == null) throw new ArgumentNullException("expectationViolationHandler");

			_configuration = configuration;
			_expectationViolationHandler = expectationViolationHandler;
		}

		private List<ExpectationResult> ExpectationResults { get; set; }

		public IEnumerable<ExpectationResult> VerifyExpectationsOf(IEnumerable<ExpectationGroup> expectationGroups)
		{
			if (expectationGroups == null) throw new ArgumentNullException("expectationGroups");

			ExpectationResults = new List<ExpectationResult>();

			foreach (var expectationGroup in expectationGroups)
				VerifyExpectationsOf(expectationGroup);

			return ExpectationResults;
		}

		private void VerifyExpectationsOf(ExpectationGroup expectationGroup)
		{
			var controllerName = expectationGroup.Controller.GetControllerName();
			var actionName = expectationGroup.Action;
			
			var policyContainer = _configuration.Runtime.PolicyContainers.GetContainerFor(controllerName, actionName);

			// TODO: Apply open/closed principle. Extract each verifyer into it's own class.
			foreach (var expectation in expectationGroup.Expectations)
			{
				AppendResultOf(
					VerifyHasType(policyContainer, expectation as HasTypeExpectation, controllerName, actionName)
					);

				AppendResultOf(
					VerifyHasInstance(policyContainer, expectation as HasInstanceExpectation, controllerName, actionName)
					);

				AppendResultOf(
					VerifyDoesNotHaveType(policyContainer, expectation as DoesNotHaveTypeExpectation)
					);

				AppendResultOf(
					VerifyDoesNotHaveInstance(policyContainer, expectation as DoesNotHaveInstanceExpectation)
					);
			}
		}

		private ExpectationResult VerifyHasType(IPolicyContainer policyContainer, HasTypeExpectation expectation, string controllerName, string actionName)
		{
			if (expectation == null) return null;
			if (policyContainer == null)
			{
				const string messageFormat = "Expected a configuration for controller \"{0}\", action \"{1}\". Policycontainer could not be found!";
				var message = string.Format(messageFormat, controllerName, actionName);
				return _expectationViolationHandler.Handle(message);
			}

			if (policyContainer.HasPolicyMatching(expectation) == false)
			{
				const string messageFormat = "Expected policy of type \"{2}\" for controller \"{0}\", action \"{1}\".{3}";
				var message = string.Format(messageFormat, policyContainer.ControllerName, policyContainer.ActionName, expectation.Type, GetPredicateDescription(expectation));
				return _expectationViolationHandler.Handle(message);
			}
			return ExpectationResult.CreateSuccessResult();
		}
		
		private ExpectationResult VerifyHasInstance(IPolicyContainer policyContainer, HasInstanceExpectation expectation, string controllerName, string actionName)
		{
			if (expectation == null) return null;
			if (policyContainer == null)
			{
				const string messageFormat = "Expected a configuration for controller \"{0}\", action \"{1}\". Policycontainer could not be found!";
				var message = string.Format(messageFormat, controllerName, actionName);
				return _expectationViolationHandler.Handle(message);
			}

			if (policyContainer.HasPolicy(expectation.Instance) == false)
			{
				const string messageFormat = "The expected instance of the policy \"{2}\" was not found! Controller \"{0}\", action \"{1}\".";
				var expectedPolicyType = expectation.Instance.GetType().Name;
				var message = string.Format(messageFormat, policyContainer.ControllerName, policyContainer.ActionName, expectedPolicyType);
				return _expectationViolationHandler.Handle(message);
			}
			return ExpectationResult.CreateSuccessResult();
		}

		private ExpectationResult VerifyDoesNotHaveType(IPolicyContainer policyContainer, DoesNotHaveTypeExpectation expectation)
		{
			if (expectation == null) return null;
			if (policyContainer != null && policyContainer.HasPolicyMatching(expectation))
			{
				const string messageFormat = "An unexpected policy of type \"{2}\" was found for controller \"{0}\", action \"{1}\".{3}";
				var message = string.Format(messageFormat, policyContainer.ControllerName, policyContainer.ActionName, expectation.Type, GetPredicateDescription(expectation));
				return _expectationViolationHandler.Handle(message);
			}
			return ExpectationResult.CreateSuccessResult();
		}

		private ExpectationResult VerifyDoesNotHaveInstance(IPolicyContainer policyContainer, DoesNotHaveInstanceExpectation expectation)
		{
			if (expectation == null) return null;
			if (policyContainer != null && policyContainer.HasPolicy(expectation.Instance))
			{
				const string messageFormat = "An unexpected instance of the policy type \"{2}\" was found for controller \"{0}\", action \"{1}\".";
				var expectedPolicyType = expectation.Instance.GetType().Name;
				var message = string.Format(messageFormat, policyContainer.ControllerName, policyContainer.ActionName, expectedPolicyType);
				return _expectationViolationHandler.Handle(message);
			}
			return ExpectationResult.CreateSuccessResult();
		}

		private static string GetPredicateDescription(TypeExpectation expectation)
		{
			if (expectation.IsPredicateExpectation)
			{
				return String.Concat(
					Environment.NewLine,
					"\t\t",
					"Predicate: ",
					expectation.GetPredicateDescription()
					);
			}
			return String.Empty;
		}

		private void AppendResultOf(ExpectationResult expectationResult)
		{
			if (expectationResult != null) ExpectationResults.Add(expectationResult);
		}
	}
}