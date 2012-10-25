using System;
using System.Reflection;

namespace FluentSecurity.Internals
{
	public class ControllerActionInfo
	{
		public Type Controller { get; private set; }
		public string Action { get; private set; }
		public Type ActionResult { get; private set; }

		internal ControllerActionInfo(Type controller, MethodInfo action)
		{
			Controller = controller;
			Action = action.GetActionName();
			ActionResult = action.ReturnType;
		}
	}
}