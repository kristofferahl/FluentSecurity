using System;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public abstract class LazyTypePolicyViolationHandlerConvention<TPolicyViolationHandler> : PolicyViolationHandlerTypeConvention where TPolicyViolationHandler : IPolicyViolationHandler
	{
		public override Type GetHandlerTypeFor(PolicyViolationException exception)
		{
			return typeof (TPolicyViolationHandler);
		}
	}
}