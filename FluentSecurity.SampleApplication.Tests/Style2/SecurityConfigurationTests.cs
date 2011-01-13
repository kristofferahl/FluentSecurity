using FluentSecurity.Policy;
using FluentSecurity.SampleApplication.Controllers;
using FluentSecurity.SampleApplication.Models;
using FluentSecurity.TestHelper;
using NUnit.Framework;

namespace FluentSecurity.SampleApplication.Tests.Style2
{
	[TestFixture]
	[Category("SecurityConfigurationTests")]
	public class When_security_is_configured_for_account_controller : SecurityExpectationHelperBase<AccountController>
	{
		protected override ISecurityConfiguration ConfigurationToTest()
		{
			return Bootstrapper.SetupFluentSecurity();
		}

		[Test]
		public void Should_be_configured_correctly()
		{
			var results = VerifyExpectations(expectations =>
			{
				expectations.Expect(x => x.LogInAsAdministrator()).Has<DenyAuthenticatedAccessPolicy>();
				expectations.Expect(x => x.LogInAsPublisher()).Has<DenyAuthenticatedAccessPolicy>();
				expectations.Expect(x => x.LogOut()).Has<DenyAnonymousAccessPolicy>();
			});

			Assert.That(results.Valid(), results.ErrorMessages());
		}
	}

	[TestFixture]
	[Category("SecurityConfigurationTests")]
	public class When_security_is_configured_for_example_controller : SecurityExpectationHelperBase<ExampleController>
	{
		protected override ISecurityConfiguration ConfigurationToTest()
		{
			return Bootstrapper.SetupFluentSecurity();
		}

		[Test]
		public void Should_be_configured_correctly()
		{
			var results = VerifyExpectations(expectations =>
			{
				expectations.Expect(x => x.DenyAnonymousAccess()).Has<DenyAnonymousAccessPolicy>();
				expectations.Expect(x => x.DenyAuthenticatedAccess()).Has<DenyAuthenticatedAccessPolicy>();

				expectations.Expect(x => x.RequireAdministratorRole()).Has(new RequireRolePolicy(UserRole.Administrator));
				expectations.Expect(x => x.RequirePublisherRole()).Has(new RequireRolePolicy(UserRole.Publisher));
			});

			Assert.That(results.Valid(), results.ErrorMessages());
		}
	}

	[TestFixture]
	[Category("SecurityConfigurationTests")]
	public class When_security_is_configured_for_admin_controller : SecurityExpectationHelperBase<AdminController>
	{
		protected override ISecurityConfiguration ConfigurationToTest()
		{
			return Bootstrapper.SetupFluentSecurity();
		}

		[Test]
		public void Should_be_configured_correctly()
		{
			var results = VerifyExpectations(expectations =>
			{
				expectations.Expect().Has<AdministratorPolicy>();
				expectations.Expect(x => x.Index()).Has<IgnorePolicy>().DoesNotHave<AdministratorPolicy>();
			});

			Assert.That(results.Valid(), results.ErrorMessages());
		}
	}
}