using System;
using System.Web.Http.Controllers;
using FluentSecurity.Scanning.TypeScanners;

namespace FluentSecurity.WebApi.Scanning.TypeScanners
{
	public class WebApiControllerTypeScanner : ControllerTypeScanner
	{
		public WebApiControllerTypeScanner() : this(typeof(IHttpController)) {}

		public WebApiControllerTypeScanner(Type controllerType) : base(controllerType) {}
	}
}