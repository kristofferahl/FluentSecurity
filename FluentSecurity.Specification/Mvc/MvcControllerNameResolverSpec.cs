using System;
using System.Web.Mvc;
using NUnit.Framework;

namespace FluentSecurity.Specification.Mvc
{
	[TestFixture]
	[Category("MvcControllerNameResolverSpec")]
	public class When_getting_the_controller_name
	{
		private MvcControllerNameResolver _controllerNameResolver;

		[SetUp]
		public void SetUp()
		{
			_controllerNameResolver = new MvcControllerNameResolver();
		}

		[Test]
		public void Should_resolve_controller_name_TestController_from_AuthorizationContext()
		{
			// Arrange
			const string expectedControllerName = "FluentSecurity.Specification.Mvc.When_getting_the_controller_name+TestController";
			const string actionName = "SomeAction";
			var controllerType = typeof(TestController);
			var controllerDescriptor = new ReflectedControllerDescriptor(controllerType);
			var method = controllerType.GetMethod(actionName);
			var actionDescriptor = new ReflectedActionDescriptor(method, actionName, controllerDescriptor);
			var context = new AuthorizationContext { ActionDescriptor = actionDescriptor };

			// Act
			var name = _controllerNameResolver.Resolve(context);

			// Assert
			Assert.That(name, Is.EqualTo(expectedControllerName));
		}

		[Test]
		public void Should_resolve_controller_name_TestController_from_Type()
		{
			// Arrange
			const string expectedControllerName = "FluentSecurity.Specification.Mvc.When_getting_the_controller_name+TestController";
			var controllerType = typeof(TestController);

			// Act
			var name = _controllerNameResolver.Resolve(controllerType);

			// Assert
			Assert.That(name, Is.EqualTo(expectedControllerName));
		}

		private class TestController : Controller
		{
			public ActionResult SomeAction()
			{
				return null;
			}
		}
	}
}