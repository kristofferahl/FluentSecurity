using System;
using System.Linq.Expressions;
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
		public void Should_handle_UnaryExpression()
		{
			// Arrange
			Expression<Func<TestController, object>> expression = x => x.UnaryExpression();

			// Act
			var name = _actionNameResolver.Resolve(expression);

			// Assert
			Assert.That(name, Is.EqualTo("UnaryExpression"));
		}

		[Test]
		public void Should_handle_InstanceMethodCallExpression()
		{
			// Arrange
			Expression<Func<TestController, object>> expression = x => x.InstanceMethodCallExpression();

			// Act
			var name = _actionNameResolver.Resolve(expression);

			// Assert
			Assert.That(name, Is.EqualTo("InstanceMethodCallExpression"));
		}

		[Test]
		public void Should_consider_ActionNameAttibute()
		{
			// Arrange
			Expression<Func<TestController, object>> expression = x => x.ActualAction();

			// Act
			var name = _actionNameResolver.Resolve(expression);

			// Assert
			Assert.That(name, Is.EqualTo("AliasAction"));
		}

		private class TestController
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