using System.Linq;
using FluentSecurity.Policy;
using FluentSecurity.TestHelper;
using FluentSecurity.SampleApplication.Controllers;
using FluentSecurity.SampleApplication.Models;
using NUnit.Framework;

namespace FluentSecurity.SampleApplication.Tests.Style3
{
	[TestFixture]
	[Category("SecurityConfigurationTests")]
	public class When_security_is_configured
	{
		[Test]
		public void Should_be_configured_correctly()
		{
            // Arrange
            var _configuration = Bootstrapper.SetupFluentSecurity();
            

			// Act
            var results = _configuration.Verify(expectations =>
			{
				expectations.Expect<HomeController>().Has<IgnorePolicy>();

				expectations.Expect<AccountController>().Has<DenyAuthenticatedAccessPolicy>();
				expectations.Expect<AccountController>(x => x.LogOut()).Has<DenyAnonymousAccessPolicy>().DoesNotHave<DenyAuthenticatedAccessPolicy>();

				expectations.Expect<ExampleController>(x => x.DenyAnonymousAccess()).Has<DenyAnonymousAccessPolicy>();
				expectations.Expect<ExampleController>(x => x.DenyAuthenticatedAccess()).Has<DenyAuthenticatedAccessPolicy>();
				expectations.Expect<ExampleController>(x => x.RequireAdministratorRole()).Has<RequireRolePolicy>(p =>
					p.RolesRequired.Contains(UserRole.Administrator) &&
					p.RolesRequired.Count() == 1
					);
				expectations.Expect<ExampleController>(x => x.RequirePublisherRole()).Has<RequireRolePolicy>(p =>
					p.RolesRequired.Contains(UserRole.Publisher) &&
					p.RolesRequired.Count() == 1
					);

				expectations.Expect<AdminController>().Has<AdministratorPolicy>();
				expectations.Expect<AdminController>(x => x.Delete()).Has<DelegatePolicy>(p => p.Name == "LocalOnlyPolicy");

				expectations.Expect<Areas.ExampleArea.Controllers.HomeController>(x => x.Index()).Has<DenyAnonymousAccessPolicy>();
				expectations.Expect<Areas.ExampleArea.Controllers.HomeController>(x => x.AdministratorsOnly()).Has(new RequireRolePolicy(UserRole.Administrator));
				expectations.Expect<Areas.ExampleArea.Controllers.HomeController>(x => x.PublishersOnly()).Has(new RequireRolePolicy(UserRole.Publisher));
			});

			// Assert
			Assert.That(results.Valid(), results.ErrorMessages());
		}
	}

	[TestFixture]
	[Category("SecurityConfigurationTests")]
	public class When_security_is_configured_for_account_controller
	{
		[Test]
		public void Should_be_configured_correctly()
		{
            // Arrange
            var _configuration = Bootstrapper.SetupFluentSecurity();

			// Act
            var results = _configuration.Verify<AccountController>(expectations =>
			{
				expectations.Expect(x => x.LogInAsAdministrator()).Has<DenyAuthenticatedAccessPolicy>();
				expectations.Expect(x => x.LogInAsPublisher()).Has<DenyAuthenticatedAccessPolicy>();
				expectations.Expect(x => x.LogOut()).Has<DenyAnonymousAccessPolicy>();
			});

			// Assert
			Assert.That(results.Valid(), results.ErrorMessages());
		}
	}
}