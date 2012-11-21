using System.Web.Mvc;

namespace FluentSecurity.SampleApplication.Helpers
{
	public static class HtmlExtensions
	{
		public static string NavigationLink(this HtmlHelper htmlHelper, string url, string innerHtml)
		{
			return htmlHelper.NavigationLink(url, innerHtml, null, null, true);
		}

		public static string NavigationLink(this HtmlHelper htmlHelper, string url, string innerHtml, bool markSelectedWhenActive)
		{
			return htmlHelper.NavigationLink(url, innerHtml, null, null, markSelectedWhenActive);
		}

		public static string NavigationLink(this HtmlHelper htmlHelper, string url, string innerHtml, object htmlAttributes)
		{
			return htmlHelper.NavigationLink(url, innerHtml, htmlAttributes, null, true);
		}

		public static string NavigationLink(this HtmlHelper htmlHelper, string url, string innerHtml, string wrapperElement)
		{
			return htmlHelper.NavigationLink(url, innerHtml, null, wrapperElement, true);
		}

		public static string NavigationLink(this HtmlHelper htmlHelper, string url, string innerHtml, object htmlAttributes, string wrapperElement)
		{
			return htmlHelper.NavigationLink(url, innerHtml, htmlAttributes, wrapperElement, true);
		}

		public static string NavigationLink(this HtmlHelper htmlHelper, string url, string innerHtml, object htmlAttributes, string wrapperElement, bool markSelectedWhenActive)
		{
			if (string.IsNullOrEmpty(url)) return string.Empty;

			var tagBuilder = new TagBuilder("a");
			tagBuilder.MergeAttribute("href", url);
			tagBuilder.AddAttributes(htmlAttributes);
			if (UrlMatchesCurrentPath(url, htmlHelper) && string.IsNullOrEmpty(wrapperElement))
			{
				tagBuilder.MergeAttribute("class", "selected");
			}
			tagBuilder.InnerHtml = innerHtml;
			var navigationLink = tagBuilder.ToString(TagRenderMode.Normal);

			if (!string.IsNullOrEmpty(wrapperElement))
			{
				var wrapperElementBuilder = new TagBuilder(wrapperElement);
				if (UrlMatchesCurrentPath(url, htmlHelper))
				{
					wrapperElementBuilder.MergeAttribute("class", "selected");
				}
				wrapperElementBuilder.InnerHtml = navigationLink;
				navigationLink = wrapperElementBuilder.ToString(TagRenderMode.Normal);
			}

			return navigationLink;
		}

		private static bool UrlMatchesCurrentPath(string url, HtmlHelper htmlHelper)
		{
			if (htmlHelper == null ||
				htmlHelper.ViewContext == null ||
				htmlHelper.ViewContext.HttpContext == null ||
				htmlHelper.ViewContext.HttpContext.Request == null)
			{
				return false;
			}

			var currentPath = htmlHelper.ViewContext.HttpContext.Request.Path;

			return UrlsMatch(url, currentPath);
		}

		private static bool UrlsMatch(string url, string currentPath)
		{
			return (url == currentPath || currentPath.StartsWith(url + "/"));
		}
	}
}