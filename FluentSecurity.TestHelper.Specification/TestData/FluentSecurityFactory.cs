using System.Collections.Generic;
using FluentSecurity.Configuration;
using FluentSecurity.Policy;

namespace FluentSecurity.TestHelper.Specification.TestData
{
	public static class FluentSecurityFactory
	{
		public static ISecurityConfiguration CreateSecurityConfiguration()
		{
			SecurityConfigurator.Configure<MvcConfiguration>(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(() => false);
				configuration.Advanced.IgnoreMissingConfiguration();

				configuration.For<SampleController>(x => x.Index()).DenyAuthenticatedAccess();
				configuration.For<SampleController>(x => x.List()).DenyAnonymousAccess();
				configuration.For<SampleController>(x => x.New()).RequireAnyRole("Editor").AddPolicy(new DenyInternetExplorerPolicy());
				configuration.For<SampleController>(x => x.VoidAction()).DenyAnonymousAccess();

				configuration.For<AdminController>().DenyAnonymousAccess();
				configuration.For<AdminController>(x => x.Login()).DenyAuthenticatedAccess();
				configuration.For<AdminController>(x => x.NewUser()).RequireAnyRole(UserRole.UserEditor);

				configuration.For<IgnoreController>().Ignore();
			});

			return SecurityConfiguration.Current;
		}

		public static ISecurityConfiguration CreateEmptySecurityConfiguration()
		{
			SecurityConfigurator.Configure<MvcConfiguration>(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(() => false);
				configuration.Advanced.IgnoreMissingConfiguration();
			});

			return SecurityConfiguration.Current;
		}

		public static ISecurityConfiguration CreateSecurityConfigurationWithTwoExpectations()
		{
			SecurityConfigurator.Configure<MvcConfiguration>(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(() => false);
				configuration.Advanced.IgnoreMissingConfiguration();

				configuration.For<SampleController>(x => x.Index());
				configuration.For<SampleController>(x => x.List());
				configuration.For<SampleController>(x => x.New()).RequireAnyRole("Writer");

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
			policyExpectations.For<SampleController>(x => x.New()).Has(new RequireAnyRolePolicy("Editor")).DoesNotHave(new RequireAnyRolePolicy("Writer")).Has<DenyInternetExplorerPolicy>();

			policyExpectations.For<AdminController>().Has<DenyAnonymousAccessPolicy>();
			policyExpectations.For<AdminController>(x => x.Login())
				.DoesNotHave<DenyAnonymousAccessPolicy>()
				.Has<DenyAuthenticatedAccessPolicy>();
			policyExpectations.For<AdminController>(x => x.NewUser())
				.DoesNotHave<DenyAnonymousAccessPolicy>()
				.Has<RequireAnyRolePolicy>();

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