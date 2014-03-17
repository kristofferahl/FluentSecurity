using System.Web.Routing;

namespace FluentSecurity.Policy.Contexts
{
	public class MvcSecurityContext : SecurityContextWrapper
	{
		public MvcSecurityContext(ISecurityContext innerSecurityContext) : base(innerSecurityContext)
		{
			RouteValues = Data.RouteValues;
		}

		public RouteValueDictionary RouteValues { get; private set; }
	}
}