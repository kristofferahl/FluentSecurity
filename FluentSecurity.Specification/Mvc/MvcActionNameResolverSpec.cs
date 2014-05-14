using System;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Web.Mvc;
using NUnit.Framework;

namespace FluentSecurity.Specification.Mvc
{
	[TestFixture]
	[Category("MvcActionNameResolverSpec")]
	public class When_getting_the_action_name
	{
		private MvcActionNameResolver _actionNameResolver;

		[SetUp]
		public void SetUp()
		{
			_actionNameResolver = new MvcActionNameResolver();
		}

		[Test]
		public void Should_resolve_action_name_UnaryExpression_from_AuthorizationContext()
		{
			// Arrange
			const string expectedActionName = "UnaryExpression";
			var controllerType = typeof(TestController);
			var controllerDescriptor = new ReflectedControllerDescriptor(controllerType);
			var method = controllerType.GetMethod(expectedActionName);
			var actionDescriptor = new ReflectedActionDescriptor(method, expectedActionName, controllerDescriptor);
			var context = new AuthorizationContext { ActionDescriptor = actionDescriptor };

			// Act
			var name = _actionNameResolver.Resolve(context);

			// Assert
			Assert.That(name, Is.EqualTo(expectedActionName));
		}

		[Test]
		public void Should_resolve_action_name_InstanceMethodCallExpression_from_AuthorizationContext()
		{
			// Arrange
			const string expectedActionName = "InstanceMethodCallExpression";
			var controllerType = typeof (TestController);
			var controllerDescriptor = new ReflectedControllerDescriptor(controllerType);
			var method = controllerType.GetMethod(expectedActionName);
			var actionDescriptor = new ReflectedActionDescriptor(method, expectedActionName, controllerDescriptor);
			var context = new AuthorizationContext { ActionDescriptor = actionDescriptor };

			// Act
			var name = _actionNameResolver.Resolve(context);

			// Assert
			Assert.That(name, Is.EqualTo(expectedActionName));
		}

		[Test]
		public void Should_resolve_action_name_AliasAction_from_AuthorizationContext()
		{
			// Arrange
			const string expectedActionName = "AliasAction";
			var controllerType = typeof(TestController);
			var controllerDescriptor = new ReflectedControllerDescriptor(controllerType);
			var method = controllerType.GetMethod("ActualAction");
			var actionDescriptor = new ReflectedActionDescriptor(method, expectedActionName, controllerDescriptor);
			var context = new AuthorizationContext { ActionDescriptor = actionDescriptor };

			// Act
			var name = _actionNameResolver.Resolve(context);

			// Assert
			Assert.That(name, Is.EqualTo(expectedActionName));
		}

		[Test]
		public void Should_resolve_action_name_UnaryExpression_from_MethodInfo()
		{
			// Arrange
			const string expectedActionName = "UnaryExpression";
			var controllerType = typeof(TestController);
			var method = controllerType.GetMethod(expectedActionName);

			// Act
			var name = _actionNameResolver.Resolve(method);

			// Assert
			Assert.That(name, Is.EqualTo(expectedActionName));
		}

		[Test]
		public void Should_resolve_action_name_InstanceMethodCallExpression_from_MethodInfo()
		{
			// Arrange
			const string expectedActionName = "InstanceMethodCallExpression";
			var controllerType = typeof(TestController);
			var method = controllerType.GetMethod(expectedActionName);

			// Act
			var name = _actionNameResolver.Resolve(method);

			// Assert
			Assert.That(name, Is.EqualTo(expectedActionName));
		}

		[Test]
		public void Should_resolve_action_name_AliasAction_from_MethodInfo()
		{
			// Arrange
			const string expectedActionName = "AliasAction";
			var controllerType = typeof(TestController);
			var method = controllerType.GetMethod("ActualAction");

			// Act
			var name = _actionNameResolver.Resolve(method);

			// Assert
			Assert.That(name, Is.EqualTo(expectedActionName));
		}

		[Test]
		public void Should_resolve_action_name_UnaryExpression_from_LambdaExpression()
		{
			// Arrange
			Expression<Func<TestController, object>> expression = x => x.UnaryExpression();

			// Act
			var name = _actionNameResolver.Resolve(expression);

			// Assert
			Assert.That(name, Is.EqualTo("UnaryExpression"));
		}

		[Test]
		public void Should_resolve_action_name_InstanceMethodCallExpression_from_LambdaExpression()
		{
			// Arrange
			Expression<Func<TestController, object>> expression = x => x.InstanceMethodCallExpression();

			// Act
			var name = _actionNameResolver.Resolve(expression);

			// Assert
			Assert.That(name, Is.EqualTo("InstanceMethodCallExpression"));
		}

		[Test]
		public void Should_resolve_action_name_ActualAction_from_LambdaExpression()
		{
			// Arrange
			Expression<Func<TestController, object>> expression = x => x.ActualAction();

			// Act
			var name = _actionNameResolver.Resolve(expression);

			// Assert
			Assert.That(name, Is.EqualTo("AliasAction"));
		}

		private class TestController : Controller
		{
			public Boolean UnaryExpression()
			{
				return false;
			}

			public ActionResult InstanceMethodCallExpression()
			{
				return new EmptyResult();
			}

			[ActionName("AliasAction")]
			public ActionResult ActualAction()
			{
				return new EmptyResult();
			}
		}
	}
}