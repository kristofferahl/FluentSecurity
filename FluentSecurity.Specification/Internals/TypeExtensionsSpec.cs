using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentSecurity.Core.Internals;
using NUnit.Framework;

namespace FluentSecurity.Specification.Internals
{
	[TestFixture]
	[Category("TypeExtensionsSpec")]
	public class When_checking_for_empty_constructor
	{
		[Test]
		public void Should_be_false()
		{
			// Arrange
			var type = typeof(ClassWithNonEmptyConstructor);

			// Act
			var result = type.HasEmptyConstructor();

			// Assert
			Assert.That(result, Is.False);
		}

		[Test]
		public void Should_be_true()
		{
			// Arrange
			var type = typeof(ClassWithEmptyConstructor);

			// Act
			var result = type.HasEmptyConstructor();

			// Assert
			Assert.That(result, Is.True);
		}

		public class ClassWithNonEmptyConstructor
		{
			public ClassWithNonEmptyConstructor(string value) { }
		}

		public class ClassWithEmptyConstructor { }
	}

	[TestFixture]
	[Category("TypeExtensionsSpec")]
	public class When_checking_if_Type_A_is_assignable_from_generic_Type_B
	{
		[Test]
		public void Should_be_assignable_when_types_are_GenericType_of_BaseType_and_GenericType_of_TypeA()
		{
			// Arrange
			var genericType1 = typeof(GenericType<BaseType>);
			var genericType2 = typeof(GenericType<TypeA>);

			// Act
			var result = genericType1.IsAssignableFromGenericType(genericType2);

			// Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void Should_be_assignable_when_types_are_GenericType_of_BaseType_and_GenericTypeA()
		{
			// Arrange
			var genericType1 = typeof(GenericType<BaseType>);
			var genericType2 = typeof(GenericTypeA);

			// Act
			var result = genericType1.IsAssignableFromGenericType(genericType2);

			// Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void Should_be_assignable_when_types_are_GenericType_of_BaseType_and_GenericType_of_TypeB()
		{
			// Arrange
			var genericType1 = typeof(GenericType<BaseType>);
			var genericType2 = typeof(GenericType<TypeB>);

			// Act
			var result = genericType1.IsAssignableFromGenericType(genericType2);

			// Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void Should_be_assignable_when_types_are_GenericType_of_BaseType_and_GenericTypeB()
		{
			// Arrange
			var genericType1 = typeof(GenericType<BaseType>);
			var genericType2 = typeof(GenericTypeB);

			// Act
			var result = genericType1.IsAssignableFromGenericType(genericType2);

			// Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void Should_be_assignable_when_types_are_GenericType_of_TypeA_and_GenericTypeA()
		{
			// Arrange
			var genericType1 = typeof(GenericType<TypeA>);
			var genericType2 = typeof(GenericTypeA);

			// Act
			var result = genericType1.IsAssignableFromGenericType(genericType2);

			// Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void Should_be_assignable_when_types_are_GenericType_of_TypeB_and_GenericTypeB()
		{
			// Arrange
			var genericType1 = typeof(GenericType<TypeB>);
			var genericType2 = typeof(GenericTypeB);

			// Act
			var result = genericType1.IsAssignableFromGenericType(genericType2);

			// Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void Should_be_assignable_when_types_are_GenericType_of_BaseType_and_GenericType_of_TypeC()
		{
			// Arrange
			var genericType1 = typeof(GenericType<BaseType>);
			var genericType2 = typeof(GenericType<TypeC>);

			// Act
			var result = genericType1.IsAssignableFromGenericType(genericType2);

			// Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void Should_be_assignable_when_types_are_GenericType_of_BaseType_and_GenericTypeC()
		{
			// Arrange
			var genericType1 = typeof(GenericType<BaseType>);
			var genericType2 = typeof(GenericTypeC);

			// Act
			var result = genericType1.IsAssignableFromGenericType(genericType2);

			// Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void Should_be_assignable_when_types_are_GenericType_of_TypeA_and_GenericType_of_TypeC()
		{
			// Arrange
			var genericTypeA = typeof(GenericType<TypeA>);
			var genericTypeB = typeof(GenericType<TypeC>);

			// Act
			var result = genericTypeA.IsAssignableFromGenericType(genericTypeB);

			// Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void Should_be_assignable_when_types_are_GenericType_of_BaseType_and_ExtendingGenericType_of_BaseType_TypeD()
		{
			// Arrange
			var genericType1 = typeof(GenericType<BaseType>);
			var genericType2 = typeof(ExtendingGenericType<BaseType, TypeD>);

			// Act
			var result = genericType1.IsAssignableFromGenericType(genericType2);

			// Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void Should_be_assignable_when_types_are_GenericType_of_BaseType_and_GenericTypeD()
		{
			// Arrange
			var genericType1 = typeof(GenericType<BaseType>);
			var genericType2 = typeof(GenericTypeD);

			// Act
			var result = genericType1.IsAssignableFromGenericType(genericType2);

			// Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void Should_be_assignable_when_Type_A_and_Type_B_has_no_generic_arguments()
		{
			// Arrange
			var genericTypeA = typeof(GenericType<>);
			var genericTypeB = typeof(GenericType<>);

			// Act
			var result = genericTypeA.IsAssignableFromGenericType(genericTypeB);

			// Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void Should_not_be_assignable_when_Type_A_and_Type_B_has_different_generic_arguments()
		{
			// Arrange
			var genericTypeA = typeof(GenericType<TypeA>);
			var genericTypeB = typeof(GenericType<TypeB>);

			// Act
			var result = genericTypeA.IsAssignableFromGenericType(genericTypeB);

			// Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void Should_not_be_assignable_when_types_are_GenericTypeA_and_GenericType_of_TypeA()
		{
			// Arrange
			var nonGenericType1 = typeof(GenericTypeA);
			var genericType2 = typeof(GenericType<TypeA>);

			// Act
			var result = nonGenericType1.IsAssignableFromGenericType(genericType2);

			// Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void Should_not_be_assignable_when_types_are_GenericTypeB_and_GenericType_of_TypeB()
		{
			// Arrange
			var nonGenericType1 = typeof(GenericTypeB);
			var genericType2 = typeof(GenericType<TypeB>);

			// Act
			var result = nonGenericType1.IsAssignableFromGenericType(genericType2);

			// Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void Should_not_be_assignable_when_second_type_is_not_generic_and_has_no_base_type()
		{
			// Arrange
			var genericType1 = typeof(GenericType<TypeA>);
			var genericType2 = typeof(object);

			// Act
			var result = genericType1.IsAssignableFromGenericType(genericType2);

			// Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void Should_not_be_assignable_when_second_type_is_non_matching_generic_and_has_no_base_type()
		{
			// Arrange
			var genericType1 = typeof(GenericType<TypeA>);
			var genericType2 = typeof(OtherGenericType<TypeA>);

			// Act
			var result = genericType1.IsAssignableFromGenericType(genericType2);

			// Assert
			Assert.IsFalse(result);
		}

		public class TypeA : BaseType {}

		public class TypeB : BaseType {}

		public class TypeC : TypeA {}

		public class TypeD : TypeB {}

		public class BaseType {}

		public class GenericTypeA : GenericType<TypeA> {}

		public class GenericTypeB : GenericType<TypeB> {}

		public class GenericTypeC : GenericType<TypeC> {}

		public class GenericTypeD : ExtendingGenericType<BaseType, object> {}

		public class GenericType<T> where T : BaseType {}

		public class OtherGenericType<T> where T : class {}

		public class ExtendingGenericType<T1, T2> : GenericType<T1>
			where T1 : BaseType
			where T2 : class
		{}
	}

	[TestFixture]
	[Category("TypeExtensionsSpec")]
	public class When_matching_generic_types
	{
		[Test]
		public void Should_be_false_when_obj_is_null()
		{
			// Arrange
			object obj = null;

			// Act
			var result = obj.IsMatchForGenericType(typeof(List<>));

			// Assert
			Assert.That(result, Is.False);
		}

		[Test]
		public void Should_be_false_when_obj_is_not_a_generic_type()
		{
			// Arrange
			object obj = new List();

			// Act
			var result = obj.IsMatchForGenericType(typeof(List<>));

			// Assert
			Assert.That(result, Is.False);
		}

		[Test]
		public void Should_be_false_when_obj_is_not_matching_generic_type()
		{
			// Arrange
			object obj = new Collection<int>();

			// Act
			var result = obj.IsMatchForGenericType(typeof(List<>));

			// Assert
			Assert.That(result, Is.False);
		}

		[Test]
		public void Should_be_true_when_obj_is_not_matching_generic_type()
		{
			// Arrange
			object obj = new List<int>();

			// Act
			var result = obj.IsMatchForGenericType(typeof(List<>));

			// Assert
			Assert.That(result, Is.True);
		}

		[Test]
		public void Should_throw_when_generic_type_argument_is_not_a_generic_type()
		{
			// Arrange
			object obj = new List<int>();

			// Act & assert
			Assert.Throws<ArgumentException>(() => obj.IsMatchForGenericType(typeof(List)));
		}
	}
}