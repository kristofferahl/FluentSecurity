using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Policy;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using Moq;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("ConventionPolicyContainerSpec")]
	public class When_I_create_a_conventionpolicycontainer
	{
		[Test]
		public void Should_throw_ArgumentNullException_when_policycontainers_is_null()
		{
			Assert.Throws<ArgumentNullException>(() =>
				new ConventionPolicyContainer(null)
			);
		}

		[Test]
		public void Should_not_throw_when_policycontainers_is_empty()
		{
			Assert.DoesNotThrow(() =>
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
			var controllerName = NameHelper<AdminController>.Controller();
			var policyContainers = new List<IPolicyContainer>()
			{
				TestDataFactory.CreateValidPolicyContainer(controllerName, "Index"),
				TestDataFactory.CreateValidPolicyContainer(controllerName, "ListPosts"),
				TestDataFactory.CreateValidPolicyContainer(controllerName, "AddPost")
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

	[TestFixture]
	[Category("ConventionPolicyContainerSpec")]
	public class When_removing_a_policies_from_a_conventionpolicycontainer
	{
		[Test]
		public void Should_delegate_work_to_policycontainers()
		{
			// Arrange
			var policyContainer1 = new Mock<IPolicyContainer>();
			var policyContainer2 = new Mock<IPolicyContainer>();
			var policyContainer3 = new Mock<IPolicyContainer>();

			var policyContainers = new List<IPolicyContainer>()
			{
				policyContainer1.Object,
				policyContainer2.Object,
				policyContainer3.Object,
			};

			var conventionPolicyContainer = new ConventionPolicyContainer(policyContainers);

			// Act
			conventionPolicyContainer.RemovePolicy<SomePolicy>();

			// Assert
			policyContainer1.Verify(x => x.RemovePolicy(It.IsAny<Func<SomePolicy, bool>>()), Times.Once());
			policyContainer2.Verify(x => x.RemovePolicy(It.IsAny<Func<SomePolicy, bool>>()), Times.Once());
			policyContainer3.Verify(x => x.RemovePolicy(It.IsAny<Func<SomePolicy, bool>>()), Times.Once());
		}

		public class SomePolicy : ISecurityPolicy
		{
			public PolicyResult Enforce(ISecurityContext context)
			{
				throw new NotImplementedException();
			}
		}
	}
}