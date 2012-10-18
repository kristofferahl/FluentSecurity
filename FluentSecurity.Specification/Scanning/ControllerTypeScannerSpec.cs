using System.Linq;
using System.Web.Mvc;
using FluentSecurity.Scanning;
using NUnit.Framework;

namespace FluentSecurity.Specification.Scanning
{
	[TestFixture]
	[Category("ControllerTypeScannerSpec")]
	public class When_scanning_for_controllers
	{
		[Test]
		public void Should_find_all_controllers()
		{
			// Arrange
			var scanner = new ControllerTypeScanner();

			// Act
			var result = scanner.Scan(new[] { GetType().Assembly }).ToList();

			// Assert
			Assert.That(result.Count(), Is.EqualTo(8));
		}

		[Test]
		public void Should_find_all_controllers_inheriting_from_base_controller_including_base_controller()
		{
			// Arrange
			var scanner = new ControllerTypeScanner(typeof(BaseController));

			// Act
			var result = scanner.Scan(new[] { GetType().Assembly }).ToList();

			// Assert
			Assert.That(result.First(), Is.EqualTo(typeof(BaseController)));
			Assert.That(result.Last(), Is.EqualTo(typeof(IneritingBaseController)));
			Assert.That(result.Count(), Is.EqualTo(2));
		}

		[Test]
		public void Should_find_all_controllers_inheriting_from_abstract_base_controller_excluding_abstract_base_controller()
		{
			// Arrange
			var scanner = new ControllerTypeScanner(typeof(AbstractBaseController));

			// Act
			var result = scanner.Scan(new[] { GetType().Assembly }).ToList();

			// Assert
			Assert.That(result.Single(), Is.EqualTo(typeof(IneritingAbstractBaseController)));
		}

		public class IneritingBaseController : BaseController {}

		public class IneritingAbstractBaseController : AbstractBaseController {}

		public class BaseController : Controller {}
		
		public abstract class AbstractBaseController : Controller {}
	}
}