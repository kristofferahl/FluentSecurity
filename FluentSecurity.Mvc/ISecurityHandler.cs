using System.Web.Mvc;

namespace FluentSecurity
{
	public interface ISecurityHandler
	{
		ActionResult HandleSecurityFor(string controllerName, string actionName, ISecurityContext securityContext);
	}
}