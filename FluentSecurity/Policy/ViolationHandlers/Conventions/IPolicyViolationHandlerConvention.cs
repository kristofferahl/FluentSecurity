namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public interface IPolicyViolationHandlerConvention
	{
		IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception);
	}
}