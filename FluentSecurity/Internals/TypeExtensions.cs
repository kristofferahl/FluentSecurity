using System;
using System.Linq;

namespace FluentSecurity.Internals
{
	public static class TypeExtensions
	{
		internal static bool IsAssignableFromGenericType(this Type firstType, Type secondType)
		{
			if (!firstType.IsGenericType)
				return false;

			if (!secondType.IsGenericType && secondType.BaseType == null)
				return false;

			if (secondType.IsGenericType && firstType.HasSameGenericSignature(secondType))
				return true;

			var baseType = secondType.BaseType;
			if (baseType == null || !baseType.IsGenericType)
				return false;

			return firstType.IsAssignableFromGenericType(baseType);
		}

		private static bool HasSameGenericSignature(this Type firstType, Type secondType)
		{
			if (firstType.GetGenericTypeDefinition() != secondType.GetGenericTypeDefinition())
				return false;

			var firstTs = firstType.GetGenericArguments();
			var secondTs = secondType.GetGenericArguments();

			for (var i = 0; i < firstTs.Length; ++i)
			{
				var firstT = firstTs.ElementAt(i);
				var secondT = secondTs.ElementAt(i);
				if (!firstT.IsAssignableFrom(secondT))
					return false;
			}

			return true;
		}
	}
}