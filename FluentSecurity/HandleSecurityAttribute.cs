using System;
using System.ComponentModel;
using System.Web.Mvc;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class HandleSecurityAttribute : AuthorizeAttribute, IAuthorizationFilter
	{
		internal ISecurityHandler Handler { get; private set; }

		public HandleSecurityAttribute() : this(ServiceLocator.Current.Resolve<ISecurityHandler>()) {}

		public HandleSecurityAttribute(ISecurityHandler securityHandler)
		{
			Handler = securityHandler;
		}

		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			var actionName = filterContext.ActionDescriptor.ActionName;
			var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.FullName;

			var securityContext = SecurityContext.Current;
			securityContext.Data.RouteValues = filterContext.RouteData.Values;

			var overrideResult = Handler.HandleSecurityFor(controllerName, actionName, securityContext);
			if (overrideResult != null) filterContext.Result = overrideResult;
		}

		[Obsolete("Not applicable in this class.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		new public string Roles { get; set; }

		[Obsolete("Not applicable in this class.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		new public string Users { get; set; }

		[Obsolete("Not applicable in this class.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		new public int Order { get; set; }
	}
}