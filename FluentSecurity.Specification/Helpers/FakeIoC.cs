using System;
using System.Collections.Generic;

namespace FluentSecurity.Specification.Helpers
{
	public static class FakeIoC
	{
		public static Func<IEnumerable<object>> GetAllInstancesProvider = () => null; 

		public static IEnumerable<object> GetAllInstances(Type type)
		{
			return GetAllInstancesProvider();
		}
	}
}