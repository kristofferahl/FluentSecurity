using System;
using System.Linq;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using FluentSecurity.Specification.TestData.Controllers;
using FluentSecurity.Specification.TestData.Controllers.AssemblyScannerControllers;
using FluentSecurity.Specification.TestData.Controllers.AssemblyScannerControllers.Exclude;
using FluentSecurity.Specification.TestData.Controllers.AssemblyScannerControllers.Include;
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
			var policyContainers = configurationExpression.Runtime.PolicyContainers;

			Assert.That(policyContainers.Count(), Is.EqualTo(21));
			var blog = NameHelper.Controller<BlogController>();
			var admin = NameHelper.Controller<AdminController>();
			var root = NameHelper.Controller<RootController>();
			var include = NameHelper.Controller<IncludedController>();
			var exclude = NameHelper.Controller<ExcludedController>();

			Assert.That(policyContainers.GetContainerFor(blog, "Index"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(blog, "ListPosts"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(blog, "AddPost"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(blog, "EditPost"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(blog, "DeletePost"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(blog, "AjaxList"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(admin, "Index"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(admin, "LogIn"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(admin, "LogOut"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(root, "Index"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(include, "Index"), Is.Not.Null);
			Assert.That(policyContainers.GetContainerFor(exclude, "Index"), Is.Not.Null);
		}
	}
}