using System;
using System.Web.Routing;
using FluentSecurity.Policy.Contexts;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy.Contexts
{
	[TestFixture]
	[Category("MvcSecurityContextSpec")]
	public class When_creating_an_MvcSecurityContext
	{
		[Test]
		public void Should_throw_when_inner_context_is_null()
		{
			// Arrange
			ISecurityContext innerSecurityContext = null;

			// Act & assert
			Assert.Throws<ArgumentNullException>(() => new MvcSecurityContext(innerSecurityContext));
		}

		[Test]
		public void Should_have_route_values_from_Data_RouteValues_property()
		{
			// Arrange
			ISecurityContext innerSecurityContext = new MockSecurityContext();
			innerSecurityContext.Data.RouteValues = new RouteValueDictionary();

			// Act
			var context = new MvcSecurityContext(innerSecurityContext);

			// Assert
			Assert.That(context.RouteValues, Is.EqualTo(innerSecurityContext.Data.RouteValues));
		}
	}
}