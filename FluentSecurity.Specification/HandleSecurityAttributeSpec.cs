using System.Web.Mvc;
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
			SecurityConfigurator.Configure(config => config.GetAuthenticationStatusFrom(() => true));

			// Act
			var attribute = new HandleSecurityAttribute();

			// Assert
			Assert.That(attribute.Handler, Is.TypeOf<SecurityHandler>());
			Assert.That(attribute.Context, Is.TypeOf<SecurityContext>());
		}
	}

	[TestFixture]
	[Category("HandleSecurityAttributeSpec")]
	public class When_creating_a_new_HandleSecurityAttribute_using_the_overloaded_constructor
	{
		[Test]
		public void Should_have_SecurityHandler_set()
		{
			// Arrange
			var securityHandler = new Mock<ISecurityHandler>();
			var securityContext = new Mock<ISecurityContext>();

			// Act
			var attribute = new HandleSecurityAttribute(securityHandler.Object, securityContext.Object);

			// Assert
			Assert.That(attribute.Handler, Is.EqualTo(securityHandler.Object));
			Assert.That(attribute.Context, Is.EqualTo(securityContext.Object));
		}
	}

	[TestFixture]
	[Category("HandleSecurityAttributeSpec")]
	public class When_executing_OnActionExecuting
	{
		[Test]
		public void Should_call_HandleSecurityFor_with_the_controllername_Blog_and_actionname_Index_passing_the_current_security_context()
		{
			// Arrange
			var securityHandler = new Mock<ISecurityHandler>();
			var securityContext = new MockSecurityContext();

			var handleSecurityAttribute = new HandleSecurityAttribute(securityHandler.Object, securityContext);
			var filterContext = MvcHelpers.GetFilterContextFor<BlogController>(x => x.Index());

			// Act
			handleSecurityAttribute.OnActionExecuting(filterContext);

			// Assert
			Assert.That(filterContext.Result, Is.Null);
			securityHandler.Verify(x => x.HandleSecurityFor(typeof(BlogController).FullName, "Index", securityContext), Times.Exactly(1));
		}

		[Test]
		public void Should_add_route_values_to_current_security_context()
		{
			// Arrange
			var securityHandler = new Mock<ISecurityHandler>();
			var securityContext = new MockSecurityContext();

			var handleSecurityAttribute = new HandleSecurityAttribute(securityHandler.Object, securityContext);
			var filterContext = MvcHelpers.GetFilterContextFor<BlogController>(x => x.Index());

			// Act
			handleSecurityAttribute.OnActionExecuting(filterContext);

			// Assert
			Assert.That(securityContext.Data.RouteValues, Is.Not.Null);
			Assert.That(securityContext.Data.RouteValues, Is.EqualTo(filterContext.RouteData.Values));
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
			SecurityConfigurator.Configure(policy =>
			{
				policy.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsTrue);
				policy.For<BlogController>(x => x.Index()).DenyAnonymousAccess();
			});

			var expectedResult = new ViewResult { ViewName = "SomeViewName" };

			var securityHandler = new Mock<ISecurityHandler>();
			var securityContext = new MockSecurityContext();
			securityHandler.Setup(x => x.HandleSecurityFor(typeof(BlogController).FullName, "Index", securityContext)).Returns(expectedResult);

			var handleSecurityAttribute = new HandleSecurityAttribute(securityHandler.Object, securityContext);
			var filterContext = MvcHelpers.GetFilterContextFor<BlogController>(x => x.Index());

			// Act
			handleSecurityAttribute.OnActionExecuting(filterContext);

			// Assert
			Assert.That(filterContext.Result, Is.EqualTo(expectedResult));
		}
	}
}