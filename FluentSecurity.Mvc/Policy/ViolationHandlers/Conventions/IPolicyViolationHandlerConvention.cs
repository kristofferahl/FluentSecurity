namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public interface IPolicyViolationHandlerConvention : IConvention
	{
		IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception);
	}
}