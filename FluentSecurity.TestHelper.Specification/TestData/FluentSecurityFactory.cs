using System.Collections.Generic;
using FluentSecurity.Policy;

namespace FluentSecurity.TestHelper.Specification.TestData
{
	public static class FluentSecurityFactory
	{
		public static ISecurityConfiguration CreateSecurityConfiguration()
		{
			SecurityConfigurator.Configure(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(() => false);
				configuration.Advanced.IgnoreMissingConfiguration();

				configuration.For<SampleController>(x => x.Index()).DenyAuthenticatedAccess();
				configuration.For<SampleController>(x => x.List()).DenyAnonymousAccess();
				configuration.For<SampleController>(x => x.New()).RequireRole("Editor").AddPolicy(new DenyInternetExplorerPolicy());

				configuration.For<AdminController>().DenyAnonymousAccess();
				configuration.For<AdminController>(x => x.Login()).DenyAuthenticatedAccess();
				configuration.For<AdminController>(x => x.NewUser()).RequireRole(UserRole.UserEditor);

				configuration.For<IgnoreController>().Ignore();
			});

			return SecurityConfiguration.Current;
		}

		public static ISecurityConfiguration CreateEmptySecurityConfiguration()
		{
			SecurityConfigurator.Configure(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(() => false);
				configuration.Advanced.IgnoreMissingConfiguration();
			});

			return SecurityConfiguration.Current;
		}

		public static ISecurityConfiguration CreateSecurityConfigurationWithTwoExpectations()
		{
			SecurityConfigurator.Configure(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(() => false);
				configuration.Advanced.IgnoreMissingConfiguration();

				configuration.For<SampleController>(x => x.Index());
				configuration.For<SampleController>(x => x.List());
				configuration.For<SampleController>(x => x.New()).RequireRole("Writer");

				configuration.For<AdminController>();
				configuration.For<AdminController>(x => x.Login());

				configuration.For<IgnoreController>();

				configuration.For<TestController>().Ignore();
			});

			return SecurityConfiguration.Current;
		}

		public static PolicyExpectations CreatePolicyExpectations()
		{
			var policyExpectations = new PolicyExpectations();
			
			policyExpectations.For<SampleController>(x => x.Index()).Has<DenyAuthenticatedAccessPolicy>();
			policyExpectations.For<SampleController>(x => x.List()).Has<DenyAnonymousAccessPolicy>();
			policyExpectations.For<SampleController>(x => x.New()).Has(new RequireRolePolicy("Editor")).DoesNotHave(new RequireRolePolicy("Writer")).Has<DenyInternetExplorerPolicy>();

			policyExpectations.For<AdminController>().Has<DenyAnonymousAccessPolicy>();
			policyExpectations.For<AdminController>(x => x.Login())
				.DoesNotHave<DenyAnonymousAccessPolicy>()
				.Has<DenyAuthenticatedAccessPolicy>();
			policyExpectations.For<AdminController>(x => x.NewUser())
				.DoesNotHave<DenyAnonymousAccessPolicy>()
				.Has<RequireRolePolicy>();

			policyExpectations.For<IgnoreController>().Has<IgnorePolicy>();

			policyExpectations.For<TestController>().DoesNotHave<IgnorePolicy>();

			return policyExpectations;
		}

		public static IEnumerable<ExpectationGroup> CreateExpectationsGroups()
		{
			return CreatePolicyExpectations().ExpectationGroups;
		}
	}
}