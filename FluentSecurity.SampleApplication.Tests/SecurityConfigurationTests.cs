using System.Collections.Generic;
using FluentSecurity.Policy;
using FluentSecurity.SampleApplication.Controllers;
using FluentSecurity.SampleApplication.Models;
using FluentSecurity.TestHelper;
using NUnit.Framework;

namespace FluentSecurity.SampleApplication.Tests
{
	[TestFixture]
	[Category("SecurityConfigurationTests")]
	public class When_security_is_configured : SecurityExpectationHelper
	{
		protected override ISecurityConfiguration ConfigurationToTest()
		{
			return Bootstrapper.SetupFluentSecurity();
		}

		[Test]
		public void Should_be_configured_correctly()
		{
			Expect<AccountController>(x => x.LogInAsAdministrator()).Has<DenyAuthenticatedAccessPolicy>();
			Expect<AccountController>(x => x.LogInAsPublisher()).Has<DenyAuthenticatedAccessPolicy>();
			Expect<AccountController>(x => x.LogOut()).Has<DenyAnonymousAccessPolicy>();

			Expect<ExampleController>(x => x.DenyAnonymousAccess()).Has<DenyAnonymousAccessPolicy>();
			Expect<ExampleController>(x => x.DenyAuthenticatedAccess()).Has<DenyAuthenticatedAccessPolicy>();

			Expect<ExampleController>(x => x.RequireAdministratorRole()).Has(new RequireRolePolicy(new List<object> { UserRole.Administrator }.ToArray()));
			Expect<ExampleController>(x => x.RequirePublisherRole()).Has(new RequireRolePolicy(new List<object> { UserRole.Publisher }.ToArray()));

			Expect<AdminController>(x => x.Add()).Has<AdministratorPolicy>();
			Expect<AdminController>(x => x.Edit()).Has<AdministratorPolicy>();
			Expect<AdminController>(x => x.Delete()).Has<AdministratorPolicy>();
			Expect<AdminController>(x => x.Index()).Has<IgnorePolicy>().DoesNotHave<AdministratorPolicy>();
		}
	}

	[TestFixture]
	[Category("SecurityConfigurationTests")]
	public class When_security_is_configured_for_account_controller : SecurityExpectationHelper<AccountController>
	{
		protected override ISecurityConfiguration ConfigurationToTest()
		{
			return Bootstrapper.SetupFluentSecurity();
		}

		[Test]
		public void Should_be_configured_correctly()
		{
			Expect(x => x.LogInAsAdministrator()).Has<DenyAuthenticatedAccessPolicy>();
			Expect(x => x.LogInAsPublisher()).Has<DenyAuthenticatedAccessPolicy>();
			Expect(x => x.LogOut()).Has<DenyAnonymousAccessPolicy>();
		}
	}
}