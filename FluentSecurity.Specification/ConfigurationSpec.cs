using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FluentSecurity.Policy;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("ConfigurationSpec")]
	public class When_I_configure_security_for_Blog_Index_and_Blog_AddPost
 	{
		private IEnumerable<IPolicyContainer> _policyContainers;
		const string ControllerName = "Blog";
		const string IndexActionName = "Index";
		const string AddPostActionName = "AddPost";

		[SetUp]
		public void SetUp()
		{
			// Act
			Configuration.Configure(policy =>
			{
				policy.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);
				policy.GetRolesFrom(StaticHelper.GetRolesExcludingOwner);
				policy.For<BlogController>(x => x.Index()).DenyAnonymousAccess();
				policy.For<BlogController>(x => x.AddPost()).RequireRole(UserRole.Writer, UserRole.Publisher, UserRole.Owner);
			});

			_policyContainers = Configuration.GetPolicyContainers();
		}

		[Test]
		public void Should_have_two_policycontainers()
		{
			Assert.That(_policyContainers.Count(), Is.EqualTo(2));
		}

		[Test]
		public void Should_have_policycontainer_for_Blog_Index()
		{
			var container = _policyContainers.GetContainerFor(ControllerName, IndexActionName);
			Assert.That(container.ControllerName, Is.EqualTo(ControllerName));
			Assert.That(container.ActionName, Is.EqualTo(IndexActionName));
			Assert.That(container.GetPolicies().Count(), Is.EqualTo(1));
			Assert.That(container.GetPolicies().First().GetType(), Is.EqualTo(typeof(DenyAnonymousAccessPolicy)));
		}

		[Test]
		public void Should_have_policycontainer_for_Blog_AddPost()
		{
			var container = _policyContainers.GetContainerFor(ControllerName, AddPostActionName);
			Assert.That(container.ControllerName, Is.EqualTo(ControllerName));
			Assert.That(container.ActionName, Is.EqualTo(AddPostActionName));
			Assert.That(container.GetPolicies().Count(), Is.EqualTo(1));
			Assert.That(container.GetPolicies().First().GetType(), Is.EqualTo(typeof(RequireRolePolicy)));
		}
 	}

	[TestFixture]
	[Category("ConfigurationSpec")]
	public class When_adding_two_containers_with_the_same_controller_and_action_name
	{
		[Test]
		public void Should_have_1_policycontainer()
		{
			// Act
			Configuration.Configure(policy =>
			{
				policy.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);
				policy.GetRolesFrom(StaticHelper.GetRolesExcludingOwner);
				policy.For<BlogController>(x => x.Index());
				policy.For<BlogController>(x => x.Index());
			});

			Assert.That(Configuration.GetPolicyContainers().Count(), Is.EqualTo(1));
			Assert.That(Configuration.GetPolicyContainers().First().ControllerName, Is.EqualTo("Blog"));
			Assert.That(Configuration.GetPolicyContainers().First().ActionName, Is.EqualTo("Index"));
		}
	}

	[TestFixture]
	[Category("ConfigurationSpec")]
	public class When_I_call_GetPolicyContainers_before_Configure
	{
		[Test]
		public void Should_throw_InvalidOperationException()
		{
			Configuration.Reset();

			// Assert
			Assert.Throws<InvalidOperationException>(() => Configuration.GetPolicyContainers());
		}
	}

	[TestFixture]
	[Category("PolicyBuilderSpec")]
	public class When_I_add_policies_before_specifying_a_function_returning_authenticationstatus
	{
		[Test]
		public void Should_throw_ConfigurationErrorsException()
		{
			// Arrange
			Configuration.Reset();

			// Assert
			Assert.Throws<ConfigurationErrorsException>(() =>
			Configuration.Configure(policy =>
			{
				policy.For<BlogController>(x => x.Index());
			}));
		}
	}

	[TestFixture]
	[Category("PolicyBuilderSpec")]
	public class When_I_add_policies_before_specifying_a_function_returning_roles
	{
		[Test]
		public void Should_throw_ConfigurationErrorsException()
		{
			// Arrange
			Configuration.Reset();

			// Assert
			Assert.Throws<ConfigurationErrorsException>(() =>
			Configuration.Configure(policy =>
			{
				policy.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);
				policy.For<BlogController>(x => x.Index());
				policy.GetRolesFrom(StaticHelper.GetRolesExcludingOwner);
			}));
		}
	}

	[TestFixture]
	[Category("ConfigurationSpec")]
	public class When_I_remove_policies_for_Blog_Index
	{
		private IEnumerable<IPolicyContainer> _policyContainers;
		const string ControllerName = "Blog";
		const string IndexActionName = "Index";

		[SetUp]
		public void SetUp()
		{
			// Act
			Configuration.Configure(policy =>
			{
				policy.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);
				policy.For<BlogController>(x => x.Index());
				policy.For<BlogController>(x => x.AddPost());
				policy.RemovePoliciesFor<BlogController>(x => x.Index());
			});

			_policyContainers = Configuration.GetPolicyContainers();
		}

		[Test]
		public void Should_have_1_policycontainer()
		{
			// Assert
			Assert.That(_policyContainers.Count(), Is.EqualTo(1));
		}

		[Test]
		public void Should_not_have_policycontainer_for_Blog_Index()
		{
			// Assert
			var container = _policyContainers.GetContainerFor(ControllerName, IndexActionName);
			Assert.That(container, Is.Null);
		}
	}

	[TestFixture]
	[Category("ConfigurationSpec")]
	public class When_I_configure_security
	{
		[Test]
		public void Should_set_IgnoreMissingConfiguration_to_false()
		{
			// Act
			Configuration.Configure(policy =>
			{
				policy.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);
				policy.For<BlogController>(x => x.Index());
			});

			// Assert
			Assert.That(Configuration.IgnoreMissingConfiguration, Is.False);
		}

		[Test]
		public void Should_set_IgnoreMissingConfiguration_to_true()
		{
			// Act
			Configuration.Configure(policy =>
			{
				policy.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);
				policy.IgnoreMissingConfiguration();
				policy.For<BlogController>(x => x.Index());
			});

			// Assert
			Assert.That(Configuration.IgnoreMissingConfiguration, Is.True);
		}
	}

	[TestFixture]
	[Category("ConfigurationSpec")]
	public class When_I_check_what_I_have
	{
		[Test]
		public void Should_return_the_current_configuration()
		{
			// Arrange
			Configuration.Configure(policy =>
			{
				policy.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);
				policy.IgnoreMissingConfiguration();
				policy.For<BlogController>(x => x.DeletePost()).DenyAnonymousAccess().RequireRole(UserRole.Owner, UserRole.Publisher);
				policy.For<BlogController>(x => x.Index()).DenyAnonymousAccess();
			});

			// Act
			var whatIHave = Configuration.WhatDoIHave();

			// Assert
			Assert.That(whatIHave.Replace("\r\n", "|").Replace("\t","%"), Is.EqualTo("Ignore missing configuration: True||------------------------------------------------------------------------------------|BlogController > DeletePost|%RequireRole (Owner or Publisher)|BlogController > Index|%DenyAnonymousAccess|------------------------------------------------------------------------------------"));
		}
	}
}