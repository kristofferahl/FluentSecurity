using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Core;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public abstract class PolicyViolationHandlerFilterConvention : IPolicyViolationHandlerConvention, IServiceLocatorDependent
	{
		private IServiceLocator _serviceLocator;

		public abstract ISecurityPolicyViolationHandler GetHandlerFor<TViolationHandlerType>(PolicyViolationException exception, IEnumerable<TViolationHandlerType> policyViolationHandlers) where TViolationHandlerType : ISecurityPolicyViolationHandler;
		
		public object GetHandlerFor<TViolationHandlerType>(PolicyViolationException exception) where TViolationHandlerType : ISecurityPolicyViolationHandler
		{
			var policyViolationHandlers = _serviceLocator.ResolveAll<TViolationHandlerType>().ToList();
			return GetHandlerFor(exception, policyViolationHandlers);
		}

		public void Inject(IServiceLocator serviceLocator)
		{
			_serviceLocator = serviceLocator;
		}
	}
}