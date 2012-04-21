using System;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public abstract class PolicyViolationHandlerTypeConvention : IPolicyViolationHandlerConvention
	{
		public abstract Type GetHandlerTypeFor(PolicyViolationException exception);

		public IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception)
		{
			var type = GetHandlerTypeFor(exception);
			if (type != null)
			{
				if (typeof(IPolicyViolationHandler).IsAssignableFrom(type))
					return ServiceLocation.ServiceLocator.Current.Resolve(type) as IPolicyViolationHandler;
			}
			return null;
		}
	}
}