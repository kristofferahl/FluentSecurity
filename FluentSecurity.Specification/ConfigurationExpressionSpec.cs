using System;
using System.Configuration;
using System.Linq;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
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
	public class When_I_set_whatdoihavebuilder_to_null
	{
		[Test]
		public void Should_throw_ArgumentNullException()
		{
			// Arrange
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Assert
			Assert.Throws<ArgumentNullException>(() => configurationExpression.SetWhatDoIHaveBuilder(null));
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
	public class When_I_set_whatdoihavebuilder_to_instance_of_DefaultWhatDoIHaveBuilder
	{
		[Test]
		public void Should_have_whatdoihavebuilder_set_to_instance_of_DefaultWhatDoIHaveBuilder()
		{
			// Arrange
			var expectedWhatDoIHaveBuilder = new DefaultWhatDoIHaveBuilder();
			var configurationExpression = TestDataFactory.CreateValidConfigurationExpression();

			// Act
			configurationExpression.SetWhatDoIHaveBuilder(expectedWhatDoIHaveBuilder);

			// Assert
			Assert.That(configurationExpression.WhatDoIHaveBuilder, Is.EqualTo(expectedWhatDoIHaveBuilder));
		}
	}
}