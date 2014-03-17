using System;
using System.Linq;

namespace FluentSecurity.Core.Internals
{
	public static class TypeExtensions
	{
		public static bool HasEmptyConstructor(this Type type)
		{
			var constructors = type.GetConstructors();
			var hasEmptyConstructor = constructors.Any(x => !x.GetParameters().Any());
			return hasEmptyConstructor;
		}

		public static bool IsMatchForGenericType(this object obj, Type genericType)
		{
			if (!genericType.IsGenericType) throw new ArgumentException("The specified type is not a generic type", "genericType");
			if (obj == null) return false;
			var type = obj.GetType();
			return type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
		}

		public static bool IsAssignableFromGenericType(this Type firstType, Type secondType)
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