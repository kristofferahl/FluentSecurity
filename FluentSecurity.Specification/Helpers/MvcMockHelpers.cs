using System;
using System.Collections.Specialized;
using System.Web;
using Moq;

namespace FluentSecurity.Specification.Helpers
{
	public static class MvcMockHelpers
	{
		public static HttpContextBase FakeHttpContext()
		{
			var context = new Mock<HttpContextBase>();
			var request = new Mock<HttpRequestBase>();
			var response = new Mock<HttpResponseBase>();
			var session = new Mock<HttpSessionStateBase>();
			var server = new Mock<HttpServerUtilityBase>();

			context.Setup(ctx => ctx.Request).Returns(request.Object);
			context.Setup(ctx => ctx.Response).Returns(response.Object);
			context.Setup(ctx => ctx.Session).Returns(session.Object);
			context.Setup(ctx => ctx.Server).Returns(server.Object);

			return context.Object;
		}

		public static HttpContextBase FakeHttpContext(string url)
		{
			HttpContextBase context = FakeHttpContext();
			context.Request.SetupRequestUrl(url);
			return context;
		}

		static string GetUrlFileName(string url)
		{
			if (url.Contains("?"))
				return url.Substring(0, url.IndexOf("?"));
			return url;
		}

		static NameValueCollection GetQueryStringParameters(string url)
		{
			if (url.Contains("?"))
			{
				var parameters = new NameValueCollection();

				string[] parts = url.Split("?".ToCharArray());
				string[] keys = parts[1].Split("&".ToCharArray());

				foreach (string key in keys)
				{
					string[] part = key.Split("=".ToCharArray());
					parameters.Add(part[0], part[1]);
				}

				return parameters;
			}
			return null;
		}

		private static void SetupRequestUrl(this HttpRequestBase request, string url)
		{
			if (url == null)
				throw new ArgumentNullException("url");

			if (!url.StartsWith("~/"))
				throw new ArgumentException("Sorry, we expect a virtual url starting with \"~/\".");

			var mock = Mock.Get(request);

			mock.Setup(req => req.QueryString)
				.Returns(GetQueryStringParameters(url));
			mock.Setup(req => req.AppRelativeCurrentExecutionFilePath)
				.Returns(GetUrlFileName(url));
			mock.Setup(req => req.PathInfo)
				.Returns(string.Empty);
		}
	}
}