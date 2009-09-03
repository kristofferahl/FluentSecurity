using System.Web.Mvc;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("SecureControllerSpec")]
	public class When_creating_a_SecureController
	{
		[Test]
		public void Should_inherit_from_Controller()
		{
			// Arrange
			var secureController = new SecureController();

			// Assert
			Assert.That(secureController.GetType().BaseType, Is.EqualTo(typeof(Controller)));
		}

		[Test]
		public void Should_have_HandleSecurityAttribute()
		{
			// Arrange
			var secureController = new SecureController();
			var attribute = secureController.GetType().GetCustomAttributes(typeof(HandleSecurityAttribute), true);

			// Assert
			Assert.That(attribute, Is.Not.Null);
		}
	}
}