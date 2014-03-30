using System.Linq;
using FluentSecurity.Configuration;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification.Configuration
{
	[TestFixture]
	[Category("SecurityProfileSpec")]
	public class When_configuring_a_security_profile_using_ForAllControllers
	{
		[Test]
		public void Should_find_controllers_in_the_assembly_of_the_profile()
		{
			// Arrange
			var profile = new ForAllControllersProfile();
			profile.Initialize(TestDataFactory.CreateSecurityRuntime());
			var expectedNamespace = profile.GetType().Assembly.GetName().Name;

			// Act
			profile.Configure();

			// Assert
			Assert.That(profile.Runtime.PolicyContainers.Any(), Is.True);
			Assert.That(profile.Runtime.PolicyContainers.All(pc => pc.ControllerName.StartsWith(expectedNamespace)), Is.True);
		}

		private class ForAllControllersProfile : SecurityProfile
		{
			public override void Configure()
			{
				ForAllControllers().DenyAnonymousAccess();
			}
		}
	}
}