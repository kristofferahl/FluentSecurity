using System.Web.Mvc;

namespace FluentSecurity
{
	public interface ISecurityHandler
	{
		ActionResult HandleSecurityFor(string areaName, string controllerName, string actionName);
	}
}