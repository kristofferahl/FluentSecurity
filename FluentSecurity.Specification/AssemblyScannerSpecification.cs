using System;
using System.Linq;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	public abstract class AssemblyScannerSpecification
	{
		protected static void Because(Action<ConfigurationExpression> configurationExpression)
		{
			// Arrange
			var expression = TestDataFactory.CreateValidConfigurationExpression();
			configurationExpression(expression);
			AssertAllControllerActionsHasContainer(expression);
		}

		private static void AssertAllControllerActionsHasContainer(ConfigurationExpression configurationExpression)
		{
			Assert.That(configurationExpression.Count(), Is.EqualTo(9));
			var blog = NameHelper<BlogController>.Controller();
			var admin = NameHelper<AdminController>.Controller();
			Assert.That(configurationExpression.GetContainerFor(blog, "Index"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor(blog, "ListPosts"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor(blog, "AddPost"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor(blog, "EditPost"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor(blog, "DeletePost"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor(blog, "AjaxList"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor(admin, "Index"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor(admin, "LogIn"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor(admin, "LogOut"), Is.Not.Null);
		}
	}
}