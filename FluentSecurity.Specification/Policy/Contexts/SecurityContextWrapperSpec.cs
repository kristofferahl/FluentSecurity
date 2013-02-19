using FluentSecurity.Policy.Contexts;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy.Contexts
{
	[TestFixture]
	[Category("SecurityContextWrapperSpec")]
	public class When_creating_a_context_inheriting_from_SecurityContextWrapper
	{
		[Test]
		public void Should_have_all_properties_set_from_inner_context()
		{
			// Arrange
			var innerContext = TestDataFactory.CreateSecurityContext(false);

			// Act
			var outerContext = new TestSecurityContext(innerContext);

			// Assert
			Assert.That(outerContext.Id, Is.EqualTo(innerContext.Id));
			Assert.That(outerContext.Data, Is.EqualTo(innerContext.Data));
			Assert.That(outerContext.CurrentUserIsAuthenticated(), Is.EqualTo(innerContext.CurrentUserIsAuthenticated()));
			Assert.That(outerContext.CurrentUserRoles(), Is.EqualTo(innerContext.CurrentUserRoles()));
			Assert.That(outerContext.Runtime, Is.EqualTo(innerContext.Runtime));
		}
	}

	public class TestSecurityContext : SecurityContextWrapper
	{
		public TestSecurityContext(ISecurityContext innerContext) : base(innerContext) {}
	}
}