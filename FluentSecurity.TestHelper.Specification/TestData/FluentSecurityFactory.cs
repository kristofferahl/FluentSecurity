using System.Collections.Generic;

namespace FluentSecurity.TestHelper.Specification.TestData
{
	public static class FluentSecurityFactory
	{
		public static IEnumerable<IPolicyContainer> CreatePolicyContainers()
		{
			Configuration.Configure(policy =>
			{
				policy.GetAuthenticationStatusFrom(() => false);
				policy.IgnoreMissingConfiguration();
				policy.For<SampleController>(x => x.Index()).DenyAuthenticatedAccess();
				policy.For<SampleController>(x => x.List()).DenyAnonymousAccess();
				policy.For<SampleController>(x => x.New()).RequireRole("Editor").AddPolicy(new DenyInternetExplorerPolicy());

				policy.For<IgnoreController>().Ignore();
			});

			return Configuration.GetPolicyContainers();
		}
	}
}