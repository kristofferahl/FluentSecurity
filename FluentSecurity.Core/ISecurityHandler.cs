namespace FluentSecurity
{
	public interface ISecurityHandler<out TResult>
	{
		TResult HandleSecurityFor(string controllerName, string actionName, ISecurityContext securityContext);
	}
}