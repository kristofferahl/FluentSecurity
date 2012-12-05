using System.Web.Mvc;
using System.Web.Routing;

namespace FluentSecurity.SampleApplication.Helpers
{
	public static class TagBuilderExtensions
	{
		public static TagBuilder AddAttributes(this TagBuilder tagBuilder, object htmlAttributes)
		{
			return tagBuilder.AddAttributes(htmlAttributes, false);
		}

		public static TagBuilder AddAttributes(this TagBuilder tagBuilder, object htmlAttributes, bool replaceExistingAttributes)
		{
			var attributes = new RouteValueDictionary(htmlAttributes);
			tagBuilder.MergeAttributes(attributes, replaceExistingAttributes);
			return tagBuilder;
		}
	}
}