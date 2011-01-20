using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentSecurity.Specification
{
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
			securityHandler.Expect(x => x.HandleSecurityFor("Blog", "Index")).Repeat.Once();
			securityHandler.Replay();

			var handleSecurityAttribute = new HandleSecurityAttribute(securityHandler);
			var filterContext = MvcHelpers.GetFilterContextFor<BlogController>(x => x.Index());

			// Act
			handleSecurityAttribute.OnActionExecuting(filterContext);

			// Assert
			securityHandler.VerifyAllExpectations();
		}
	}
}