namespace FluentSecurity.WebApi.Policy.ViolationHandlers
{
	public class ExceptionPolicyViolationHandler : IWebApiPolicyViolationHandler
	{
		public object Handle(PolicyViolationException exception)
		{
			throw exception;
		}
	}
}