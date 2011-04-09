using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FluentSecurity.ServiceLocation;
using FluentSecurity.Specification.Helpers;
using Moq;
using NUnit.Framework;

namespace FluentSecurity.Specification.ServiceLocation
{
	[TestFixture]
	[Category("ServiceLocatorSpec")]
	public class When_creating_a_new_servicelocator
	{
		private ServiceLocator _serviceLocator;

		[SetUp]
		public void SetUp()
		{
			// Arrange
			SecurityConfigurator.Configure(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(() => true);
			});

			// Act
			_serviceLocator = new ServiceLocator();
		}

		[Test]
		public void Should_have_single_singleton_instance_of_ISecurityConfiguration()
		{
			// Assert
			Assert.That(_serviceLocator.Resolve<ISecurityConfiguration>(), Is.InstanceOf<SecurityConfiguration>());
			Assert.That(_serviceLocator.Resolve<ISecurityConfiguration>(), Is.EqualTo(_serviceLocator.Resolve<ISecurityConfiguration>()));
			Assert.That(_serviceLocator.ResolveAll<ISecurityConfiguration>().Single(), Is.EqualTo(_serviceLocator.Resolve<ISecurityConfiguration>()));
		}

		[Test]
		public void Should_have_single_transient_instance_of_ISecurityHandler()
		{
			// Assert
			Assert.That(_serviceLocator.Resolve<ISecurityHandler>(), Is.InstanceOf<SecurityHandler>());
			Assert.That(_serviceLocator.ResolveAll<ISecurityHandler>().Single(), Is.InstanceOf<SecurityHandler>());
			Assert.That(_serviceLocator.Resolve<ISecurityHandler>(), Is.Not.EqualTo(_serviceLocator.Resolve<ISecurityHandler>()));
		}

		[Test]
		public void Should_have_single_transient_instance_of_ISecurityContext()
		{
			// Assert
			Assert.That(_serviceLocator.Resolve<ISecurityContext>(), Is.InstanceOf<SecurityContext>());
			Assert.That(_serviceLocator.ResolveAll<ISecurityContext>().Single(), Is.InstanceOf<SecurityContext>());
			Assert.That(_serviceLocator.Resolve<ISecurityContext>(), Is.Not.EqualTo(_serviceLocator.Resolve<ISecurityContext>()));
		}

		[Test]
		public void Should_have_single_transient_instance_of_IPolicyViolationHandlerSelector()
		{
			// Assert
			Assert.That(_serviceLocator.Resolve<IPolicyViolationHandlerSelector>(), Is.InstanceOf<PolicyViolationHandlerSelector>());
			Assert.That(_serviceLocator.ResolveAll<IPolicyViolationHandlerSelector>().Single(), Is.InstanceOf<PolicyViolationHandlerSelector>());
			Assert.That(_serviceLocator.Resolve<IPolicyViolationHandlerSelector>(), Is.Not.EqualTo(_serviceLocator.Resolve<IPolicyViolationHandlerSelector>()));
		}

		[Test]
		public void Should_have_single_singleton_instance_of_IWhatDoIHaveBuilder()
		{
			// Assert
			Assert.That(_serviceLocator.Resolve<IWhatDoIHaveBuilder>(), Is.InstanceOf<DefaultWhatDoIHaveBuilder>());
			Assert.That(_serviceLocator.Resolve<IWhatDoIHaveBuilder>(), Is.EqualTo(_serviceLocator.Resolve<IWhatDoIHaveBuilder>()));
			Assert.That(_serviceLocator.ResolveAll<IWhatDoIHaveBuilder>().Single(), Is.EqualTo(_serviceLocator.Resolve<IWhatDoIHaveBuilder>()));
		}
	}

	[TestFixture]
	[Category("ServiceLocatorSpec")]
	public class When_resolving_an_instance_of_ISecurityContext
	{
		[Test]
		public void Should_throw_when_no_authentication_status_mechanism_has_been_provided()
		{
			// Arrange
			SecurityConfigurator.Configure(configuration => {});
			var serviceLocator = new ServiceLocator();

			// Act & assert
			Assert.Throws<ConfigurationErrorsException>(() => serviceLocator.Resolve<ISecurityContext>());
		}

		[Test]
		public void Should_not_throw_when_instance_is_registered_in_an_external_IoC_container()
		{
			// Arrange
			var expectedInstance = TestDataFactory.CreateSecurityContext(true);
			FakeIoC.Reset();
			FakeIoC.GetInstanceProvider = () => new List<object> { expectedInstance };
			SecurityConfigurator.Configure(configuration =>
			{
				configuration.ResolveServicesUsing(FakeIoC.GetAllInstances, FakeIoC.GetInstance);
			});
			var serviceLocator = new ServiceLocator();

			// Act
			var instance = serviceLocator.Resolve<ISecurityContext>();

			// Assert
			Assert.That(instance, Is.EqualTo(expectedInstance));
		}
	}
}