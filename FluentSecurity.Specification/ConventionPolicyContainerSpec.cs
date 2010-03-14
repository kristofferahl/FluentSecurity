using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Policy;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("ConventionPolicyContainerSpec")]
	public class When_I_create_a_conventionpolicycontainer
	{
		[Test]
		public void Should_throw_ArgumentException_when_policycontainers_is_null()
		{
			Assert.Throws<ArgumentException>(() =>
				new ConventionPolicyContainer(null)
			);
		}

		[Test]
		public void Should_throw_ArgumentException_when_policycontainers_is_empty()
		{
			Assert.Throws<ArgumentException>(() =>
				new ConventionPolicyContainer(new List<IPolicyContainer>())
			);
		}
	}

	[TestFixture]
	[Category("ConventionPolicyContainerSpec")]
	public class When_adding_a_policy_to_a_conventionpolicycontainer
	{
		[Test]
		public void Should_add_policy_to_policycontainers()
		{
			// Arrange
			var policyContainers = new List<IPolicyContainer>()
				{
					TestDataFactory.CreateValidPolicyContainer("Admin", "Index"),
					TestDataFactory.CreateValidPolicyContainer("Admin", "ListPosts"),
					TestDataFactory.CreateValidPolicyContainer("Admin", "AddPost")
				};

			var conventionPolicyContainer = new ConventionPolicyContainer(policyContainers);
			var policy = new DenyAnonymousAccessPolicy();

			// Act
			conventionPolicyContainer.AddPolicy(policy);

			// Assert
			Assert.That(policyContainers[0].GetPolicies().First(), Is.EqualTo(policy));
			Assert.That(policyContainers[1].GetPolicies().First(), Is.EqualTo(policy));
			Assert.That(policyContainers[2].GetPolicies().First(), Is.EqualTo(policy));
		}
	}
}