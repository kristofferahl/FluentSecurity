using FluentSecurity.Policy;

namespace FluentSecurity.TestHelper.Specification.TestData
{
	public class DenyInternetExplorerPolicy : ISecurityPolicy
	{
		public PolicyResult Enforce(ISecurityContext context)
		{
			return PolicyResult.CreateFailureResult(this, "Access to this section is restricted for Internet Explorer. Please switch to another browser!");
		}
	}
}