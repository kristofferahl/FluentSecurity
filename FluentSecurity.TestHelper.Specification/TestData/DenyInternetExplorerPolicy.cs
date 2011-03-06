using FluentSecurity.Policy;

namespace FluentSecurity.TestHelper.Specification.TestData
{
	public class DenyInternetExplorerPolicy : ISecurityPolicy
	{
		public void Enforce(ISecurityContext context)
		{
			const bool isInternetExplorer = true;
			if (isInternetExplorer)
			{
				throw new PolicyViolationException<DenyInternetExplorerPolicy>("Access to this section is restricted for Internet Explorer. Please switch to another browser!");
			}
		}
	}
}