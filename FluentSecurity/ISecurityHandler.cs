namespace FluentSecurity
{
	public interface ISecurityHandler
	{
		void HandleSecurityFor(string controllerName, string actionName);
	}
}