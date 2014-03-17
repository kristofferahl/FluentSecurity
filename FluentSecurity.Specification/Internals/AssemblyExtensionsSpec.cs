using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentSecurity.Specification.TestData;
using FluentSecurity.Specification.TestData.Controllers;
using NUnit.Framework;
using FluentSecurity.Core.Internals;

namespace FluentSecurity.Specification.Internals
{
	[TestFixture]
	[Category("AssemblyExtensionsSpec")]
	public class When_getting_the_types_of_an_assembly
	{
		[SetUp]
		public void SetUp()
		{
			AssemblyExtensions.Reset();
		}

		[TearDown]
		public void TearDown()
		{
			AssemblyExtensions.Reset();
		}

		[Test]
		public void Should_throw_when_assembly_is_null()
		{
			// Arrange
			Assembly assembly = null;

			// Act
			Assert.Throws<ArgumentNullException>(() => assembly.GetLoadableTypes());
		}

		[Test]
		public void Should_return_types()
		{
			// Arrange
			var assembly = GetType().Assembly;

			// Act
			var types = assembly.GetLoadableTypes();

			// Assert
			Assert.That(types.Any(), Is.True);
		}

		[Test]
		public void Should_return_types_successfully_loaded()
		{
			// Arrange
			AssemblyExtensions.GetTypesProvider = a =>
			{
				throw new ReflectionTypeLoadException(new List<Type>
				{
					typeof(AdminController),
					null,
					typeof(BlogController),
					null,
					null
				}.ToArray(), new Exception[0]);
			};
			var assembly = GetType().Assembly;

			// Act
			var types = assembly.GetLoadableTypes();

			// Assert
			Assert.That(types.Count(), Is.EqualTo(2));
		}
	}

	[TestFixture]
	[Category("AssemblyExtensionsSpec")]
	public class When_getting_the_exported_types_of_an_assembly
	{
		[SetUp]
		public void SetUp()
		{
			AssemblyExtensions.Reset();
		}

		[Test]
		public void Should_throw_when_assembly_is_null()
		{
			// Arrange
			Assembly assembly = null;

			// Act
			Assert.Throws<ArgumentNullException>(() => assembly.GetLoadableExportedTypes());
		}

		[Test]
		public void Should_return_types()
		{
			// Arrange
			var assembly = GetType().Assembly;

			// Act
			var types = assembly.GetLoadableExportedTypes();

			// Assert
			Assert.That(types.Any(), Is.True);
		}

		[Test]
		public void Should_return_types_successfully_loaded()
		{
			// Arrange
			AssemblyExtensions.GetTypesProvider = a =>
			{
				throw new ReflectionTypeLoadException(new List<Type>
				{
					typeof(AdminController),
					null,
					typeof(BlogController),
					null,
					null
				}.ToArray(), new Exception[0]);
			};
			var assembly = GetType().Assembly;

			// Act
			var types = assembly.GetLoadableExportedTypes();

			// Assert
			Assert.That(types.Count(), Is.EqualTo(2));
		}
	}
}