using System;
using System.Linq;
using FluentSecurity.Specification.Helpers;
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
			Assert.That(configurationExpression.GetContainerFor("Blog", "Index"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor("Blog", "ListPosts"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor("Blog", "AddPost"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor("Blog", "EditPost"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor("Blog", "DeletePost"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor("Blog", "AjaxList"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor("Admin", "Index"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor("Admin", "LogIn"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor("Admin", "LogOut"), Is.Not.Null);
		}
	}
}