using System;
using System.Linq;
using System.Web.Mvc;
using FluentSecurity.Scanning.TypeScanners;
using FluentSecurity.Specification.TestData.Controllers;
using FluentSecurity.Specification.TestData.Controllers.BaseControllers;
using NUnit.Framework;

namespace FluentSecurity.Specification.Scanning.TypeScanners
{
	[TestFixture]
	[Category("ControllerTypeScannerSpec")]
	public class When_creating_a_controller_type_scanner
	{
		[Test]
		public void Should_set_the_controller_type_to_IController_for_empty_constructor()
		{
			// Arrange
			var expectedType = typeof(IController);

			// Act
			var scanner = new MvcControllerTypeScanner();

			// Assert
			Assert.That(scanner.ControllerType, Is.EqualTo(expectedType));
		}

		[Test]
		public void Should_throw_when_type_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new MvcControllerTypeScanner(null));
		}

		[Test]
		public void Should_set_the_controller_type()
		{
			// Arrange
			var expectedType = typeof (AdminController);
			
			// Act
			var scanner = new MvcControllerTypeScanner(expectedType);

			// Assert
			Assert.That(scanner.ControllerType, Is.EqualTo(expectedType));
		}
	}

	[TestFixture]
	[Category("ControllerTypeScannerSpec")]
	public class When_scanning_for_controllers
	{
		[Test]
		public void Should_find_all_controllers()
		{
			// Arrange
			var scanner = new MvcControllerTypeScanner();

			// Act
			var result = scanner.Scan(new[] { GetType().Assembly }).ToList();

			// Assert
			Assert.That(result.Count(), Is.EqualTo(10));
		}

		[Test]
		public void Should_find_all_controllers_inheriting_from_base_controller_including_base_controller()
		{
			// Arrange
			var scanner = new MvcControllerTypeScanner(typeof(BaseController));

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
			var scanner = new MvcControllerTypeScanner(typeof(AbstractBaseController));

			// Act
			var result = scanner.Scan(new[] { GetType().Assembly }).ToList();

			// Assert
			Assert.That(result.Single(), Is.EqualTo(typeof(IneritingAbstractBaseController)));
		}
	}
}