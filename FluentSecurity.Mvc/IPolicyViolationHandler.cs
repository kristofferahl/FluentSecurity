using System.Web.Mvc;

namespace FluentSecurity
{
	public interface IPolicyViolationHandler : ISecurityPolicyViolationHandler<ActionResult> {}
}