using System;
using System.Collections.Generic;

namespace FluentSecurity.Policy.Contexts
{
	public class SecurityContextWrapper : ISecurityContext
	{
		private readonly ISecurityContext _securityContext;

		public SecurityContextWrapper(ISecurityContext securityContext)
		{
			if (securityContext == null)
				throw new ArgumentNullException("securityContext");
			
			_securityContext = securityContext;
		}

		public Guid Id
		{
			get { return _securityContext.Id; }
		}

		public dynamic Data
		{
			get { return _securityContext.Data; }
		}

		public bool CurrentUserIsAuthenticated()
		{
			return _securityContext.CurrentUserIsAuthenticated();
		}

		public IEnumerable<object> CurrentUserRoles()
		{
			return _securityContext.CurrentUserRoles();
		}

		public ISecurityRuntime Runtime
		{
			get { return _securityContext.Runtime; }
		}
	}
}