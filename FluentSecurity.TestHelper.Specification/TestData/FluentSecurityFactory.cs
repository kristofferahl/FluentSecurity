using System.Collections.Generic;

namespace FluentSecurity.TestHelper.Specification.TestData
{
	public static class FluentSecurityFactory
	{
		public static IEnumerable<IPolicyContainer> CreatePolicyContainers()
		{
			FluentSecurity.Configure(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(() => false);
				configuration.IgnoreMissingConfiguration();
				
				configuration.For<SampleController>(x => x.Index()).DenyAuthenticatedAccess();
				configuration.For<SampleController>(x => x.List()).DenyAnonymousAccess();
				configuration.For<SampleController>(x => x.New()).RequireRole("Editor").AddPolicy(new DenyInternetExplorerPolicy());

				configuration.For<IgnoreController>().Ignore();
			});

			return FluentSecurity.CurrentConfiguration.PolicyContainers;
		}
	}
}