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
			var configuration = Bootstrapper.SetupFluentSecurity();

			// Act
			var results = configuration.Verify(expectations =>
			{
				expectations.Expect<AccountController>(x => x.LogInAsAdministrator()).Has<DenyAuthenticatedAccessPolicy>();
				expectations.Expect<AccountController>(x => x.LogInAsPublisher()).Has<DenyAuthenticatedAccessPolicy>();
				expectations.Expect<AccountController>(x => x.LogOut()).Has<DenyAnonymousAccessPolicy>();

				expectations.Expect<ExampleController>(x => x.DenyAnonymousAccess()).Has<DenyAnonymousAccessPolicy>();
				expectations.Expect<ExampleController>(x => x.DenyAuthenticatedAccess()).Has<DenyAuthenticatedAccessPolicy>();

				expectations.Expect<ExampleController>(x => x.RequireAdministratorRole()).Has(new RequireRolePolicy(UserRole.Administrator));
				expectations.Expect<ExampleController>(x => x.RequirePublisherRole()).Has(new RequireRolePolicy(UserRole.Publisher));

				expectations.Expect<AdminController>().Has<AdministratorPolicy>();
				expectations.Expect<AdminController>(x => x.Index()).Has<IgnorePolicy>().DoesNotHave<AdministratorPolicy>();
			});

			// Assert
			Assert.That(results.Valid(), results.ErrorMessages());
		}
	}
}