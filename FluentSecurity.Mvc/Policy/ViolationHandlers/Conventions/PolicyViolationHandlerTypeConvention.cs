using System;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public abstract class PolicyViolationHandlerTypeConvention : IPolicyViolationHandlerConvention
	{
		public Func<Type, object> PolicyViolationHandlerProvider = t => ServiceLocation.ServiceLocator.Current.Resolve(t);

		public abstract Type GetHandlerTypeFor(PolicyViolationException exception);

		public IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception)
		{
			var type = GetHandlerTypeFor(exception);
			if (type != null)
			{
				if (typeof(IPolicyViolationHandler).IsAssignableFrom(type))
					return PolicyViolationHandlerProvider.Invoke(type) as IPolicyViolationHandler;
			}
			return null;
		}
	}
}