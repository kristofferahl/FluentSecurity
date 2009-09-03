using System;
using System.Linq;
using FluentSecurity.Specification.Fakes;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("PolicyBuilderSpec")]
	public class When_adding_a_policy_for_Blog_Index
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
			Assert.That(builder.GetContainerFor("Blog", "Index"), Is.Not.Null);
			Assert.That(builder.ToList().Count, Is.EqualTo(1));
		}
	}

	[TestFixture]
	[Category("PolicyBuilderSpec")]
	public class When_adding_a_policy_for_Blog_Index_and_AddPost
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
	public class When_removing_a_policy_for_Blog_AddPost
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
		[Test]
		public void Should_not_cotain_any_policycontainers()
		{
			// Arrange
			var builder = new PolicyBuilder();

			// Act
			var containers = builder.Count();

			// Assert
			Assert.That(containers, Is.EqualTo(0));
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
}