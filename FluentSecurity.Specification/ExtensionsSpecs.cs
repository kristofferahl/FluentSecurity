using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using FluentSecurity.Policy;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("ExtensionsSpecs")]
	public class When_getting_the_container
	{
		private ICollection<IPolicyContainer> _containers;

		[SetUp]
		public void SetUp()
		{
			// Arrange
			_containers = new Collection<IPolicyContainer>
			{
				TestDataFactory.CreateValidPolicyContainer("Controller", "ActionThatDoesExist")
			};
		}

		[Test]
		public void Should_return_a_container_for_Controller_ActionThatDoesExist()
		{
			// Act
			var policyContainer = _containers.GetContainerFor("Controller", "ActionThatDoesExist");

			// Assert
			Assert.That(policyContainer, Is.Not.Null);
		}

		[Test]
		public void Should_return_null_for_Controller_ActionThatDoesNotExists()
		{
			// Act
			var policyContainer = _containers.GetContainerFor("Controller", "ActionThatDoesNotExists");

			// Assert
			Assert.That(policyContainer, Is.Null);
		}

		[Test]
		public void Should_return_a_container_for_controller_ActionThatDoesExist()
		{
			// Act
			var policyContainer = _containers.GetContainerFor("controller", "ActionThatDoesExist");

			// Assert
			Assert.That(policyContainer, Is.Not.Null);
		}

		[Test]
		public void Should_return_a_container_for_Controller_actionthatdoesexist()
		{
			// Act
			var policyContainer = _containers.GetContainerFor("Controller", "actionthatdoesexist");

			// Assert
			Assert.That(policyContainer, Is.Not.Null);
		}

		[Test]
		public void Should_return_a_container_for_controller_actionthatdoesexist()
		{
			// Act
			var policyContainer = _containers.GetContainerFor("controller", "actionthatdoesexist");

			// Assert
			Assert.That(policyContainer, Is.Not.Null);
		}

		[Test]
		public void Should_return_a_container_for_Controller_ActIonThatDoesExist_EN()
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			var policyContainer = _containers.GetContainerFor("Controller", "ActIonThatDoesExist");
			Assert.That(policyContainer, Is.Not.Null);
		}

		[Test]
		public void Should_return_a_container_for_Controller_ActIonThatDoesExist_TR()
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");
			var policyContainer = _containers.GetContainerFor("Controller", "ActIonThatDoesExist");
			Assert.That(policyContainer, Is.Not.Null);
		}
	}

	[TestFixture]
	[Category("ExtensionsSpecs")]
	public class When_getting_the_are_name_from_route_data
	{
		[Test]
		public void Should_return_the_are_name_from_data_tokens()
		{
			// Arrange
			var routeData = new RouteData();
			routeData.DataTokens.Add("area", "AreaName");

			// Act
			var areaName = routeData.GetAreaName();

			// Assert
			Assert.That(areaName, Is.EqualTo("AreaName"));
		}
	}

	[TestFixture]
	[Category("ExtensionsSpecs")]
	public class When_getting_the_area_name_from_route_base
	{
		[Test]
		public void Should_return_the_are_name_from_data_tokens()
		{
			// Arrange
			var route = new Route("some-url", new MvcRouteHandler());
			route.DataTokens = new RouteValueDictionary();
			route.DataTokens.Add("area", "AreaName");

			// Act
			var areaName = route.GetAreaName();

			// Assert
			Assert.That(areaName, Is.EqualTo("AreaName"));
		}

		[Test]
		public void Should_return_the_are_name_from_IRouteWithArea()
		{
			// Arrange
			var route = new AreaRoute();

			// Act
			var areaName = route.GetAreaName();

			// Assert
			Assert.That(areaName, Is.EqualTo("AreaName"));
		}

		[Test]
		public void Should_return_emtpy_string_when_DataTokens_is_null()
		{
			// Arrange
			var route = new Route("some-url", new MvcRouteHandler());

			// Act
			var areaName = route.GetAreaName();

			// Assert
			Assert.That(areaName, Is.Empty);
		}

		private class AreaRoute : Route, IRouteWithArea
		{
			public AreaRoute() : base("some-url", new MvcRouteHandler()) { }

			public string Area
			{
				get { return "AreaName"; }
			}
		}
	}

	[TestFixture]
	[Category("ExtensionsSpecs")]
	public class When_getting_the_action_name
	{
		[Test]
		public void Should_handle_UnaryExpression()
		{
			// Arrange
			Expression<Func<TestController, object>> expression = x => x.UnaryExpression();

			// Act
			var name = expression.GetActionName();

			// Assert
			Assert.That(name, Is.EqualTo("UnaryExpression"));
		}

		[Test]
		public void Should_handle_InstanceMethodCallExpression()
		{
			// Arrange
			Expression<Func<TestController, object>> expression = x => x.InstanceMethodCallExpression();

			// Act
			var name = expression.GetActionName();

			// Assert
			Assert.That(name, Is.EqualTo("InstanceMethodCallExpression"));
		}
		
		[Test]
		public void Should_consider_ActionNameAttibute()
		{
			// Arrange
			Expression<Func<TestController, object>> expression = x => x.ActualAction();

			// Act
			var name = expression.GetActionName();

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

	[TestFixture]
	[Category("ExtensionsSpecs")]
	public class When_getting_the_policy_type_of_an_ISecurityPolicy
	{
		[Test]
		public void Should_retun_the_type_of_normal_policies()
		{
			// Arrange
			ISecurityPolicy policy = new IgnorePolicy();

			// Act & assert
			Assert.That(policy.GetPolicyType(), Is.EqualTo(typeof(IgnorePolicy)));
		}

		[Test]
		public void Should_retun_the_type_of_lazy_policies()
		{
			// Arrange
			ISecurityPolicy policy = new LazySecurityPolicy<IgnorePolicy>();

			// Act & assert
			Assert.That(policy.GetPolicyType(), Is.EqualTo(typeof(IgnorePolicy)));
		}
	}

	[TestFixture]
	[Category("ExtensionsSpecs")]
	public class When_checking_if_a_type_is_a_controller_action_return_type
	{
		[Test]
		public void Should_be_true_for_ActionResult()
		{
			Assert.That(typeof(ActionResult).IsControllerActionReturnType(), Is.True);
		}

		[Test]
		public void Should_be_true_for_ContentResult()
		{
			Assert.That(typeof(ContentResult).IsControllerActionReturnType(), Is.True);
		}

		[Test]
		public void Should_be_true_for_EmptyResult()
		{
			Assert.That(typeof(EmptyResult).IsControllerActionReturnType(), Is.True);
		}

		[Test]
		public void Should_be_true_for_FileResult()
		{
			Assert.That(typeof(FileResult).IsControllerActionReturnType(), Is.True);
		}

		[Test]
		public void Should_be_true_for_HttpStatusCodeResult()
		{
			Assert.That(typeof(HttpStatusCodeResult).IsControllerActionReturnType(), Is.True);
		}

		[Test]
		public void Should_be_true_for_JavaScriptResult()
		{
			Assert.That(typeof(JavaScriptResult).IsControllerActionReturnType(), Is.True);
		}

		[Test]
		public void Should_be_true_for_JsonResult()
		{
			Assert.That(typeof(JsonResult).IsControllerActionReturnType(), Is.True);
		}

		[Test]
		public void Should_be_true_for_PartialViewResult()
		{
			Assert.That(typeof(PartialViewResult).IsControllerActionReturnType(), Is.True);
		}

		[Test]
		public void Should_be_true_for_RedirectResult()
		{
			Assert.That(typeof(RedirectResult).IsControllerActionReturnType(), Is.True);
		}

		[Test]
		public void Should_be_true_for_RedirectToRouteResult()
		{
			Assert.That(typeof(RedirectToRouteResult).IsControllerActionReturnType(), Is.True);
		}

		[Test]
		public void Should_be_true_for_ViewResult()
		{
			Assert.That(typeof(ViewResult).IsControllerActionReturnType(), Is.True);
		}

		[Test]
		public void Should_be_true_for_Task_of_ActionResult()
		{
			Assert.That(typeof(Task<ActionResult>).IsControllerActionReturnType(), Is.True);
		}

		[Test]
		public void Should_be_true_for_Task_of_ContentResult()
		{
			Assert.That(typeof(Task<ContentResult>).IsControllerActionReturnType(), Is.True);
		}

		[Test]
		public void Should_be_true_for_Task_of_EmptyResult()
		{
			Assert.That(typeof(Task<EmptyResult>).IsControllerActionReturnType(), Is.True);
		}

		[Test]
		public void Should_be_true_for_Task_of_FileResult()
		{
			Assert.That(typeof(Task<FileResult>).IsControllerActionReturnType(), Is.True);
		}

		[Test]
		public void Should_be_true_for_Task_of_HttpStatusCodeResult()
		{
			Assert.That(typeof(Task<HttpStatusCodeResult>).IsControllerActionReturnType(), Is.True);
		}

		[Test]
		public void Should_be_true_for_Task_of_JavaScriptResult()
		{
			Assert.That(typeof(Task<JavaScriptResult>).IsControllerActionReturnType(), Is.True);
		}

		[Test]
		public void Should_be_true_for_Task_of_JsonResult()
		{
			Assert.That(typeof(Task<JsonResult>).IsControllerActionReturnType(), Is.True);
		}

		[Test]
		public void Should_be_true_for_Task_of_PartialViewResult()
		{
			Assert.That(typeof(Task<PartialViewResult>).IsControllerActionReturnType(), Is.True);
		}

		[Test]
		public void Should_be_true_for_Task_of_RedirectResult()
		{
			Assert.That(typeof(Task<RedirectResult>).IsControllerActionReturnType(), Is.True);
		}

		[Test]
		public void Should_be_true_for_Task_of_RedirectToRouteResult()
		{
			Assert.That(typeof(Task<RedirectToRouteResult>).IsControllerActionReturnType(), Is.True);
		}

		[Test]
		public void Should_be_true_for_Task_of_ViewResult()
		{
			Assert.That(typeof(Task<ViewResult>).IsControllerActionReturnType(), Is.True);
		}
	}

}