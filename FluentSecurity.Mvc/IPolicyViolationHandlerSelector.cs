namespace FluentSecurity
{
	public interface IPolicyViolationHandlerSelector
	{
		IPolicyViolationHandler FindHandlerFor(PolicyViolationException exception);
	}
}