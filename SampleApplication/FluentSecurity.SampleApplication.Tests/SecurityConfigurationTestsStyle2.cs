using System.Linq;
using FluentSecurity.Policy;
using FluentSecurity.SampleApplication.Controllers;
using FluentSecurity.SampleApplication.Models;
using FluentSecurity.TestHelper;
using NUnit.Framework;

namespace FluentSecurity.SampleApplication.Tests
{
	[TestFixture]
	[Category("SecurityConfigurationTests")]
	public class When_security_is_configured_using_style2
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
			expectations.For<ExampleController>(x => x.RequireAdministratorRole()).Has<RequireAnyRolePolicy>(p =>
				p.RolesRequired.Contains(UserRole.Administrator) &&
				p.RolesRequired.Count() == 1
				);
			expectations.For<ExampleController>(x => x.RequirePublisherRole()).Has<RequireAnyRolePolicy>(p =>
				p.RolesRequired.Contains(UserRole.Publisher) &&
				p.RolesRequired.Count() == 1
				);

			expectations.For<AdminController>().Has<AdministratorPolicy>();
			expectations.For<AdminController>(x => x.Delete()).Has<DelegatePolicy>(p => p.Name == "LocalOnlyPolicy");

			expectations.For<Areas.ExampleArea.Controllers.HomeController>(x => x.Index()).Has<DenyAnonymousAccessPolicy>();
			expectations.For<Areas.ExampleArea.Controllers.HomeController>(x => x.AdministratorsOnly()).Has(new RequireAnyRolePolicy(UserRole.Administrator));
			expectations.For<Areas.ExampleArea.Controllers.HomeController>(x => x.PublishersOnly()).Has(new RequireAnyRolePolicy(UserRole.Publisher));

			expectations.For<BlogPostController>().Has<DenyAnonymousAccessPolicy>();
			expectations.For<BlogPostController>(x => x.Index()).Has<IgnorePolicy>().DoesNotHave<DenyAnonymousAccessPolicy>();
			expectations.For<BlogPostController>(x => x.Details()).Has<IgnorePolicy>().DoesNotHave<DenyAnonymousAccessPolicy>();
			expectations.For<BlogPostController>(x => x.Delete()).Has(new RequireAnyRolePolicy(UserRole.Administrator)).DoesNotHave<DenyAnonymousAccessPolicy>();

			var results = expectations.VerifyAll(Bootstrapper.SetupFluentSecurity());

			Assert.That(results.Valid(), results.ErrorMessages());
		}
	}
}