using FluentSecurity.Policy;

namespace FluentSecurity.TestHelper.Specification.TestData
{
	public class DenyLynxPolicy : ISecurityPolicy
	{
		public void Enforce(ISecurityContext context)
		{
			const bool isLynx = true;
			if (isLynx)
			{
				throw new PolicyViolationException<DenyLynxPolicy>("Access to this section is restricted for Lynx. Please switch to another browser!");
			}
		}
	}
}