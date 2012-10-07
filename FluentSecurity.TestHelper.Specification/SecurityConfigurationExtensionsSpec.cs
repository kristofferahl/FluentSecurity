using System;
using System.Linq;
using FluentSecurity.Policy;
using FluentSecurity.TestHelper.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.TestHelper.Specification
{
	[TestFixture]
	[Category("SecurityConfigurationExtensions")]
	public class When_verifying_expectations_for_a_security_configuration
	{
		[Test]
		public void Should_throw_when_configuration_is_null()
		{
			ISecurityConfiguration securityConfiguration = null;
			
			Assert.Throws<ArgumentNullException>(() =>
				securityConfiguration.Verify(expectations => {})
			);
		}

		[Test]
		public void Should_throw_when_expectations_are_null()
		{
			ISecurityConfiguration securityConfiguration = FluentSecurityFactory.CreateSecurityConfigurationWithTwoExpectations();

			Assert.Throws<ArgumentNullException>(() =>
				securityConfiguration.Verify(null)
			);
		}

		[Test]
		public void Should_not_throw()
		{
			ISecurityConfiguration securityConfiguration = FluentSecurityFactory.CreateSecurityConfiguration();

			Assert.DoesNotThrow(() =>
				securityConfiguration.Verify(expectations => {})
			);
		}

		[Test]
		public void Should_verify_expectations()
		{
			ISecurityConfiguration securityConfiguration = FluentSecurityFactory.CreateSecurityConfiguration();

			var results = securityConfiguration.Verify(expectations =>
			{
				expectations.Expect<AdminController>().Has<DenyAnonymousAccessPolicy>();
				expectations.Expect<AdminController>(x => x.Login()).DoesNotHave<DenyAnonymousAccessPolicy>().Has<DenyAuthenticatedAccessPolicy>();
				expectations.Expect<AdminController>(x => x.NewUser())
					.DoesNotHave<DenyAnonymousAccessPolicy>()
					.DoesNotHave<RequireRolePolicy>(p => p.RolesRequired.Contains(UserRole.UserViewer))
					.Has<RequireRolePolicy>(p => p.RolesRequired.Contains(UserRole.UserEditor));
			});

			Assert.That(results.All(x => x.ExpectationsMet), results.ErrorMessages());
		}
		
		[Test]
		public void Should_verify_expectations_for_aliased_action()
		{
			SecurityConfigurator.Configure(configuration  => 
               {
				configuration.For<SampleController>(a => a.ActualAction()).DenyAnonymousAccess();
               });
			var policyExpectations = new PolicyExpectations();
			policyExpectations.For<SampleController>(a => a.ActualAction()).Has<DenyAnonymousAccessPolicy>();
			var results = policyExpectations.VerifyAll(SecurityConfiguration.Current);
			Assert.That(results.Valid(), results.ErrorMessages());
		}
	}

	[TestFixture]
	[Category("SecurityConfigurationExtensions")]
	public class When_verifying_expectations_for_a_security_configuration_with_generic_extension
	{
		[Test]
		public void Should_throw_when_configuration_is_null()
		{
			ISecurityConfiguration securityConfiguration = null;

			Assert.Throws<ArgumentNullException>(() =>
				securityConfiguration.Verify<AdminController>(expectations => { })
			);
		}

		[Test]
		public void Should_throw_when_expectations_are_null()
		{
			ISecurityConfiguration securityConfiguration = FluentSecurityFactory.CreateSecurityConfigurationWithTwoExpectations();

			Assert.Throws<ArgumentNullException>(() =>
				securityConfiguration.Verify<AdminController>(null)
			);
		}

		[Test]
		public void Should_not_throw()
		{
			ISecurityConfiguration securityConfiguration = FluentSecurityFactory.CreateSecurityConfiguration();

			Assert.DoesNotThrow(() =>
				securityConfiguration.Verify<AdminController>(expectations => { })
			);
		}

		[Test]
		public void Should_verify_expectations()
		{
			ISecurityConfiguration securityConfiguration = FluentSecurityFactory.CreateSecurityConfiguration();

			var results = securityConfiguration.Verify<AdminController>(expectations =>
			{
				expectations.Expect().Has<DenyAnonymousAccessPolicy>();
				expectations.Expect(x => x.Login()).DoesNotHave<DenyAnonymousAccessPolicy>().Has<DenyAuthenticatedAccessPolicy>();
				expectations.Expect(x => x.NewUser())
					.DoesNotHave<DenyAnonymousAccessPolicy>()
					.DoesNotHave<RequireRolePolicy>(p => p.RolesRequired.Contains(UserRole.UserViewer))
					.Has<RequireRolePolicy>(p => p.RolesRequired.Contains(UserRole.UserEditor));
			});

			Assert.That(results.All(x => x.ExpectationsMet), results.ErrorMessages());
		}
	}
}