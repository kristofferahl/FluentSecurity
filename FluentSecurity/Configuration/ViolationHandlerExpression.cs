using System;
using FluentSecurity.Policy;
using FluentSecurity.Policy.ViolationHandlers.Conventions;

namespace FluentSecurity.Configuration
{
	public class ViolationHandlerExpression<TSecurityPolicy> where TSecurityPolicy : class, ISecurityPolicy
	{
		private readonly ViolationsExpression _violationsExpression;

		internal ViolationHandlerExpression(ViolationsExpression violationsExpression)
		{
			_violationsExpression = violationsExpression;
		}

		public void IsHandledBy<TPolicyViolationHandler>() where TPolicyViolationHandler : class, IPolicyViolationHandler
		{
			_violationsExpression.AddConvention(new PolicyTypeToPolicyViolationHandlerTypeConvention<TSecurityPolicy, TPolicyViolationHandler>());
		}

		public void IsHandledBy<TPolicyViolationHandler>(Func<TPolicyViolationHandler> policyViolationHandlerFactory) where TPolicyViolationHandler : class, IPolicyViolationHandler
		{
			_violationsExpression.AddConvention(new PolicyTypeToPolicyViolationHandlerInstanceConvention<TSecurityPolicy, TPolicyViolationHandler>(policyViolationHandlerFactory));
		}
	}
}