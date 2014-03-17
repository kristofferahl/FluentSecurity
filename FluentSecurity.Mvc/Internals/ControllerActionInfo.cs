using System;
using System.Reflection;

namespace FluentSecurity.Internals
{
	public class ControllerActionInfo
	{
		public Type ControllerType { get; private set; }
		public string ActionName { get; private set; }
		public Type ActionResultType { get; private set; }

		internal ControllerActionInfo(Type controller, MethodInfo action)
		{
			ControllerType = controller;
			ActionName = action.GetActionName();
			ActionResultType = action.ReturnType;
		}
	}
}