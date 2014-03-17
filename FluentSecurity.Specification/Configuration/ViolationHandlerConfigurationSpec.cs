using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Configuration;
using FluentSecurity.Core;
using FluentSecurity.Policy;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification.Configuration
{
	[TestFixture]
	[Category("ViolationHandlerConfigurationSpec")]
	public class When_creating_a_ViolationHandlerConfiguration
	{
		private readonly ViolationConfiguration _validConfiguration = TestDataFactory.CreatedValidViolationConfiguration();
		private readonly Func<PolicyResult, bool> _validPredicate = x => true;

		[Test]
		public void Should_throw_when_configuration_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new ViolationHandlerConfiguration(null, _validPredicate));
		}

		[Test]
		public void Should_throw_when_predicate_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new ViolationHandlerConfiguration(_validConfiguration, null));
		}

		[Test]
		public void Should_not_throw_when_ViolationConfiguration_is_not_null()
		{
			Assert.DoesNotThrow(() => new ViolationHandlerConfiguration(_validConfiguration, _validPredicate));
		}
	}

	[TestFixture]
	[Category("ViolationHandlerConfigurationSpec")]
	public class When_creating_a_ViolationHandlerConfiguration_of_T
	{
		private readonly ViolationConfiguration _validConfiguration = TestDataFactory.CreatedValidViolationConfiguration();

		[Test]
		public void Should_throw_when_configuration_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new ViolationHandlerConfiguration<IgnorePolicy>(null));
		}

		[Test]
		public void Should_not_throw_when_configuration_is_not_null()
		{
			Assert.DoesNotThrow(() => new ViolationHandlerConfiguration<IgnorePolicy>(_validConfiguration));
		}
	}

	[TestFixture]
	[Category("ViolationHandlerConfigurationSpec")]
	public class When_specifying_a_handler_factory_for_a_ViolationHandlerConfiguration
	{
		[Test]
		public void Should_throw_when_factory_method_is_null()
		{
			// Arrange
			var violationConfiguration = TestDataFactory.CreatedValidViolationConfiguration();
			var configuration = new ViolationHandlerConfiguration(violationConfiguration, x => true);

			// Act & assert
			Assert.Throws<ArgumentNullException>(() => configuration.IsHandledBy<DefaultPolicyViolationHandler>(null));
		}

		[Test]
		public void Should_add_convention_for_predicate_to_type()
		{
			// Arrange
			Func<PolicyResult, bool> expectedPredicate = x => true;
			var conventions = new List<IConvention>();
			var violationConfiguration  = TestDataFactory.CreatedValidViolationConfiguration(conventions);
			var configuration = new ViolationHandlerConfiguration(violationConfiguration, expectedPredicate);

			// Act
			configuration.IsHandledBy<DefaultPolicyViolationHandler>();

			// Assert
			Assert.That(conventions.Single().As<PredicateToPolicyViolationHandlerTypeConvention<DefaultPolicyViolationHandler>>().Predicate, Is.EqualTo(expectedPredicate));
		}

		[Test]
		public void Should_add_convention_for_predicate_to_instance()
		{
			// Arrange
			Func<PolicyResult, bool> expectedPredicate = x => true;
			var conventions = new List<IConvention>();
			var violationConfiguration = TestDataFactory.CreatedValidViolationConfiguration(conventions);
			var configuration = new ViolationHandlerConfiguration(violationConfiguration, expectedPredicate);

			// Act
			configuration.IsHandledBy(() => new DefaultPolicyViolationHandler());

			// Assert
			Assert.That(conventions.Single().As<PredicateToPolicyViolationHandlerInstanceConvention<DefaultPolicyViolationHandler>>().Predicate, Is.EqualTo(expectedPredicate));
		}
	}

	[TestFixture]
	[Category("ViolationHandlerConfigurationSpec")]
	public class When_specifying_a_handler_factory_for_a_ViolationHandlerConfiguration_of_T
	{
		[Test]
		public void Should_throw_when_factory_method_is_null()
		{
			// Arrange
			var violationConfiguration = TestDataFactory.CreatedValidViolationConfiguration();
			var configuration = new ViolationHandlerConfiguration<IgnorePolicy>(violationConfiguration);

			// Act & assert
			Assert.Throws<ArgumentNullException>(() => configuration.IsHandledBy<DefaultPolicyViolationHandler>(null));
		}

		[Test]
		public void Should_add_convention_for_predicate_to_type()
		{
			// Arrange
			var conventions = new List<IConvention>();
			var violationConfiguration = TestDataFactory.CreatedValidViolationConfiguration(conventions);
			var configuration = new ViolationHandlerConfiguration<IgnorePolicy>(violationConfiguration);

			// Act
			configuration.IsHandledBy<DefaultPolicyViolationHandler>();

			// Assert
			Assert.That(conventions.Single(), Is.InstanceOf<PolicyTypeToPolicyViolationHandlerTypeConvention<IgnorePolicy, DefaultPolicyViolationHandler>>());
		}

		[Test]
		public void Should_add_convention_for_predicate_to_instance()
		{
			// Arrange
			var conventions = new List<IConvention>();
			var violationConfiguration = TestDataFactory.CreatedValidViolationConfiguration(conventions);
			var configuration = new ViolationHandlerConfiguration<IgnorePolicy>(violationConfiguration);

			// Act
			configuration.IsHandledBy(() => new DefaultPolicyViolationHandler());

			// Assert
			Assert.That(conventions.Single(), Is.InstanceOf<PolicyTypeToPolicyViolationHandlerInstanceConvention<IgnorePolicy, DefaultPolicyViolationHandler>>());
		}
	}
}