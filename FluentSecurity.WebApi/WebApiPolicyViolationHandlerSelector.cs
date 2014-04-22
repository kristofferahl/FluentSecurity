using FluentSecurity.WebApi.Policy.ViolationHandlers;

namespace FluentSecurity.WebApi
{
	public class WebApiPolicyViolationHandlerSelector : IPolicyViolationHandlerSelector<object>
	{
		public ISecurityPolicyViolationHandler<object> FindHandlerFor(PolicyViolationException exception)
		{
			return new ExceptionPolicyViolationHandler();
		}
	}
}