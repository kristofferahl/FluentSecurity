using System;
using System.Linq;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("PolicyBuilderSpec")]
	public class When_adding_a_policycontainter_for_Blog_Index
	{
		[Test]
		public void Should_have_policycontainer_for_Blog_Index()
		{
			// Arrange
			var builder = new PolicyBuilder();
			builder.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);

			// Act
			builder.For<BlogController>(x => x.Index());

			// Assert
			var policyContainer = builder.GetContainerFor("Blog", "Index");
			Assert.That(policyContainer, Is.Not.Null);
			Assert.That(builder.ToList().Count, Is.EqualTo(1));
		}

		[Test]
		public void Should_have_PolicyManager_set_to_PolicyManager()
		{
			// Arrange
			var builder = TestDataFactory.CreateValidPolicyBuilder();

			// Act
			builder.For<BlogController>(x => x.Index());

			// Assert
			var policyContainer = builder.GetContainerFor("Blog", "Index");
			Assert.That(policyContainer.Manager, Is.EqualTo(builder.PolicyManager));
		}
	}

	[TestFixture]
	[Category("PolicyBuilderSpec")]
	public class When_adding_a_policycontainter_for_Blog_Index_and_AddPost
	{
		[Test]
		public void Should_have_policycontainer_for_Blog_Index_and_AddPost()
		{
			// Arrange
			var builder = new PolicyBuilder();
			builder.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);

			// Act
			builder.For<BlogController>(x => x.Index());
			builder.For<BlogController>(x => x.AddPost());

			// Assert
			Assert.That(builder.GetContainerFor("Blog", "Index"), Is.Not.Null);
			Assert.That(builder.GetContainerFor("Blog", "AddPost"), Is.Not.Null);
			Assert.That(builder.ToList().Count, Is.EqualTo(2));
		}
	}

	[TestFixture]
	[Category("PolicyBuilderSpec")]
	public class When_adding_a_conventionpolicycontainter_for_the_Blog_controller
	{
		private PolicyBuilder _builder;

		[SetUp]
		public void SetUp()
		{
			// Arrange
			_builder = new PolicyBuilder();
			_builder.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);
		}

		private void Because()
		{
			_builder.For<BlogController>().DenyAnonymousAccess();
		}

		[Test]
		public void Should_have_policycontainers_for_all_actions()
		{
			// Arrange
			const string expectedControllerName = "Blog";

			// Act
			Because();

			// Assert
			Assert.That(_builder.GetContainerFor(expectedControllerName, "Index"), Is.Not.Null);
			Assert.That(_builder.GetContainerFor(expectedControllerName, "ListPosts"), Is.Not.Null);
			Assert.That(_builder.GetContainerFor(expectedControllerName, "AddPost"), Is.Not.Null);
			Assert.That(_builder.GetContainerFor(expectedControllerName, "EditPost"), Is.Not.Null);
			Assert.That(_builder.GetContainerFor(expectedControllerName, "DeletePost"), Is.Not.Null);
		}

		[Test]
		public void Should_have_5_policycontainers()
		{
			// Act
			Because();

			// Assert
			Assert.That(_builder.ToList().Count, Is.EqualTo(5));
		}
	}

	[TestFixture]
	[Category("PolicyBuilderSpec")]
	public class When_removing_policies_for_Blog_AddPost
	{
		private PolicyBuilder _builder;
		private IPolicyContainer _addPostPolicyContainer;

		[SetUp]
		public void SetUp()
		{
			// Arrange
			_builder = new PolicyBuilder();
			_builder.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);
			_builder.For<BlogController>(x => x.Index());
			_builder.For<BlogController>(x => x.AddPost());

			_addPostPolicyContainer = _builder.GetContainerFor("Blog", "AddPost");

			// Act
			_builder.RemovePoliciesFor<BlogController>(x => x.AddPost());
		}

		[Test]
		public void Should_have_1_policycontainer()
		{
			// Assert
			Assert.That(_builder.ToList().Count, Is.EqualTo(1));
		}

		[Test]
		public void Should_have_policycontainer_for_Blog_Index()
		{
			// Assert
			Assert.That(_builder.GetContainerFor("Blog", "Index"), Is.Not.Null);
		}

		[Test]
		public void Should_not_have_policycontainer_for_Blog_AddPost()
		{
			// Assert
			Assert.That(_builder.Contains(_addPostPolicyContainer), Is.False);
		}

		[Test]
		public void Shoud_return_null_when_getting_a_policycontainer_for_Blog_AddPost()
		{
			// Assert
			Assert.That(_builder.GetContainerFor("Blog", "AddPost"), Is.Null);
		}
	}

	[TestFixture]
	[Category("PolicyBuilderSpec")]
	public class When_creating_a_new_PolicyBuilder
	{
		private static PolicyBuilder Because()
		{
			return new PolicyBuilder();
		}

		[Test]
		public void Should_not_contain_any_policycontainers()
		{
			// Act
			var builder = Because();

			// Assert
			var containers = builder.Count();
			Assert.That(containers, Is.EqualTo(0));
		}

		[Test]
		public void Should_have_PolicyManager_set_to_DefaultPolicyManager()
		{
			// Arrange
			var expectedPolicyManagerType = typeof(DefaultPolicyManager);

			// Act
			var builder = Because();

			// Assert
			Assert.That(builder.PolicyManager, Is.TypeOf(expectedPolicyManagerType));
		}
	}

	[TestFixture]
	[Category("PolicyBuilderSpec")]
	public class When_I_pass_null_to_GetAuthenticationStatusFrom
	{
		[Test]
		public void Should_throw_ArgumentNullException()
		{
			// Arrange
			var builder = new PolicyBuilder();

			// Assert
			Assert.Throws<ArgumentNullException>(() => builder.GetAuthenticationStatusFrom(null));
		}
	}

	[TestFixture]
	[Category("PolicyBuilderSpec")]
	public class When_I_pass_null_to_GetRolesFrom
	{
		[Test]
		public void Should_throw_ArgumentNullException()
		{
			// Arrange
			var builder = new PolicyBuilder();

			// Assert
			Assert.Throws<ArgumentNullException>(() => builder.GetRolesFrom(null));
		}
	}

	[TestFixture]
	[Category("PolicyBuilderSpec")]
	public class When_I_set_policy_manager_to_null
	{
		[Test]
		public void Should_throw_ArgumentNullException()
		{
			// Arrange
			var builder = new PolicyBuilder();

			// Assert
			Assert.Throws<ArgumentNullException>(() => builder.SetCurrentPolicyManager(null));
		}
	}

	[TestFixture]
	[Category("PolicyBuilderSpec")]
	public class When_I_set_policy_manager_to_instance_of_DefaultPolicyManager
	{
		[Test]
		public void Should_have_policy_manager_set_to_instance_of_DefaultPolicyManager()
		{
			// Arrange
			var expectedPolicyManager = new DefaultPolicyManager();
			var builder = new PolicyBuilder();

			// Act
			builder.SetCurrentPolicyManager(expectedPolicyManager);

			// Assert
			Assert.That(builder.PolicyManager, Is.EqualTo(expectedPolicyManager));
		}
	}
}