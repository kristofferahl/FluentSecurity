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
    public class When_security_has_base_controller
    {
        [Test]
        public void Should_be_configured_base_actions()
        {
            // Arrange
            var _expectations = new PolicyExpectations();
            _expectations.For<HomeController>().Has<DenyAnonymousAccessPolicy>();
            _expectations.For<Areas.ExampleArea.Controllers.BaseController>().Has(new RequireRolePolicy(UserRole.Administrator));
            _expectations.For<Areas.ExampleArea.Controllers.RolesController>(p => p.Index()).Has(new RequireRolePolicy(UserRole.Administrator));
            _expectations.For<Areas.ExampleArea.Controllers.RolesController>().Has(new RequireRolePolicy(UserRole.Administrator));

            SecurityConfigurator.Configure(c => {
                c.ForAllControllersInAssembly(typeof(HomeController).Assembly).DenyAnonymousAccess();
                c.ForAllControllersInAssemblyThatInherit<Areas.ExampleArea.Controllers.BaseController>().RequireRole(UserRole.Administrator);
            });

            // Act
            var results = _expectations.VerifyAll(SecurityConfiguration.Current);

            // Assert
            Assert.That(results.Valid(), results.ErrorMessages());
        }
    }
}