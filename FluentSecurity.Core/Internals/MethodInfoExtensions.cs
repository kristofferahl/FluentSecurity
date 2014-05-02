using System;
using System.Linq;
using System.Reflection;

namespace FluentSecurity.Core.Internals
{
	public static class MethodInfoExtensions
	{
		public static bool IsDeclaredBy<T>(this MethodInfo methodInfo)
		{
			var passedType = typeof(T);
			var declaringType = methodInfo.GetBaseDefinition().DeclaringType;
			return declaringType != null && declaringType.IsAssignableFrom(passedType);
		}

		public static bool HasAttribute<TAttribute>(this MethodInfo methodInfo) where TAttribute : Attribute
		{
			return methodInfo.GetCustomAttributes(typeof(TAttribute), false).Any();
		}
	}
}