using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FluentSecurity.ServiceLocation;
using FluentSecurity.Specification.Helpers;
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

		[TearDown]
		public void TearDown()
		{
			HttpContextRequestDescription.HttpContextProvider = HttpContextRequestDescription.DefaultHttpContextProvider;
		}

		[Test]
		public void Should_have_single_singleton_instance_of_ISecurityConfiguration()
		{
			// Assert
			VerifyHasOneSingletonOf<ISecurityConfiguration, SecurityConfiguration>();
		}

		[Test]
		public void Should_have_single_transient_instance_of_ISecurityHandler()
		{
			// Assert
			VerifyHasOneTransientOf<ISecurityHandler, SecurityHandler>();
		}

		[Test]
		public void Should_have_single_transient_instance_of_ISecurityContext()
		{
			// Assert
			VerifyHasOneTransientOf<ISecurityContext, SecurityContext>();
		}

		[Test]
		public void Should_have_single_transient_instance_of_IPolicyViolationHandlerSelector()
		{
			// Assert
			VerifyHasOneTransientOf<IPolicyViolationHandlerSelector, PolicyViolationHandlerSelector>();
		}

		[Test]
		public void Should_have_single_singleton_instance_of_IWhatDoIHaveBuilder()
		{
			// Assert
			VerifyHasOneSingletonOf<IWhatDoIHaveBuilder, DefaultWhatDoIHaveBuilder>();
		}

		[Test]
		public void Should_have_single_singleton_instance_of_IRequestDescription()
		{
			// Arrange
			HttpContextRequestDescription.HttpContextProvider = () => MvcMockHelpers.FakeHttpContext();

			// Assert
			VerifyHasOneTransientOf<IRequestDescription, HttpContextRequestDescription>();
		}

		private void VerifyHasOneSingletonOf<TInterface, TDefaultInstance>()
		{
			Assert.That(_serviceLocator.Resolve<TInterface>(), Is.InstanceOf<TDefaultInstance>());
			Assert.That(_serviceLocator.Resolve<TInterface>(), Is.EqualTo(_serviceLocator.Resolve<TInterface>()));
			Assert.That(_serviceLocator.ResolveAll<TInterface>().Single(), Is.EqualTo(_serviceLocator.Resolve<TInterface>()));
		}

		private void VerifyHasOneTransientOf<TInterface, TDefaultInstance>()
		{
			Assert.That(_serviceLocator.Resolve<TInterface>(), Is.InstanceOf<TDefaultInstance>());
			Assert.That(_serviceLocator.ResolveAll<TInterface>().Single(), Is.InstanceOf<TDefaultInstance>());
			Assert.That(_serviceLocator.Resolve<TInterface>(), Is.Not.EqualTo(_serviceLocator.Resolve<TInterface>()));
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