using System.Collections.Generic;
using FluentSecurity.Policy.Contexts;

namespace FluentSecurity.WebApi.Policy
{
	public class WebApiSecurityContext : SecurityContextWrapper
	{
		public WebApiSecurityContext(ISecurityContext innerSecurityContext) : base(innerSecurityContext)
		{
			RouteValues = Data.RouteValues;
		}

		public IDictionary<string, object> RouteValues { get; private set; }
	}
}