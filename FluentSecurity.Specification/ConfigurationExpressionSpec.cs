using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
			var builder = Because();

			// Assert
			Assert.That(builder.PolicyAppender, Is.TypeOf(expectedPolicyAppenderType));
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
			var policyContainer = _configurationExpression.GetContainerFor("Blog", "Index");
			
			Assert.That(policyContainer, Is.Not.Null);
			Assert.That(_configurationExpression.ToList().Count, Is.EqualTo(1));
		}

		[Test]
		public void Should_have_PolicyAppender_set_to_PolicyAppender()
		{
			// Act
			Because();

			// Assert
			var policyContainer = _configurationExpression.GetContainerFor("Blog", "Index");
			Assert.That(policyContainer.PolicyAppender, Is.EqualTo(_configurationExpression.PolicyAppender));
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
			Assert.That(configurationExpression.GetContainerFor("Blog", "Index"), Is.Not.Null);
			Assert.That(configurationExpression.GetContainerFor("Blog", "AddPost"), Is.Not.Null);
			Assert.That(configurationExpression.ToList().Count, Is.EqualTo(2));
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
			_configurationExpression.For<BlogController>().DenyAnonymousAccess();
		}

		[Test]
		public void Should_have_policycontainers_for_all_actions()
		{
			// Arrange
			const string expectedControllerName = "Blog";

			// Act
			Because();

			// Assert
			Assert.That(_configurationExpression.GetContainerFor(expectedControllerName, "Index"), Is.Not.Null);
			Assert.That(_configurationExpression.GetContainerFor(expectedControllerName, "ListPosts"), Is.Not.Null);
			Assert.That(_configurationExpression.GetContainerFor(expectedControllerName, "AddPost"), Is.Not.Null);
			Assert.That(_configurationExpression.GetContainerFor(expectedControllerName, "EditPost"), Is.Not.Null);
			Assert.That(_configurationExpression.GetContainerFor(expectedControllerName, "DeletePost"), Is.Not.Null);
		}

		[Test]
		public void Should_have_5_policycontainers()
		{
			// Act
			Because();

			// Assert
			Assert.That(_configurationExpression.ToList().Count, Is.EqualTo(5));
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_removing_policies_for_Blog_AddPost
	{
		private ConfigurationExpression _configurationExpression;
		private IPolicyContainer _addPostPolicyContainer;

		[SetUp]
		public void SetUp()
		{
			// Arrange
			_configurationExpression = TestDataFactory.CreateValidConfigurationExpression();
			_configurationExpression.For<BlogController>(x => x.Index());
			_configurationExpression.For<BlogController>(x => x.AddPost());

			_addPostPolicyContainer = _configurationExpression.GetContainerFor("Blog", "AddPost");

			// Act
			_configurationExpression.RemovePoliciesFor<BlogController>(x => x.AddPost());
		}

		[Test]
		public void Should_have_1_policycontainer()
		{
			// Assert
			Assert.That(_configurationExpression.ToList().Count, Is.EqualTo(1));
		}

		[Test]
		public void Should_have_policycontainer_for_Blog_Index()
		{
			// Assert
			Assert.That(_configurationExpression.GetContainerFor("Blog", "Index"), Is.Not.Null);
		}

		[Test]
		public void Should_not_have_policycontainer_for_Blog_AddPost()
		{
			// Assert
			Assert.That(_configurationExpression.Contains(_addPostPolicyContainer), Is.False);
		}

		[Test]
		public void Shoud_return_null_when_getting_a_policycontainer_for_Blog_AddPost()
		{
			// Assert
			Assert.That(_configurationExpression.GetContainerFor("Blog", "AddPost"), Is.Null);
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
	public class When_I_add_policies_before_specifying_a_function_returning_authenticationstatus
	{
		[Test]
		public void Should_throw_ConfigurationErrorsException()
		{
			// Arrange
			var configurationExpression = new ConfigurationExpression();
			
			// Assert
			Assert.Throws<ConfigurationErrorsException>(() =>
				configurationExpression.For<BlogController>(x => x.Index())
			);
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
			Assert.That(configurationExpression.PolicyAppender, Is.EqualTo(expectedPolicyAppender));
		}
	}

	[TestFixture]
	[Category("ConfigurationExpressionSpec")]
	public class When_I_set_external_servicelocators
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
			Assert.That(configurationExpression.ExternalServiceLocator.ResolveAll(typeof(ConcreteType1)), Is.EqualTo(concreteTypes));
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
			Assert.That(configurationExpression.ExternalServiceLocator.Resolve(typeof(ConcreteType1)), Is.EqualTo(concreteTypes.First()));
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
			Assert.That(configurationExpression.ExternalServiceLocator.ResolveAll(typeof(ConcreteType1)), Is.EqualTo(concreteTypesInServiceLocator1));
			Assert.That(configurationExpression.ExternalServiceLocator.Resolve(typeof(ConcreteType2)), Is.EqualTo(concreteTypesInServiceLocator2.Last()));
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
			Assert.That(configurationExpression.ExternalServiceLocator.ResolveAll(typeof(ConcreteType1)), Is.EqualTo(expectedTypes));
			Assert.That(configurationExpression.ExternalServiceLocator.Resolve(typeof(ConcreteType2)), Is.EqualTo(expectedType));
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
			Func<Type, IEnumerable<object>> servicesLocator = null;
			Func<Type, object> singleServiceLocator = FakeIoC.GetInstance;

			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Act & assert
			var exception = Assert.Throws<ArgumentNullException>(() => configurationExpression.ResolveServicesUsing(null));
			Assert.That(exception.ParamName, Is.EqualTo("securityServiceLocator"));
		}

		private class ConcreteType1 {}
		private class ConcreteType2 {}
	}
}