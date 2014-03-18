using System;
using FluentSecurity.Configuration;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public abstract class PolicyViolationHandlerTypeConvention : IPolicyViolationHandlerConvention
	{
		public Func<Type, object> PolicyViolationHandlerProvider = t => SecurityConfiguration.Get<MvcConfiguration>().ServiceLocator.Resolve(t);

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