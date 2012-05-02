using System.Collections.Generic;
using System.Dynamic;
using System.Web.Routing;

namespace FluentSecurity.Specification.Helpers
{
	public class MockSecurityContext : ISecurityContext
	{
		private readonly bool _isAuthenticated;
		private readonly IEnumerable<object> _roles;

		public MockSecurityContext(bool isAuthenticated = true, IEnumerable<object> roles = null, RouteValueDictionary routeValues = null)
		{
			_isAuthenticated = isAuthenticated;
			_roles = roles;

			Data = new ExpandoObject();
			Data.RouteValues = routeValues;
		}

		public dynamic Data { get; private set; }

		public bool CurrenUserAuthenticated()
		{
			return _isAuthenticated;
		}

		public IEnumerable<object> CurrenUserRoles()
		{
			return _roles;
		}
	}
}