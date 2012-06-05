using System.Web;
using System.Web.Routing;
using FluentSecurity.Internals;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification.Internals
{
	[TestFixture]
	[Category("HttpContextRequestDescriptionSpec")]
	public class When_creating_a_HttpContextRequestDescription
	{
		[TearDown]
		public void TearDown()
		{
			RouteTable.Routes.Clear();
			HttpContextRequestDescription.HttpContextProvider = HttpContextRequestDescription.DefaultHttpContextProvider;
		}

		[Test]
		public void Should_return_httpcontext_from_httpcontextprovider()
		{
			// Act
			var httpRequest = new HttpRequest("file", "http://localhost/", "query");
			var httpResponse = new HttpResponse(new TextMessageWriter());
			HttpContext.Current = new HttpContext(httpRequest, httpResponse);
			var httpContextProvider = HttpContextRequestDescription.HttpContextProvider();

			// Assert
			Assert.That(httpContextProvider, Is.Not.Null);
		}

		[Test]
		public void Should_have_default_httpcontextprovider()
		{
			// Act
			var httpRequest = new HttpRequest("file", "http://localhost/", "query");
			var httpResponse = new HttpResponse(new TextMessageWriter());
			HttpContext.Current = new HttpContext(httpRequest, httpResponse);
			var httpContextProvider = HttpContextRequestDescription.DefaultHttpContextProvider;

			// Assert
			Assert.That(httpContextProvider, Is.Not.Null);
		}

		[Test]
		public void Should_have_areaname_controllername_and_actionname()
		{
			// Arrange
			HttpContextRequestDescription.HttpContextProvider = () => MvcMockHelpers.FakeHttpContext("~/some-url");
			RouteTable.Routes.Add(TestDataFactory.CreateRoute("AreaName", "ControllerName", "ActionName"));

			// Act
			var requestDescription = new HttpContextRequestDescription();

			// Assert
			Assert.That(requestDescription.AreaName, Is.EqualTo("AreaName"));
			Assert.That(requestDescription.ControllerName, Is.EqualTo("ControllerName"));
			Assert.That(requestDescription.ActionName, Is.EqualTo("ActionName"));
		}

		[Test]
		public void Should_have_controllername_and_actionname()
		{
			// Arrange
			HttpContextRequestDescription.HttpContextProvider = () => MvcMockHelpers.FakeHttpContext("~/some-url");
			RouteTable.Routes.Add(TestDataFactory.CreateRoute(null, "ControllerName", "ActionName"));

			// Act
			var requestDescription = new HttpContextRequestDescription();

			// Assert
			Assert.That(requestDescription.AreaName, Is.EqualTo(""));
			Assert.That(requestDescription.ControllerName, Is.EqualTo("ControllerName"));
			Assert.That(requestDescription.ActionName, Is.EqualTo("ActionName"));
		}
	}
}