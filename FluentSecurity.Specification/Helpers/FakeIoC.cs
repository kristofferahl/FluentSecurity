using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentSecurity.Specification.Helpers
{
	public static class FakeIoC
	{
		public static Func<IEnumerable<object>> GetAllInstancesProvider = () => null;
		public static Func<IEnumerable<object>> GetInstanceProvider = () => null;

		public static IEnumerable<object> GetAllInstances(Type type)
		{
			return GetAllInstancesProvider().Where(x => type.IsAssignableFrom(x.GetType()));
		}

		public static object GetInstance(Type type)
		{
			return GetInstanceProvider().SingleOrDefault(x => type.IsAssignableFrom(x.GetType()));
		}

		public static void Reset()
		{
			GetAllInstancesProvider = () => null;
			GetInstanceProvider = () => null;
		}
	}
}