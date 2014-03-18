using System;
using System.Web.Mvc;

namespace FluentSecurity.Scanning.TypeScanners
{
	public class MvcControllerTypeScanner : ControllerTypeScanner
	{
		public MvcControllerTypeScanner() : this(typeof(IController)) {}

		public MvcControllerTypeScanner(Type controllerType) : base(controllerType) {}
	}
}