using FluentSecurity.Policy;

namespace FluentSecurity.TestHelper.Specification.TestData
{
	public class DenyInternetExplorerPolicy : ISecurityPolicy
	{
		public void Enforce(bool isAuthenticated, object[] roles)
		{
			const bool isInternetExplorer = true;
			if (isInternetExplorer)
			{
				throw new FluentSecurityException<DenyInternetExplorerPolicy>("Access to this section is restricted for Internet Explorer. Please switch to another browser!");
			}
		}

		public object[] RolesRequired
		{
			get { return null; }
		}
	}
}