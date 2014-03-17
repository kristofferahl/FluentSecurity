using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Configuration;
using FluentSecurity.Core;
using FluentSecurity.Policy;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification.Configuration
{
	[TestFixture]
	[Category("ViolationConfigurationSpec")]
	public class When_creating_a_ViolationConfiguration
	{
		[Test]
		public void Should_throw_when_conventions_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new ViolationConfiguration(null));
		}
	}

	[TestFixture]
	[Category("ViolationConfigurationSpec")]
	public class When_adding_a_convention
	{
		[Test]
		public void Should_throw_when_convention_is_null()
		{
			// Arrange
			var configuration = TestDataFactory.CreatedValidViolationConfiguration();

			// Act & assert
			Assert.Throws<ArgumentNullException>(() => configuration.AddConvention(null));
		}

		[Test]
		public void Should_add_convention()
		{
			// Arrange
			var expectedConvention = new MockConvention();
			var conventions = new List<IConvention>();
			var configuration = TestDataFactory.CreatedValidViolationConfiguration(conventions);

			// Act
			configuration.AddConvention(expectedConvention);

			// Assert
			Assert.That(conventions.First(), Is.EqualTo(expectedConvention));
		}
	}

	[TestFixture]
	[Category("ViolationConfigurationSpec")]
	public class When_removing_a_convention
	{
		[Test]
		public void Should_throw_when_predicate_is_null()
		{
			// Arrange
			var configuration = TestDataFactory.CreatedValidViolationConfiguration();

			// Act & assert
			Assert.Throws<ArgumentNullException>(() => configuration.RemoveConventions(null));
		}
		
		[Test]
		public void Should_remove_conventions_matching_predicate()
		{
			// Arrange
			var conventions = new List<IConvention> { new MockConvention() };
			var configuration = TestDataFactory.CreatedValidViolationConfiguration(conventions);

			// Act
			configuration.RemoveConventions(c => c is MockConvention);

			// Assert
			Assert.That(conventions.Any(), Is.False);
		}

		[Test]
		public void Should_remove_conventions_matching_type()
		{
			// Arrange
			var conventions = new List<IConvention> { new MockConvention() };
			var configuration = TestDataFactory.CreatedValidViolationConfiguration(conventions);

			// Act
			configuration.RemoveConventions<MockConvention>();

			// Assert
			Assert.That(conventions.Any(), Is.False);
		}

		[Test]
		public void Should_not_remove_conventions_not_matching_predicate()
		{
			// Arrange
			var conventions = new List<IConvention> { new MockConvention() };
			var configuration = TestDataFactory.CreatedValidViolationConfiguration(conventions);

			// Act
			configuration.RemoveConventions(c => c is NonMatchingConvention);

			// Assert
			Assert.That(conventions.Any(), Is.True);
		}

		[Test]
		public void Should_not_remove_conventions_not_matching_type()
		{
			// Arrange
			var conventions = new List<IConvention> { new MockConvention() };
			var configuration = TestDataFactory.CreatedValidViolationConfiguration(conventions);

			// Act
			configuration.RemoveConventions<NonMatchingConvention>();

			// Assert
			Assert.That(conventions.Any(), Is.True);
		}

		public class NonMatchingConvention : MockConvention {}
	}

	[TestFixture]
	[Category("ViolationConfigurationSpec")]
	public class When_calling_Of_on_ViolationConfiguration
	{
		[Test]
		public void Should_not_add_any_convention_and_return_ViolationHandlerConfiguration_of_T()
		{
			// Arrange
			var conventions = new List<IConvention>();
			var configuration = TestDataFactory.CreatedValidViolationConfiguration(conventions);

			// Act
			var result = configuration.Of<IgnorePolicy>();

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(conventions.Any(), Is.False);
		}

		[Test]
		public void Should_not_add_any_convention_and_return_ViolationHandlerConfiguration_with_predicate()
		{
			// Arrange
			Func<PolicyResult, bool> expectedPredicate = pr => pr.PolicyType == typeof(IgnorePolicy);
			var conventions = new List<IConvention>();
			var configuration = TestDataFactory.CreatedValidViolationConfiguration(conventions);

			// Act
			var result = configuration.Of(expectedPredicate);

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Predicate, Is.EqualTo(expectedPredicate));
			Assert.That(conventions.Any(), Is.False);
		}
	}
}