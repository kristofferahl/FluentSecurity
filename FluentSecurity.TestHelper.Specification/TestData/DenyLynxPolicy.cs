using FluentSecurity.Policy;

namespace FluentSecurity.TestHelper.Specification.TestData
{
	public class DenyLynxPolicy : ISecurityPolicy
	{
		public PolicyResult Enforce(ISecurityContext context)
		{
			return PolicyResult.CreateFailureResult(this, "Access to this section is restricted for Lynx. Please switch to another browser!");
		}
	}
}