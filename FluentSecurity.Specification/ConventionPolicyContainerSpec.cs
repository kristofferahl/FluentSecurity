using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Caching;
using FluentSecurity.Configuration;
using FluentSecurity.Policy;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData.Controllers;
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
				new ConventionPolicyContainer(new List<IPolicyContainerConfiguration>())
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
			var controllerName = NameHelper.Controller<AdminController>();
			var policyContainers = new List<PolicyContainer>
			{
				TestDataFactory.CreateValidPolicyContainer(controllerName, "Index"),
				TestDataFactory.CreateValidPolicyContainer(controllerName, "ListPosts"),
				TestDataFactory.CreateValidPolicyContainer(controllerName, "AddPost")
			};

			var conventionPolicyContainer = new ConventionPolicyContainer(policyContainers.Cast<IPolicyContainerConfiguration>().ToList());
			var policy = new DenyAnonymousAccessPolicy();

			// Act
			conventionPolicyContainer.AddPolicy(policy);

			// Assert
			Assert.That(policyContainers[0].GetPolicies().First(), Is.EqualTo(policy));
			Assert.That(policyContainers[1].GetPolicies().First(), Is.EqualTo(policy));
			Assert.That(policyContainers[2].GetPolicies().First(), Is.EqualTo(policy));
		}

		[Test]
		public void Should_add_lazy_policy_to_policycontainers()
		{
			// Arrange
			var controllerName = NameHelper.Controller<AdminController>();
			var policyContainers = new List<PolicyContainer>
			{
				TestDataFactory.CreateValidPolicyContainer(controllerName, "Index"),
				TestDataFactory.CreateValidPolicyContainer(controllerName, "ListPosts"),
				TestDataFactory.CreateValidPolicyContainer(controllerName, "AddPost")
			};

			var conventionPolicyContainer = new ConventionPolicyContainer(policyContainers.Cast<IPolicyContainerConfiguration>().ToList());
			conventionPolicyContainer.SetTypeFactory(new MvcTypeFactory());

			// Act
			conventionPolicyContainer.AddPolicy<DenyAnonymousAccessPolicy>();

			// Assert
			Assert.That(policyContainers[0].GetPolicies().First(), Is.TypeOf<LazySecurityPolicy<DenyAnonymousAccessPolicy>>());
			Assert.That(policyContainers[1].GetPolicies().First(), Is.TypeOf<LazySecurityPolicy<DenyAnonymousAccessPolicy>>());
			Assert.That(policyContainers[2].GetPolicies().First(), Is.TypeOf<LazySecurityPolicy<DenyAnonymousAccessPolicy>>());
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
			var policyContainer1 = new Mock<IPolicyContainerConfiguration>();
			var policyContainer2 = new Mock<IPolicyContainerConfiguration>();
			var policyContainer3 = new Mock<IPolicyContainerConfiguration>();

			var policyContainers = new List<IPolicyContainerConfiguration>
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

	[TestFixture]
	[Category("ConventionPolicyContainerSpec")]
	public class When_setting_the_cache_lifecycle_for_a_policy_on_a_conventionpolicycontainer
	{
		[Test]
		public void Should_add_policyresult_cache_strategy_to_policycontainers()
		{
			// Arrange
			var controllerName = NameHelper.Controller<AdminController>();
			var policyContainers = new List<PolicyContainer>
			{
				TestDataFactory.CreateValidPolicyContainer(controllerName, "Index"),
				TestDataFactory.CreateValidPolicyContainer(controllerName, "ListPosts"),
				TestDataFactory.CreateValidPolicyContainer(controllerName, "AddPost")
			};

			var conventionPolicyContainer = new ConventionPolicyContainer(policyContainers.Cast<IPolicyContainerConfiguration>().ToList(), By.Controller);
			const Cache expectedLifecycle = Cache.PerHttpRequest;
			var expectedType = typeof(DenyAnonymousAccessPolicy);

			// Act
			conventionPolicyContainer.Cache<DenyAnonymousAccessPolicy>(expectedLifecycle);

			// Assert
			var containers = policyContainers.ToList();
			Assert.That(containers[0].CacheStrategies.Single().PolicyType, Is.EqualTo(expectedType));
			Assert.That(containers[0].CacheStrategies.Single().CacheLifecycle, Is.EqualTo(expectedLifecycle));
			Assert.That(containers[0].CacheStrategies.Single().CacheLevel, Is.EqualTo(By.Controller));
			Assert.That(containers[1].CacheStrategies.Single().PolicyType, Is.EqualTo(expectedType));
			Assert.That(containers[1].CacheStrategies.Single().CacheLifecycle, Is.EqualTo(expectedLifecycle));
			Assert.That(containers[1].CacheStrategies.Single().CacheLevel, Is.EqualTo(By.Controller));
			Assert.That(containers[2].CacheStrategies.Single().PolicyType, Is.EqualTo(expectedType));
			Assert.That(containers[2].CacheStrategies.Single().CacheLifecycle, Is.EqualTo(expectedLifecycle));
			Assert.That(containers[2].CacheStrategies.Single().CacheLevel, Is.EqualTo(By.Controller));
		}
	}

	[TestFixture]
	[Category("ConventionPolicyContainerSpec")]
	public class When_setting_the_cache_lifecycle_and_cache_level_for_a_policy_on_a_conventionpolicycontainer
	{
		[Test]
		public void Should_add_policyresult_cache_strategy_to_policycontainers()
		{
			// Arrange
			var controllerName = NameHelper.Controller<AdminController>();
			var policyContainers = new List<PolicyContainer>
			{
				TestDataFactory.CreateValidPolicyContainer(controllerName, "Index"),
				TestDataFactory.CreateValidPolicyContainer(controllerName, "ListPosts"),
				TestDataFactory.CreateValidPolicyContainer(controllerName, "AddPost")
			};

			var conventionPolicyContainer = new ConventionPolicyContainer(policyContainers.Cast<IPolicyContainerConfiguration>().ToList());
			const Cache expectedLifecycle = Cache.PerHttpRequest;
			const By expectedLevel = By.ControllerAction;
			var expectedType = typeof(DenyAnonymousAccessPolicy);

			// Act
			conventionPolicyContainer.Cache<DenyAnonymousAccessPolicy>(expectedLifecycle, expectedLevel);

			// Assert
			var containers = policyContainers.ToList();
			Assert.That(containers[0].CacheStrategies.Single().PolicyType, Is.EqualTo(expectedType));
			Assert.That(containers[0].CacheStrategies.Single().CacheLifecycle, Is.EqualTo(expectedLifecycle));
			Assert.That(containers[0].CacheStrategies.Single().CacheLevel, Is.EqualTo(expectedLevel));
			Assert.That(containers[1].CacheStrategies.Single().PolicyType, Is.EqualTo(expectedType));
			Assert.That(containers[1].CacheStrategies.Single().CacheLifecycle, Is.EqualTo(expectedLifecycle));
			Assert.That(containers[1].CacheStrategies.Single().CacheLevel, Is.EqualTo(expectedLevel));
			Assert.That(containers[2].CacheStrategies.Single().PolicyType, Is.EqualTo(expectedType));
			Assert.That(containers[2].CacheStrategies.Single().CacheLifecycle, Is.EqualTo(expectedLifecycle));
			Assert.That(containers[2].CacheStrategies.Single().CacheLevel, Is.EqualTo(expectedLevel));
		}
	}

	[TestFixture]
	[Category("PolicyContainerSpec")]
	public class When_clearing_the_cache_strategies_of_a_conventionpolicycontainer
	{
		[Test]
		public void Should_clear_all_cache_strategies()
		{
			var controllerName = NameHelper.Controller<AdminController>();
			var policyContainers = new List<PolicyContainer>
			{
				TestDataFactory.CreateValidPolicyContainer(controllerName, "Index"),
				TestDataFactory.CreateValidPolicyContainer(controllerName, "ListPosts"),
				TestDataFactory.CreateValidPolicyContainer(controllerName, "AddPost")
			};

			var conventionPolicyContainer = new ConventionPolicyContainer(policyContainers.Cast<IPolicyContainerConfiguration>().ToList());
			conventionPolicyContainer.Cache<RequireAnyRolePolicy>(Cache.PerHttpRequest);

			// Act
			conventionPolicyContainer.ClearCacheStrategies();

			// Assert
			var containers = policyContainers.ToList();
			Assert.That(containers[0].CacheStrategies.Any(), Is.False);
			Assert.That(containers[1].CacheStrategies.Any(), Is.False);
			Assert.That(containers[2].CacheStrategies.Any(), Is.False);
		}

		[Test]
		public void Should_clear_all_cache_strategies_for_policy()
		{
			var controllerName = NameHelper.Controller<AdminController>();
			var policyContainers = new List<PolicyContainer>
			{
				TestDataFactory.CreateValidPolicyContainer(controllerName, "Index"),
				TestDataFactory.CreateValidPolicyContainer(controllerName, "ListPosts"),
				TestDataFactory.CreateValidPolicyContainer(controllerName, "AddPost")
			};

			var conventionPolicyContainer = new ConventionPolicyContainer(policyContainers.Cast<IPolicyContainerConfiguration>().ToList());
			conventionPolicyContainer.Cache<RequireAnyRolePolicy>(Cache.PerHttpRequest);
			conventionPolicyContainer.Cache<RequireAllRolesPolicy>(Cache.PerHttpRequest);

			// Act
			conventionPolicyContainer.ClearCacheStrategyFor<RequireAnyRolePolicy>();

			// Assert
			var containers = policyContainers.ToList();
			Assert.That(containers[0].CacheStrategies.Single().PolicyType, Is.EqualTo(typeof(RequireAllRolesPolicy)));
			Assert.That(containers[1].CacheStrategies.Single().PolicyType, Is.EqualTo(typeof(RequireAllRolesPolicy)));
			Assert.That(containers[2].CacheStrategies.Single().PolicyType, Is.EqualTo(typeof(RequireAllRolesPolicy)));
		}
	}
}