using System.Linq;
using FluentSecurity.Policy;
using FluentSecurity.SampleApplication.Controllers;
using FluentSecurity.SampleApplication.Models;
using FluentSecurity.TestHelper;
using NUnit.Framework;

namespace FluentSecurity.SampleApplication.Tests.Style4
{
	[TestFixture]
	[Category("SecurityConfigurationTests")]
	public class When_security_is_configured
	{
		[Test]
		public void Should_be_configured_correctly()
		{
			var expectations = new PolicyExpectations();

			expectations.For<HomeController>().Has<IgnorePolicy>();

			expectations.For<AccountController>().Has<DenyAuthenticatedAccessPolicy>();
			expectations.For<AccountController>(x => x.LogOut()).Has<DenyAnonymousAccessPolicy>().DoesNotHave<DenyAuthenticatedAccessPolicy>();

			expectations.For<ExampleController>(x => x.DenyAnonymousAccess()).Has<DenyAnonymousAccessPolicy>();
			expectations.For<ExampleController>(x => x.DenyAuthenticatedAccess()).Has<DenyAuthenticatedAccessPolicy>();
			expectations.For<ExampleController>(x => x.RequireAdministratorRole()).Has<RequireRolePolicy>(p =>
				p.RolesRequired.Contains(UserRole.Administrator) &&
				p.RolesRequired.Count() == 1
				);
			expectations.For<ExampleController>(x => x.RequirePublisherRole()).Has<RequireRolePolicy>(p =>
				p.RolesRequired.Contains(UserRole.Publisher) &&
				p.RolesRequired.Count() == 1
				);

			expectations.For<AdminController>().Has<AdministratorPolicy>();
			expectations.For<AdminController>(x => x.Delete()).Has<DelegatePolicy>(p => p.Name == "LocalOnlyPolicy");

			expectations.For<Areas.ExampleArea.Controllers.HomeController>(x => x.Index()).Has<DenyAnonymousAccessPolicy>();
			expectations.For<Areas.ExampleArea.Controllers.HomeController>(x => x.AdministratorsOnly()).Has(new RequireRolePolicy(UserRole.Administrator));
			expectations.For<Areas.ExampleArea.Controllers.HomeController>(x => x.PublishersOnly()).Has(new RequireRolePolicy(UserRole.Publisher));

			var results = expectations.VerifyAll(Bootstrapper.SetupFluentSecurity());

			Assert.That(results.Valid(), results.ErrorMessages());
		}
	}
}