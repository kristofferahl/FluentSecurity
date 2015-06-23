using System.Collections.Generic;
using System.Web.Mvc;
using FluentSecurity.Configuration;
using FluentSecurity.Core;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using Moq;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("HandleSecurityAttributeSpec")]
	public class When_creating_a_new_HandleSecurityAttribute_using_the_default_constructor
	{
		[Test]
		public void Should_have_SecurityHandler_set()
		{
			SecurityConfigurator.Configure<MvcConfiguration>(config => config.GetAuthenticationStatusFrom(() => true));

			// Act
			var attribute = new HandleSecurityAttribute();

			// Assert
			Assert.That(attribute.Handler, Is.TypeOf<SecurityHandler>());
		}
	}

	[TestFixture]
	[Category("HandleSecurityAttributeSpec")]
	public class When_creating_a_new_HandleSecurityAttribute_using_the_overloaded_constructor
	{
		private HandleSecurityAttribute _attribute;
		private Mock<ISecurityHandler<ActionResult>> _securityHandler;
		private Mock<IControllerNameResolver<AuthorizationContext>> _controllerNameResolver;
		private Mock<IActionNameResolver<AuthorizationContext>> _actionNameResolver;

		[SetUp]
		public void SetUp()
		{
			// Arrange
			_securityHandler = new Mock<ISecurityHandler<ActionResult>>();
			_controllerNameResolver = new Mock<IControllerNameResolver<AuthorizationContext>>();
			_actionNameResolver = new Mock<IActionNameResolver<AuthorizationContext>>();

			// Act
			_attribute = new HandleSecurityAttribute(_securityHandler.Object, _controllerNameResolver.Object, _actionNameResolver.Object);
		}

		[Test]
		public void Should_have_SecurityHandler_set()
		{
			// Assert
			Assert.That(_attribute.Handler, Is.EqualTo(_securityHandler.Object));
		}

		[Test]
		public void Should_have_ControllerNameResolver_set()
		{
			// Assert
			Assert.That(_attribute.ControllerNameResolver, Is.EqualTo(_controllerNameResolver.Object));
		}

		[Test]
		public void Should_have_ActionNameResolver_set()
		{
			// Assert
			Assert.That(_attribute.ActionNameResolver, Is.EqualTo(_actionNameResolver.Object));
		}
	}

	[TestFixture]
	[Category("HandleSecurityAttributeSpec")]
	public class When_executing_OnActionExecuting
	{
		private MockSecurityContext _securityContext;
		private Mock<IControllerNameResolver<AuthorizationContext>> _controllerNameResolver;
		private Mock<IActionNameResolver<AuthorizationContext>> _actionNameResolver;

		[SetUp]
		public void SetUp()
		{
			_securityContext = new MockSecurityContext();
			_controllerNameResolver = new Mock<IControllerNameResolver<AuthorizationContext>>();
			_actionNameResolver = new Mock<IActionNameResolver<AuthorizationContext>>();

			_controllerNameResolver.Setup(x => x.Resolve(It.IsAny<AuthorizationContext>())).Returns(NameHelper.Controller<BlogController>());
			_actionNameResolver.Setup(x => x.Resolve(It.IsAny<AuthorizationContext>())).Returns("Index");

			FakeIoC.GetAllInstancesProvider = () => new List<object>
			{
				_securityContext
			};
			SecurityConfigurator.Configure<MvcConfiguration>(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsTrue);
				configuration.ResolveServicesUsing(FakeIoC.GetAllInstances);
			});
		}

		[Test]
		public void Should_call_HandleSecurityFor_with_the_controllername_Blog_and_actionname_Index_passing_the_current_security_context()
		{
			// Arrange
			var securityHandler = new Mock<ISecurityHandler<ActionResult>>();

			var handleSecurityAttribute = new HandleSecurityAttribute(securityHandler.Object, _controllerNameResolver.Object, _actionNameResolver.Object);
			var filterContext = MvcHelpers.GetAuthorizationContextFor<BlogController>(x => x.Index());

			// Act
			handleSecurityAttribute.OnAuthorization(filterContext);

			// Assert
			Assert.That(filterContext.Result, Is.Null);
			securityHandler.Verify(x => x.HandleSecurityFor(NameHelper.Controller<BlogController>(), "Index", _securityContext), Times.Exactly(1));
		}

		[Test]
		public void Should_add_route_values_to_current_security_context()
		{
			// Arrange
			var securityHandler = new Mock<ISecurityHandler<ActionResult>>();

			var handleSecurityAttribute = new HandleSecurityAttribute(securityHandler.Object, _controllerNameResolver.Object, _actionNameResolver.Object);
			var filterContext = MvcHelpers.GetAuthorizationContextFor<BlogController>(x => x.Index());

			// Act
			handleSecurityAttribute.OnAuthorization(filterContext);

			// Assert
			Assert.That(_securityContext.Data.RouteValues, Is.Not.Null);
			Assert.That(_securityContext.Data.RouteValues, Is.EqualTo(filterContext.RouteData.Values));
		}
	}

	[TestFixture]
	[Category("HandleSecurityAttributeSpec")]
	public class When_executing_OnActionExecuting_and_the_security_handler_returns_an_action_result
	{
		[Test]
		public void Should_set_the_result_of_the_filtercontext()
		{
			// Arrange
			SecurityConfigurator.Configure<MvcConfiguration>(policy =>
			{
				policy.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsTrue);
				policy.For<BlogController>(x => x.Index()).DenyAnonymousAccess();
			});

			var expectedResult = new ViewResult { ViewName = "SomeViewName" };

			var securityHandler = new Mock<ISecurityHandler<ActionResult>>();
			var controllerNameResolver = new Mock<IControllerNameResolver<AuthorizationContext>>();
			var actionNameResolver = new Mock<IActionNameResolver<AuthorizationContext>>();

			var controllerName = NameHelper.Controller<BlogController>();
			const string actionName = "Index";

			securityHandler.Setup(x => x.HandleSecurityFor(controllerName, actionName, It.IsAny<ISecurityContext>())).Returns(expectedResult);
			controllerNameResolver.Setup(x => x.Resolve(It.IsAny<AuthorizationContext>())).Returns(controllerName);
			actionNameResolver.Setup(x => x.Resolve(It.IsAny<AuthorizationContext>())).Returns(actionName);

			var handleSecurityAttribute = new HandleSecurityAttribute(securityHandler.Object, controllerNameResolver.Object, actionNameResolver.Object);
			var filterContext = MvcHelpers.GetAuthorizationContextFor<BlogController>(x => x.Index());

			// Act
			handleSecurityAttribute.OnAuthorization(filterContext);

			// Assert
			Assert.That(filterContext.Result, Is.EqualTo(expectedResult));
		}
	}
}