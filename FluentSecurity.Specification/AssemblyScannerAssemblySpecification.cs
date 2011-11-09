using System;
using System.Linq;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	public abstract class AssemblyScannerAssemblySpecification
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
			Assert.That(configurationExpression.Count(), Is.EqualTo(12));
			var blog = NameHelper<BlogController>.Controller();
			var admin = NameHelper<AdminController>.Controller();
			var root = NameHelper<TestData.AssemblyScannerControllers.RootController>.Controller();
			var include = NameHelper<TestData.AssemblyScannerControllers.Include.IncludedController>.Controller();
			var exclude = NameHelper<TestData.AssemblyScannerControllers.Exclude.ExcludedController>.Controller();

			Assert.That(configurationExpression.GetContainerFor(blog, "Index"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor(blog, "ListPosts"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor(blog, "AddPost"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor(blog, "EditPost"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor(blog, "DeletePost"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor(blog, "AjaxList"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor(admin, "Index"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor(admin, "LogIn"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor(admin, "LogOut"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor(root , "Index"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor(include, "Index"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor(exclude, "Index"), Is.Not.Null);
		}
	}
}