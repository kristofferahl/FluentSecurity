using System.Web.Mvc;

namespace FluentSecurity
{
	public interface IPolicyViolationHandler
	{
		ActionResult Handle(PolicyViolationException exception);
	}
}