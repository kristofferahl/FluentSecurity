using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using FluentSecurity.Configuration;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using Moq;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_creating_a_new_ConfigurationExpression
	{
		private static ConfigurationExpression Because()
		{
			return new ConfigurationExpression();
		}

		[Test]
		public void Should_not_contain_any_policycontainers()
		{
			// Act
			var configurationExpression = Because();

			// Assert
			var containers = configurationExpression.Count();
			Assert.That(containers, Is.EqualTo(0));
		}

		[Test]
		public void Should_have_PolicyAppender_set_to_DefaultPolicyAppender()
		{
			// Arrange
			var expectedPolicyAppenderType = typeof(DefaultPolicyAppender);

			// Act
			var configurationExpression = Because();

			// Assert
			var policyAppenderProperty = configurationExpression.GetType().GetProperty("PolicyAppender", BindingFlags.Instance | BindingFlags.NonPublic);
			var policyAppender = policyAppenderProperty.GetValue(configurationExpression, null);
			Assert.That(policyAppender, Is.TypeOf(expectedPolicyAppenderType));
		}

		[Test]
		public void Should_have_Advanced_set_to_AdvancedConfigurationExpression()
		{
			// Arrange
			var expectedType = typeof(AdvancedConfiguration);

			// Act
			var configurationExpression = Because();

			// Assert
			Assert.That(configurationExpression.Advanced, Is.TypeOf(expectedType));
			Assert.That(configurationExpression.Advanced, Is.Not.Null);
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_adding_a_policycontainter_for_Blog_Index
	{
		private ConfigurationExpression _configurationExpression;

		[SetUp]
		public void SetUp()
		{
			_configurationExpression = new ConfigurationExpression();
			_configurationExpression.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);
		}

		private void Because()
		{
			_configurationExpression.For<BlogController>(x => x.Index());
		}

		[Test]
		public void Should_have_policycontainer_for_Blog_Index()
		{
			// Act
			Because();

			// Assert
			var policyContainer = _configurationExpression.GetContainerFor(NameHelper.Controller<BlogController>(), "Index");
			
			Assert.That(policyContainer, Is.Not.Null);
			Assert.That(_configurationExpression.ToList().Count, Is.EqualTo(1));
		}

		[Test]
		public void Should_have_PolicyAppender_set_to_PolicyAppender()
		{
			// Act
			Because();

			// Assert
			var policyContainer = _configurationExpression.GetContainerFor(NameHelper.Controller<BlogController>(), "Index");
			Assert.That(policyContainer.PolicyAppender, Is.TypeOf(typeof(DefaultPolicyAppender)));
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_adding_a_policycontainter_for_Blog_Index_and_AddPost
	{
		[Test]
		public void Should_have_policycontainer_for_Blog_Index_and_AddPost()
		{
			// Arrange
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Act
			configurationExpression.For<BlogController>(x => x.Index());
			configurationExpression.For<BlogController>(x => x.AddPost());

			// Assert
			Assert.That(configurationExpression.GetContainerFor(NameHelper.Controller<BlogController>(), "Index"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor(NameHelper.Controller<BlogController>(), "AddPost"), Is.Not.Null);
			Assert.That(configurationExpression.ToList().Count, Is.EqualTo(2));
		}
	}

    [TestFixture]
    [Category("ConfigurationExpressionSpec")]
    public class When_adding_a_conventionpolicycontainer_for_the_Blog_controller_that_defines_additional_name_patterns
    {

        private ConfigurationExpression _configurationExpression;
        private string proxyNamingFormat = "{0}Proxy";


        [SetUp]
        public void SetUp()
        {
            // Arrange
            _configurationExpression = TestDataFactory.CreateValidConfigurationExpression();
        }

        private void Because()
        {
            _configurationExpression.For<BlogController>(proxyNamingFormat);
        }

        [Test]
        public void Should_have_policycontainers_for_all_actions()
        {
            // Arrange
            var expectedControllerName = NameHelper.Controller<BlogController>();
            var expectedControllerProxyName = string.Format(proxyNamingFormat, expectedControllerName);

            // Act
            Because();

            // Assert
            Assert.That(_configurationExpression.GetContainerFor(expectedControllerName, "Index"), Is.Not.Null);
            Assert.That(_configurationExpression.GetContainerFor(expectedControllerProxyName, "Index"), Is.Not.Null);
            Assert.That(_configurationExpression.GetContainerFor(expectedControllerName, "ListPosts"), Is.Not.Null);
            Assert.That(_configurationExpression.GetContainerFor(expectedControllerProxyName, "ListPosts"), Is.Not.Null);
            Assert.That(_configurationExpression.GetContainerFor(expectedControllerName, "AddPost"), Is.Not.Null);
            Assert.That(_configurationExpression.GetContainerFor(expectedControllerProxyName, "AddPost"), Is.Not.Null);
            Assert.That(_configurationExpression.GetContainerFor(expectedControllerName, "EditPost"), Is.Not.Null);
            Assert.That(_configurationExpression.GetContainerFor(expectedControllerProxyName, "EditPost"), Is.Not.Null);
            Assert.That(_configurationExpression.GetContainerFor(expectedControllerName, "DeletePost"), Is.Not.Null);
            Assert.That(_configurationExpression.GetContainerFor(expectedControllerProxyName, "DeletePost"), Is.Not.Null);
            Assert.That(_configurationExpression.GetContainerFor(expectedControllerName, "AjaxList"), Is.Not.Null);
            Assert.That(_configurationExpression.GetContainerFor(expectedControllerProxyName, "AjaxList"), Is.Not.Null);
        }

        [Test]
        public void Should_have_12_policycontainers()
        {
            // Act
            Because();

            // Assert
            Assert.That(_configurationExpression.ToList().Count, Is.EqualTo(12));
        }
    }

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_adding_a_conventionpolicycontainter_for_the_Blog_controller
	{
		private ConfigurationExpression _configurationExpression;

		[SetUp]
		public void SetUp()
		{
			// Arrange
			_configurationExpression = TestDataFactory.CreateValidConfigurationExpression();
		}

		private void Because()
		{
			_configurationExpression.For<BlogController>();
		}

		[Test]
		public void Should_have_policycontainers_for_all_actions()
		{
			// Arrange
			var expectedControllerName = NameHelper.Controller<BlogController>();

			// Act
			Because();

			// Assert
			Assert.That(_configurationExpression.GetContainerFor(expectedControllerName, "Index"), Is.Not.Null);
			Assert.That(_configurationExpression.GetContainerFor(expectedControllerName, "ListPosts"), Is.Not.Null);
			Assert.That(_configurationExpression.GetContainerFor(expectedControllerName, "AddPost"), Is.Not.Null);
			Assert.That(_configurationExpression.GetContainerFor(expectedControllerName, "EditPost"), Is.Not.Null);
			Assert.That(_configurationExpression.GetContainerFor(expectedControllerName, "DeletePost"), Is.Not.Null);
			Assert.That(_configurationExpression.GetContainerFor(expectedControllerName, "AjaxList"), Is.Not.Null);
		}

		[Test]
		public void Should_have_6_policycontainers()
		{
			// Act
			Because();

			// Assert
			Assert.That(_configurationExpression.ToList().Count, Is.EqualTo(6));
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_adding_a_conventionpolicycontainter_for_all_controllers_in_calling_assembly : AssemblyScannerAssemblySpecification
	{
		[Test]
		public void Should_have_policycontainers_for_all_controllers_and_all_actions()
		{
			// Act & assert
			Because(configurationExpression =>
				configurationExpression.ForAllControllers()
				);
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_adding_a_conventionpolicycontainter_for_all_controllers_in_specific_assembly : AssemblyScannerAssemblySpecification
	{
		[Test]
		public void Should_have_policycontainers_for_all_controllers_and_all_actions()
		{
			// Act & assert
			Because(configurationExpression =>
				configurationExpression.ForAllControllersInAssembly(GetType().Assembly)
				);
		}

		[Test]
		public void Should_throw_when_assembly_is_null()
		{
			// Act & assert
			Assert.Throws<ArgumentNullException>(() =>
				Because(configurationExpression =>
					configurationExpression.ForAllControllersInAssembly(null)
					)
				);
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_adding_a_conventionpolicycontainter_for_all_controllers_in_assembly_containing_type : AssemblyScannerAssemblySpecification
	{
		[Test]
		public void Should_have_policycontainers_for_all_controllers_and_all_actions()
		{
			// Act & assert
			Because(configurationExpression =>
				configurationExpression.ForAllControllersInAssemblyContainingType<SomeClass>()
				);
		}

		internal class SomeClass {}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_adding_a_conventionpolicycontainter_for_all_controllers_in_namespace_containing_type : AssemblyScannerNamespaceSpecification
	{
		[Test]
		public void Should_have_policycontainers_for_all_controllers_and_all_actions_in_namespace_of_ClassInRootNamespace()
		{
			// Arrange
			const string index = "Index";
			var root = NameHelper.Controller<TestData.AssemblyScannerControllers.RootController>();
			var include = NameHelper.Controller<TestData.AssemblyScannerControllers.Include.IncludedController>();
			var exclude = NameHelper.Controller<TestData.AssemblyScannerControllers.Exclude.ExcludedController>();

			// Act
			Because(configurationExpression =>
				configurationExpression.ForAllControllersInNamespaceContainingType<TestData.AssemblyScannerControllers.ClassInRootNamespace>()
				);

			// Assert
			Assert.That(PolicyContainers.Count(), Is.EqualTo(3));
			Assert.That(PolicyContainers.GetContainerFor(root, index), Is.Not.Null);
			Assert.That(PolicyContainers.GetContainerFor(include, index), Is.Not.Null);
			Assert.That(PolicyContainers.GetContainerFor(exclude, index), Is.Not.Null);
		}

		[Test]
		public void Should_have_policycontainers_for_all_controllers_and_all_actions_in_namespace_of_ClassInInvcludeNamespace()
		{
			// Arrange
			const string index = "Index";
			var root = NameHelper.Controller<TestData.AssemblyScannerControllers.RootController>();
			var include = NameHelper.Controller<TestData.AssemblyScannerControllers.Include.IncludedController>();
			var exclude = NameHelper.Controller<TestData.AssemblyScannerControllers.Exclude.ExcludedController>();

			// Act
			Because(configurationExpression =>
				configurationExpression.ForAllControllersInNamespaceContainingType<TestData.AssemblyScannerControllers.Include.ClassInIncludeNamespace>()
				);

			// Assert
			Assert.That(PolicyContainers.Count(), Is.EqualTo(1));
			Assert.That(PolicyContainers.GetContainerFor(include, index), Is.Not.Null);
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_I_pass_null_to_GetAuthenticationStatusFrom
	{
		[Test]
		public void Should_throw_ArgumentNullException()
		{
			// Arrange
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Assert
			Assert.Throws<ArgumentNullException>(() => configurationExpression.GetAuthenticationStatusFrom(null));
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_I_add_policies_before_specifying_a_function_returning_roles
	{
		[Test]
		public void Should_throw_ConfigurationErrorsException()
		{
			// Arrange
			var configurationExpression = new ConfigurationExpression();
			configurationExpression.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);
			configurationExpression.For<BlogController>(x => x.Index());

			// Assert
			Assert.Throws<ConfigurationErrorsException>(() => configurationExpression.GetRolesFrom(StaticHelper.GetRolesExcludingOwner));
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_I_pass_null_to_GetRolesFrom
	{
		[Test]
		public void Should_throw_ArgumentNullException()
		{
			// Arrange
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Assert
			Assert.Throws<ArgumentNullException>(() => configurationExpression.GetRolesFrom(null));
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_I_set_policyappender_to_null
	{
		[Test]
		public void Should_throw_ArgumentNullException()
		{
			// Arrange
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Assert
			Assert.Throws<ArgumentNullException>(() => configurationExpression.SetPolicyAppender(null));
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_I_set_servicelocator_to_null
	{
		[Test]
		public void Should_throw_ArgumentNullException()
		{
			// Arrange
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Assert
			Assert.Throws<ArgumentNullException>(() => configurationExpression.ResolveServicesUsing(null));
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_I_set_policyappender_to_instance_of_DefaultPolicyAppender
	{
		[Test]
		public void Should_have_policyappender_set_to_instance_of_DefaultPolicyAppender()
		{
			// Arrange
			var expectedPolicyAppender = new DefaultPolicyAppender();
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Act
			configurationExpression.SetPolicyAppender(expectedPolicyAppender);

			// Assert
			configurationExpression.For<BlogController>(x => x.Index());
			var policyContainer = configurationExpression.GetContainerFor(NameHelper.Controller<BlogController>(), "Index");
			Assert.That(policyContainer.PolicyAppender, Is.EqualTo(expectedPolicyAppender));
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_I_specify_a_default_policy_violation_handler
	{
		[Test]
		public void Should_clear_conflicting_conventions_and_add_convention_for_lazy_default_PolicyViolationHandler()
		{
			// Arrange
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();
			configurationExpression.Advanced.Conventions.Add(new FindDefaultPolicyViolationHandlerByNameConvention());
			configurationExpression.Advanced.Conventions.Add(new DefaultPolicyViolationHandlerIsInstanceConvention<AnyPolicyViolationHandler>(() => new AnyPolicyViolationHandler()));
			configurationExpression.Advanced.Conventions.Add(new DefaultPolicyViolationHandlerIsOfTypeConvention<AnyPolicyViolationHandler>());

			// Act
			configurationExpression.DefaultPolicyViolationHandlerIs<CustomDefaultPolicyViolationHandler>();

			// Assert
			var conventions = configurationExpression.Advanced.Conventions.OfType<IPolicyViolationHandlerConvention>().ToList();
			Assert.That(conventions.Count(), Is.EqualTo(2));
			Assert.That(conventions.First(), Is.TypeOf<FindByPolicyNameConvention>());
			Assert.That(conventions.Last(), Is.TypeOf<DefaultPolicyViolationHandlerIsOfTypeConvention<CustomDefaultPolicyViolationHandler>>());
		}

		[Test]
		public void Should_clear_conflicting_conventions_and_add_convention_for_lazy_default_PolicyViolationHandler_instance()
		{
			// Arrange
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();
			configurationExpression.Advanced.Conventions.Add(new FindDefaultPolicyViolationHandlerByNameConvention());
			configurationExpression.Advanced.Conventions.Add(new DefaultPolicyViolationHandlerIsInstanceConvention<AnyPolicyViolationHandler>(() => new AnyPolicyViolationHandler()));
			configurationExpression.Advanced.Conventions.Add(new DefaultPolicyViolationHandlerIsOfTypeConvention<AnyPolicyViolationHandler>());

			// Act
			configurationExpression.DefaultPolicyViolationHandlerIs(() => new CustomDefaultPolicyViolationHandler());

			// Assert
			var conventions = configurationExpression.Advanced.Conventions.OfType<IPolicyViolationHandlerConvention>().ToList();
			Assert.That(conventions.Count(), Is.EqualTo(2));
			Assert.That(conventions.First(), Is.TypeOf<FindByPolicyNameConvention>());
			Assert.That(conventions.Last(), Is.TypeOf<DefaultPolicyViolationHandlerIsInstanceConvention<CustomDefaultPolicyViolationHandler>>());
		}

		public class AnyPolicyViolationHandler : DefaultPolicyViolationHandler {}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_I_set_external_servicelocators : ConfigurationExpressionSpecification
	{
		[Test]
		public void Should_resolve_all_instances_from_services_locator()
		{
			// Arrange
			var concreteTypes = new List<ConcreteType1> { new ConcreteType1(), new ConcreteType1(), new ConcreteType1() };
			FakeIoC.GetAllInstancesProvider = () => concreteTypes;
			Func<Type, IEnumerable<object>> servicesLocator = FakeIoC.GetAllInstances;
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Act
			configurationExpression.ResolveServicesUsing(servicesLocator);

			// Assert
			var externalServiceLocator = GetExternalServiceLocator(configurationExpression);
			Assert.That(externalServiceLocator.ResolveAll(typeof(ConcreteType1)), Is.EqualTo(concreteTypes));
		}

		[Test]
		public void Should_resolve_single_instance_from_services_locator()
		{
			// Arrange
			var concreteTypes = new List<ConcreteType1> { new ConcreteType1(), new ConcreteType1(), new ConcreteType1() };
			FakeIoC.GetAllInstancesProvider = () => concreteTypes;
			Func<Type, IEnumerable<object>> servicesLocator = FakeIoC.GetAllInstances;
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Act
			configurationExpression.ResolveServicesUsing(servicesLocator);

			// Assert
			var externalServiceLocator = GetExternalServiceLocator(configurationExpression);
			Assert.That(externalServiceLocator.Resolve(typeof(ConcreteType1)), Is.EqualTo(concreteTypes.First()));
		}

		[Test]
		public void Should_resolve_all_instances_from_services_locator_and_resolve_single_instance_from_single_service_locator()
		{
			// Arrange
			var concreteTypesInServiceLocator1 = new List<object> { new ConcreteType1(), new ConcreteType1(), new ConcreteType1() };
			var concreteTypesInServiceLocator2 = new List<object> { new ConcreteType1(), new ConcreteType2() };

			FakeIoC.GetAllInstancesProvider = () => concreteTypesInServiceLocator1;
			FakeIoC.GetInstanceProvider = () => concreteTypesInServiceLocator2;

			Func<Type, IEnumerable<object>> servicesLocator = FakeIoC.GetAllInstances;
			Func<Type, object> singleServiceLocator = FakeIoC.GetInstance;

			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Act
			configurationExpression.ResolveServicesUsing(servicesLocator, singleServiceLocator);

			// Assert
			var externalServiceLocator = GetExternalServiceLocator(configurationExpression);
			Assert.That(externalServiceLocator.ResolveAll(typeof(ConcreteType1)), Is.EqualTo(concreteTypesInServiceLocator1));
			Assert.That(externalServiceLocator.Resolve(typeof(ConcreteType2)), Is.EqualTo(concreteTypesInServiceLocator2.Last()));
		}

		[Test]
		public void Should_resolve_everything_from_implementation_of_ISecurityServiceLocator()
		{
			// Arrange
			IEnumerable<object> expectedTypes = new List<object> { new ConcreteType1(), new ConcreteType1(), new ConcreteType1() };
			object expectedType = new ConcreteType2();

			var securityServiceLocator = new Mock<ISecurityServiceLocator>();
			securityServiceLocator.Setup(x => x.ResolveAll(typeof(ConcreteType1))).Returns(expectedTypes);
			securityServiceLocator.Setup(x => x.Resolve(typeof(ConcreteType2))).Returns(expectedType);

			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Act
			configurationExpression.ResolveServicesUsing(securityServiceLocator.Object);

			// Assert
			var externalServiceLocator = GetExternalServiceLocator(configurationExpression);
			Assert.That(externalServiceLocator.ResolveAll(typeof(ConcreteType1)), Is.EqualTo(expectedTypes));
			Assert.That(externalServiceLocator.Resolve(typeof(ConcreteType2)), Is.EqualTo(expectedType));
			securityServiceLocator.VerifyAll();
		}

		[Test]
		public void Should_throw_when_serviceslocator_is_null()
		{
			// Arrange
			Func<Type, IEnumerable<object>> servicesLocator = null;
			Func<Type, object> singleServiceLocator = FakeIoC.GetInstance;

			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Act & assert
			var exception = Assert.Throws<ArgumentNullException>(() => configurationExpression.ResolveServicesUsing(servicesLocator, singleServiceLocator));
			Assert.That(exception.ParamName, Is.EqualTo("servicesLocator"));
		}

		[Test]
		public void Should_throw_when_securityservicelocator_is_null()
		{
			// Arrange
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Act & assert
			var exception = Assert.Throws<ArgumentNullException>(() => configurationExpression.ResolveServicesUsing(null));
			Assert.That(exception.ParamName, Is.EqualTo("securityServiceLocator"));
		}

		private class ConcreteType1 { }
		private class ConcreteType2 { }
	}

	public abstract class ConfigurationExpressionSpecification
	{
		protected static ISecurityServiceLocator GetExternalServiceLocator(ConfigurationExpression configurationExpression)
		{
			var serviceLocatorProperty = configurationExpression.GetType().GetProperty("ExternalServiceLocator", BindingFlags.Instance | BindingFlags.NonPublic);
			var serviceLocator = serviceLocatorProperty.GetValue(configurationExpression, null);
			return (ISecurityServiceLocator) serviceLocator;
		}
	}
}