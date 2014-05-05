using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentSecurity.Specification.Helpers
{
	public static class FakeIoC
	{
		public static Stack<Type> CallStack = new Stack<Type>();

		public static Func<IEnumerable<object>> GetAllInstancesProvider = () => null;
		public static Func<IEnumerable<object>> GetInstanceProvider = () => null;

		public static IEnumerable<object> GetAllInstances(Type type)
		{
			CallStack.Push(type);
			return GetAllInstancesProvider().Where(x => type.IsAssignableFrom(x.GetType()));
		}

		public static object GetInstance(Type type)
		{
			CallStack.Push(type);
			return GetInstanceProvider().SingleOrDefault(x => type.IsAssignableFrom(x.GetType()));
		}

		public static void Reset()
		{
			GetAllInstancesProvider = () => null;
			GetInstanceProvider = () => null;
			CallStack = new Stack<Type>();
		}
	}
}