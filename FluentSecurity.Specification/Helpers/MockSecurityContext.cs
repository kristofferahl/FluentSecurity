using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web.Routing;

namespace FluentSecurity.Specification.Helpers
{
	public class MockSecurityContext : ISecurityContext
	{
		private readonly bool _isAuthenticated;
		private readonly IEnumerable<object> _roles;

		public MockSecurityContext(bool isAuthenticated = true, IEnumerable<object> roles = null, RouteValueDictionary routeValues = null, ISecurityRuntime runtime = null)
		{
			Id = Guid.NewGuid();
			
			_isAuthenticated = isAuthenticated;
			_roles = roles;

			Data = new ExpandoObject();
			Data.RouteValues = routeValues;

			Runtime = runtime ?? TestDataFactory.CreateSecurityRuntime();
		}

		public Guid Id { get; private set; }

		public dynamic Data { get; private set; }

		public bool CurrentUserIsAuthenticated()
		{
			return _isAuthenticated;
		}

		public IEnumerable<object> CurrentUserRoles()
		{
			return _roles;
		}

		public ISecurityRuntime Runtime { get; private set; }
	}
}