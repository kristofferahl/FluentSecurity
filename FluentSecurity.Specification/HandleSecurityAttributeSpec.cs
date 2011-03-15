using System.Web.Mvc;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("HandleSecurityAttributeSpec")]
	public class When_creating_a_new_HandleSecurityAttribute_using_the_default_constructor
	{
		[Test]
		public void Should_have_SecurityHandler_set()
		{
			SecurityConfigurator.Configure(config => {});

			// Act
			var attribute = new HandleSecurityAttribute();

			// Assert
			Assert.That(attribute.SecurityHandler, Is.AssignableTo(typeof(ISecurityHandler)));
			Assert.That(attribute.SecurityHandler, Is.Not.Null);
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
			var mockSecurityHandler = MockRepository.GenerateMock<ISecurityHandler>();

			// Act
			var attribute = new HandleSecurityAttribute(mockSecurityHandler);

			// Assert
			Assert.That(attribute.SecurityHandler, Is.AssignableTo(typeof(ISecurityHandler)));
			Assert.That(attribute.SecurityHandler, Is.EqualTo(mockSecurityHandler));
		}
	}

	[TestFixture]
	[Category("HandleSecurityAttributeSpec")]
	public class When_executing_OnActionExecuting
	{
		[Test]
		public void Should_call_HandleSecurityFor_with_the_controllername_Blog_and_actionname_Index()
		{
			// Arrange
			SecurityConfigurator.Configure(policy =>
			{
				policy.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsTrue);
				policy.For<BlogController>(x => x.Index()).DenyAnonymousAccess();
			});

			var securityHandler = MockRepository.GenerateMock<ISecurityHandler>();
			securityHandler.Expect(x => x.HandleSecurityFor("Blog", "Index")).Repeat.Once().Return(null);
			securityHandler.Replay();

			var handleSecurityAttribute = new HandleSecurityAttribute(securityHandler);
			var filterContext = MvcHelpers.GetFilterContextFor<BlogController>(x => x.Index());

			// Act
			handleSecurityAttribute.OnActionExecuting(filterContext);

			// Assert
			Assert.That(filterContext.Result, Is.Null);
			securityHandler.VerifyAllExpectations();
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

			var securityHandler = MockRepository.GenerateMock<ISecurityHandler>();
			securityHandler.Expect(x => x.HandleSecurityFor("Blog", "Index")).Repeat.Once().Return(expectedResult);
			securityHandler.Replay();

			var handleSecurityAttribute = new HandleSecurityAttribute(securityHandler);
			var filterContext = MvcHelpers.GetFilterContextFor<BlogController>(x => x.Index());

			// Act
			handleSecurityAttribute.OnActionExecuting(filterContext);

			// Assert
			Assert.That(filterContext.Result, Is.EqualTo(expectedResult));
			securityHandler.VerifyAllExpectations();
		}
	}
}