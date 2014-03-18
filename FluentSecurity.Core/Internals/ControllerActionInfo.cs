using System;

namespace FluentSecurity.Internals
{
	public class ControllerActionInfo
	{
		public Type ControllerType { get; private set; }
		public string ActionName { get; private set; }
		public Type ActionResultType { get; private set; }

		public ControllerActionInfo(Type controller, string actionName, Type returnType)
		{
			ControllerType = controller;
			ActionName = actionName;
			ActionResultType = returnType;
		}
	}
}