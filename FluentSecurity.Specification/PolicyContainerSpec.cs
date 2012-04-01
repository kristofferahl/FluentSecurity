using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FluentSecurity.Caching;
using FluentSecurity.Policy;
using FluentSecurity.ServiceLocation;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using Moq;
using NUnit.Framework;
using Rhino.Mocks;
using MockRepository = Rhino.Mocks.MockRepository;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("PolicyContainerSpec")]
	public class When_I_create_an_invalid_policycontainer
	{
		private string _validControllerName;
		private string _validActionName;
		private IPolicyAppender _validPolicyAppender;

		[SetUp]
		public void SetUp()
		{
			_validControllerName = TestDataFactory.ValidControllerName;
			_validActionName = TestDataFactory.ValidActionName;
			_validPolicyAppender = TestDataFactory.CreateValidPolicyAppender();
		}

		[Test]
		public void Should_throw_ArgumentException_when_the_controllername_is_null()
		{
			Assert.Throws<ArgumentException>(() =>
				new PolicyContainer(null, _validActionName, _validPolicyAppender)
			);
		}

		[Test]
		public void Should_throw_ArgumentException_when_controllername_is_empty()
		{
			Assert.Throws<ArgumentException>(() =>
				new PolicyContainer(string.Empty, _validActionName, _validPolicyAppender)
			);
		}

		[Test]
		public void Should_throw_ArgumentException_when_actionname_is_null()
		{
			Assert.Throws<ArgumentException>(() =>
				new PolicyContainer(_validControllerName, null, _validPolicyAppender)
			);
		}

		[Test]
		public void Should_throw_ArgumentException_when_actionname_is_empty()
		{
			Assert.Throws<ArgumentException>(() =>
				new PolicyContainer(_validControllerName, string.Empty, _validPolicyAppender)
			);
		}

		[Test]
		public void Should_throw_ArgumentException_when_policy_manager_is_null()
		{
			// Arrange
			const IPolicyAppender policyAppender = null;

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() =>
				new PolicyContainer(_validControllerName, _validActionName, policyAppender)
			);
		}
	}

	[TestFixture]
	[Category("PolicyContainerSpec")]
	public class When_I_create_a_valid_PolicyContainer
	{
		private IPolicyAppender _expectedPolicyAppender = new DefaultPolicyAppender();
		private string _expectedControllerName = "SomeController";
		private string _expectedActionName = "SomeAction";

		[SetUp]
		public void SetUp()
		{
			_expectedControllerName = TestDataFactory.ValidControllerName;
			_expectedActionName = TestDataFactory.ValidActionName;
			_expectedPolicyAppender = TestDataFactory.CreateValidPolicyAppender();
		}

		private IPolicyContainer Because()
		{
			return new PolicyContainer(_expectedControllerName, _expectedActionName, _expectedPolicyAppender);
		}

		[Test]
		public void Should_have_controller_name_set_to_SomeController()
		{
			// Act
			var policyContainer = Because();

			// Assert
			Assert.That(policyContainer.ControllerName, Is.EqualTo(_expectedControllerName));
		}

		[Test]
		public void Should_have_action_name_set_to_SomeAction()
		{
			// Act
			var policyContainer = Because();

			// Assert
			Assert.That(policyContainer.ActionName, Is.EqualTo(_expectedActionName));
		}

		[Test]
		public void Should_have_Manager_set_to_DefaultPolicyAppender()
		{
			// Act
			var policyContainer = Because();

			// Assert
			Assert.That(policyContainer.PolicyAppender, Is.EqualTo(_expectedPolicyAppender));
		}

	}

	[TestFixture]
	[Category("PolicContainerExtensionsSpec")]
	public class When_adding_two_policies_of_the_same_type_to_a_policycontainer
	{
		private PolicyContainer _policyContainer;

		[SetUp]
		public void SetUp()
		{
			// Arrange
			_policyContainer = TestDataFactory.CreateValidPolicyContainer();
		}

		private void Because()
		{
			_policyContainer
				.AddPolicy(new DenyAnonymousAccessPolicy())
				.AddPolicy(new DenyAnonymousAccessPolicy());
		}

		[Test]
		public void Should_have_1_policy()
		{
			// Act
			Because();

			// Assert
			Assert.That(_policyContainer.GetPolicies().Count(), Is.EqualTo(1));
		}
	}

	[TestFixture]
	[Category("PolicContainerExtensionsSpec")]
	public class When_removing_policies_from_a_policy_container
	{
		private PolicyContainer _policyContainer;
		private readonly Policy1 _policy1 = new Policy1();
		private readonly Policy2 _policy2 = new Policy2();

		[SetUp]
		public void SetUp()
		{
			// Arrange
			_policyContainer = TestDataFactory.CreateValidPolicyContainer();
			_policyContainer.SecurityConfigurationProvider = TestDataFactory.CreateValidSecurityConfiguration;
			_policyContainer
				.AddPolicy(_policy1)
				.AddPolicy(_policy2);
		}

		[Test]
		public void Should_remove_policy1()
		{
			// Act
			_policyContainer.RemovePolicy<Policy1>();

			// Assert
			Assert.That(_policyContainer.GetPolicies().Single(), Is.EqualTo(_policy2));
		}

		[Test]
		public void Should_remove_policy2()
		{
			// Act
			_policyContainer.RemovePolicy<Policy2>();

			// Assert
			Assert.That(_policyContainer.GetPolicies().Single(), Is.EqualTo(_policy1));
		}

		[Test]
		public void Should_remove_policy_matching_predicate()
		{
			// Act
			_policyContainer.RemovePolicy<Policy1>(p => p.Name == "Policy1");

			// Assert
			Assert.That(_policyContainer.GetPolicies().Single(), Is.EqualTo(_policy2));
		}

		[Test]
		public void Should_not_remove_policies_not_matching_predicate()
		{
			// Act
			_policyContainer.RemovePolicy<Policy1>(p => p.Name == "X");

			// Assert
			Assert.That(_policyContainer.GetPolicies().Count(), Is.EqualTo(2));
		}

		[Test]
		public void Should_remove_all_policies()
		{
			// Act
			_policyContainer.RemovePolicy<ISecurityPolicy>();

			// Assert
			Assert.That(_policyContainer.GetPolicies().Any(), Is.False);
		}

		[Test]
		public void Should_not_remove_any_policies()
		{
			// Act
			_policyContainer.RemovePolicy<DenyAnonymousAccessPolicy>();

			// Assert
			Assert.That(_policyContainer.GetPolicies().Count(), Is.EqualTo(2));
			Assert.That(_policyContainer.GetPolicies().First(), Is.EqualTo(_policy1));
			Assert.That(_policyContainer.GetPolicies().Last(), Is.EqualTo(_policy2));
		}

		public class Policy1 : ISecurityPolicy
		{
			public PolicyResult Enforce(ISecurityContext context)
			{
				return PolicyResult.CreateSuccessResult(this);
			}

			public string Name = "Policy1";
		}

		public class Policy2 : ISecurityPolicy
		{
			public PolicyResult Enforce(ISecurityContext context)
			{
				return PolicyResult.CreateSuccessResult(this);
			}
		}
	}
	
	[TestFixture]
	[Category("PolicyContainerSpec")]
	public class When_enforcing_policies
	{
		[Test]
		public void Should_invoke_the_isautheticated_and_roles_functions()
		{
			// Arrange
			var context = MockRepository.GenerateMock<ISecurityContext>();
			context.Expect(x => x.CurrenUserAuthenticated()).Return(true).Repeat.Once();
			context.Expect(x => x.CurrenUserRoles()).Return(new List<object> { UserRole.Owner }.ToArray()).Repeat.Once();
			context.Replay();
			
			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
			policyContainer.SecurityConfigurationProvider = TestDataFactory.CreateValidSecurityConfiguration;
			policyContainer.AddPolicy(new TestPolicy());

			// Act
			policyContainer.EnforcePolicies(context);

			// Assert
			context.VerifyAllExpectations();
		}

		[Test]
		public void Should_enforce_policies_with_context()
		{
			// Arrange
			var roles = new List<object> { UserRole.Owner }.ToArray();
			const bool isAuthenticated = true;

			var context = new Mock<ISecurityContext>();
			context.Setup(x => x.CurrenUserAuthenticated()).Returns(isAuthenticated);
			context.Setup(x => x.CurrenUserRoles()).Returns(roles);

			var policy = new Mock<ISecurityPolicy>();
			policy.Setup(x => x.Enforce(It.Is<ISecurityContext>(c => c.CurrenUserAuthenticated() == isAuthenticated && c.CurrenUserRoles() == roles))).Returns(PolicyResult.CreateSuccessResult(policy.Object));

			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
			policyContainer.SecurityConfigurationProvider = TestDataFactory.CreateValidSecurityConfiguration;
			policyContainer.AddPolicy(policy.Object);

			// Act
			policyContainer.EnforcePolicies(context.Object);

			// Assert
			policy.VerifyAll();
		}

		[Test]
		public void Should_return_results()
		{
			// Arrange
			var roles = new List<object> { UserRole.Owner }.ToArray();
			const bool isAuthenticated = true;
			const string failureOccured = "Failure occured";
			var context = TestDataFactory.CreateSecurityContext(isAuthenticated, roles);

			var policy = new Mock<ISecurityPolicy>();
			policy.Setup(x => x.Enforce(It.IsAny<ISecurityContext>())).Returns(PolicyResult.CreateFailureResult(policy.Object, failureOccured));

			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
			policyContainer.SecurityConfigurationProvider = TestDataFactory.CreateValidSecurityConfiguration;
			policyContainer.AddPolicy(policy.Object);

			// Act
			var results = policyContainer.EnforcePolicies(context);

			// Assert
			Assert.That(results.Count(), Is.EqualTo(1));
			Assert.That(results.Single().ViolationOccured, Is.True);
			Assert.That(results.Single().Message, Is.EqualTo(failureOccured));
		}

		[Test]
		public void Should_stop_on_first_violation_and_return_1_result()
		{
			// Arrange
			var context = TestDataFactory.CreateSecurityContext(false);

			var firstPolicy = new Mock<ISecurityPolicy>();
			firstPolicy.Setup(x => x.Enforce(It.IsAny<ISecurityContext>())).Returns(PolicyResult.CreateFailureResult(firstPolicy.Object, "Failure occured"));

			var secondPolicy = new Mock<ISecurityPolicy>();
			secondPolicy.Setup(x => x.Enforce(It.IsAny<ISecurityContext>())).Returns(PolicyResult.CreateSuccessResult(secondPolicy.Object));

			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
			policyContainer.SecurityConfigurationProvider = TestDataFactory.CreateValidSecurityConfiguration;
			policyContainer.AddPolicy(firstPolicy.Object).AddPolicy(secondPolicy.Object);

			// Act
			var results = policyContainer.EnforcePolicies(context);

			// Assert
			Assert.That(results.Count(), Is.EqualTo(1));
			Assert.That(results.Single().ViolationOccured, Is.True);
		}

		[Test]
		public void Should_stop_on_first_violation_and_return_2_results()
		{
			// Arrange
			var context = TestDataFactory.CreateSecurityContext(false);

			var firstPolicy = new TestPolicy();

			var secondPolicy = new Mock<ISecurityPolicy>();
			secondPolicy.Setup(x => x.Enforce(It.IsAny<ISecurityContext>())).Returns(PolicyResult.CreateFailureResult(secondPolicy.Object, "Failure occured"));

			var thirdPolicy = new TestPolicy();

			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
			policyContainer.SecurityConfigurationProvider = TestDataFactory.CreateValidSecurityConfiguration;
			policyContainer.AddPolicy(firstPolicy).AddPolicy(secondPolicy.Object).AddPolicy(thirdPolicy);

			// Act
			var results = policyContainer.EnforcePolicies(context);

			// Assert
			Assert.That(results.Count(), Is.EqualTo(2));
			Assert.That(results.First().ViolationOccured, Is.False);
			Assert.That(results.Last().ViolationOccured, Is.True);
		}

		[Test]
		public void Should_throw_ConfigurationErrorsException_when_a_container_has_no_policies()
		{
			// Arrange
			ExceptionFactory.RequestDescriptionProvider = () => TestDataFactory.CreateRequestDescription();

			var context = TestDataFactory.CreateSecurityContext(false);
			var policyContainer = TestDataFactory.CreateValidPolicyContainer();
			policyContainer.SecurityConfigurationProvider = TestDataFactory.CreateValidSecurityConfiguration;

			// Act & Assert
			Assert.Throws<ConfigurationErrorsException>(() => policyContainer.EnforcePolicies(context));
		}

		private class TestPolicy : ISecurityPolicy
		{
			public PolicyResult Enforce(ISecurityContext context)
			{
				// NOTE: OK to leave like this as tests depends on it.
				var authenticated = context.CurrenUserAuthenticated();
				var roles = context.CurrenUserRoles();
				return PolicyResult.CreateSuccessResult(this);
			}
		}
	}

	[TestFixture]
	[Category("PolicyContainerSpec")]
	public class When_enforcing_policies_with_default_cache_lifecycle_set_to_DoNotCache
	{
		[Test]
		public void Should_return_unique_results()
		{
			// Arrange
			var firstPolicy = new IgnorePolicy();			
			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
			policyContainer.SecurityConfigurationProvider = () => TestDataFactory.CreateValidSecurityConfiguration(configuration => configuration.Advanced.SetDefaultResultsCacheLifecycle(Cache.DoNotCache));
			policyContainer.AddPolicy(firstPolicy);

			// Act
			var context = TestDataFactory.CreateSecurityContext(false);
			var results1 = policyContainer.EnforcePolicies(context);
			var results2 = policyContainer.EnforcePolicies(context);

			// Assert
			Assert.That(results1.Single(), Is.Not.EqualTo(results2.Single()));
		}
	}

	[TestFixture]
	[Category("PolicyContainerSpec")]
	public class When_enforcing_policies_with_default_cache_lifecycle_set_to_PerHttpRequest
	{
		[Test]
		public void Should_return_the_same_results()
		{
			// Arrange
			var context = TestDataFactory.CreateSecurityContext(false);
			var firstPolicy = new IgnorePolicy();
			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
			policyContainer.SecurityConfigurationProvider = () => TestDataFactory.CreateValidSecurityConfiguration(configuration => configuration.Advanced.SetDefaultResultsCacheLifecycle(Cache.PerHttpRequest));
			policyContainer.AddPolicy(firstPolicy);

			// Act
			var results1 = policyContainer.EnforcePolicies(context);
			var results2 = policyContainer.EnforcePolicies(context);

			SecurityCache.ClearCache(Lifecycle.HybridHttpContext);;

			var results3 = policyContainer.EnforcePolicies(context);
			var results4 = policyContainer.EnforcePolicies(context);

			// Assert
			Assert.That(results1.Single(), Is.EqualTo(results2.Single()));
			Assert.That(results3.Single(), Is.EqualTo(results4.Single()));

			Assert.That(results1.Single(), Is.Not.EqualTo(results3.Single()), "Results should not be equal across requests.");
		}
	}

	[TestFixture]
	[Category("PolicyContainerSpec")]
	public class When_enforcing_policies_with_default_cache_lifecycle_set_to_PerHttpSession
	{
		[Test]
		public void Should_return_the_same_results()
		{
			// Arrange
			var context = TestDataFactory.CreateSecurityContext(false);
			var firstPolicy = new IgnorePolicy();
			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
			policyContainer.SecurityConfigurationProvider = () => TestDataFactory.CreateValidSecurityConfiguration(configuration => configuration.Advanced.SetDefaultResultsCacheLifecycle(Cache.PerHttpSession));
			policyContainer.AddPolicy(firstPolicy);

			// Act
			var results1 = policyContainer.EnforcePolicies(context);
			var results2 = policyContainer.EnforcePolicies(context);

			SecurityCache.ClearCache(Lifecycle.HybridHttpSession); ;

			var results3 = policyContainer.EnforcePolicies(context);
			var results4 = policyContainer.EnforcePolicies(context);

			// Assert
			Assert.That(results1.Single(), Is.EqualTo(results2.Single()));
			Assert.That(results3.Single(), Is.EqualTo(results4.Single()));

			Assert.That(results1.Single(), Is.Not.EqualTo(results3.Single()), "Results should not be equal across sessions.");
		}
	}

	[TestFixture]
	[Category("PolicyContainerSpec")]
	public class When_setting_the_cache_lifecycle
	{
		[Test]
		public void Should_add_policyresult_cache_strategy_for_RequireRolePolicy_with_lifecycle_set_to_DoNotCache()
		{
			const Cache expectedLifecycle = Cache.DoNotCache;
			const string expectedControllerName = "Controller1";
			const string expectedActionName = "Action1";

			var policyContainer = new PolicyContainer(expectedControllerName, expectedActionName, TestDataFactory.CreateValidPolicyAppender());

			// Act
			policyContainer.CacheResultsOf<RequireRolePolicy>(expectedLifecycle);

			// Assert
			var policyResultCacheStrategy = policyContainer.CacheStrategies.Single();
			Assert.That(policyResultCacheStrategy.ControllerName, Is.EqualTo(expectedControllerName));
			Assert.That(policyResultCacheStrategy.ActionName, Is.EqualTo(expectedActionName));
			Assert.That(policyResultCacheStrategy.PolicyType, Is.EqualTo(typeof(RequireRolePolicy)));
			Assert.That(policyResultCacheStrategy.CacheLifecycle, Is.EqualTo(expectedLifecycle));
			Assert.That(policyResultCacheStrategy.CacheLevel, Is.EqualTo(By.ControllerAction));
		}

		[Test]
		public void Should_add_policyresult_cache_strategy_for_RequireRolePolicy_with_lifecycle_set_to_PerHttpRequest()
		{
			const Cache expectedLifecycle = Cache.PerHttpRequest;
			const string expectedControllerName = "Controller2";
			const string expectedActionName = "Action2";
			
			var policyContainer = new PolicyContainer(expectedControllerName, expectedActionName, TestDataFactory.CreateValidPolicyAppender());

			// Act
			policyContainer.CacheResultsOf<RequireRolePolicy>(expectedLifecycle);

			// Assert
			var policyResultCacheStrategy = policyContainer.CacheStrategies.Single();
			Assert.That(policyResultCacheStrategy.ControllerName, Is.EqualTo(expectedControllerName));
			Assert.That(policyResultCacheStrategy.ActionName, Is.EqualTo(expectedActionName));
			Assert.That(policyResultCacheStrategy.PolicyType, Is.EqualTo(typeof(RequireRolePolicy)));
			Assert.That(policyResultCacheStrategy.CacheLifecycle, Is.EqualTo(expectedLifecycle));
			Assert.That(policyResultCacheStrategy.CacheLevel, Is.EqualTo(By.ControllerAction));
		}

		[Test]
		public void Should_add_policyresult_cache_strategy_for_RequireRolePolicy_with_lifecycle_set_to_PerHttpSession()
		{
			const Cache expectedLifecycle = Cache.PerHttpSession;
			const string expectedControllerName = "Controller3";
			const string expectedActionName = "Action3";

			var policyContainer = new PolicyContainer(expectedControllerName, expectedActionName, TestDataFactory.CreateValidPolicyAppender());

			// Act
			policyContainer.CacheResultsOf<RequireRolePolicy>(expectedLifecycle);

			// Assert
			var policyResultCacheStrategy = policyContainer.CacheStrategies.Single();
			Assert.That(policyResultCacheStrategy.ControllerName, Is.EqualTo(expectedControllerName));
			Assert.That(policyResultCacheStrategy.ActionName, Is.EqualTo(expectedActionName));
			Assert.That(policyResultCacheStrategy.PolicyType, Is.EqualTo(typeof(RequireRolePolicy)));
			Assert.That(policyResultCacheStrategy.CacheLifecycle, Is.EqualTo(expectedLifecycle));
			Assert.That(policyResultCacheStrategy.CacheLevel, Is.EqualTo(By.ControllerAction));
		}

		[Test]
		public void Should_add_policyresult_cache_strategy_for_DenyAnonymousAccessPolicy_with_lifecycle_set_to_PerHttpRequest()
		{
			const Cache expectedLifecycle = Cache.PerHttpRequest;
			const string expectedControllerName = "Controller4";
			const string expectedActionName = "Action4";

			var policyContainer = new PolicyContainer(expectedControllerName, expectedActionName, TestDataFactory.CreateValidPolicyAppender());

			// Act
			policyContainer.CacheResultsOf<DenyAnonymousAccessPolicy>(expectedLifecycle);

			// Assert
			var policyResultCacheStrategy = policyContainer.CacheStrategies.Single();
			Assert.That(policyResultCacheStrategy.ControllerName, Is.EqualTo(expectedControllerName));
			Assert.That(policyResultCacheStrategy.ActionName, Is.EqualTo(expectedActionName));
			Assert.That(policyResultCacheStrategy.PolicyType, Is.EqualTo(typeof(DenyAnonymousAccessPolicy)));
			Assert.That(policyResultCacheStrategy.CacheLifecycle, Is.EqualTo(expectedLifecycle));
			Assert.That(policyResultCacheStrategy.CacheLevel, Is.EqualTo(By.ControllerAction));
		}

		[Test]
		public void Should_add_policyresult_cache_strategies_for_each_policy_type()
		{
			// Arrange
			const string expectedControllerName = "Controller5";
			const string expectedActionName = "Action5";
			var policyContainer = new PolicyContainer(expectedControllerName, expectedActionName, TestDataFactory.CreateValidPolicyAppender());

			// Act
			policyContainer
				.CacheResultsOf<RequireAllRolesPolicy>(Cache.PerHttpRequest)
				.CacheResultsOf<RequireRolePolicy>(Cache.PerHttpSession);

			// Assert
			Assert.That(policyContainer.CacheStrategies.Count, Is.EqualTo(2));

			var strategy1 = policyContainer.CacheStrategies.First();
			Assert.That(strategy1.ControllerName, Is.EqualTo(expectedControllerName));
			Assert.That(strategy1.ActionName, Is.EqualTo(expectedActionName));
			Assert.That(strategy1.PolicyType, Is.EqualTo(typeof(RequireAllRolesPolicy)));
			Assert.That(strategy1.CacheLifecycle, Is.EqualTo(Cache.PerHttpRequest));
			Assert.That(strategy1.CacheLevel, Is.EqualTo(By.ControllerAction));

			var strategy2 = policyContainer.CacheStrategies.Last();
			Assert.That(strategy2.ControllerName, Is.EqualTo(expectedControllerName));
			Assert.That(strategy2.ActionName, Is.EqualTo(expectedActionName));
			Assert.That(strategy2.PolicyType, Is.EqualTo(typeof(RequireRolePolicy)));
			Assert.That(strategy2.CacheLifecycle, Is.EqualTo(Cache.PerHttpSession));
			Assert.That(strategy2.CacheLevel, Is.EqualTo(By.ControllerAction));
		}

		[Test]
		public void Should_update_existing_policyresult_cache_strategies()
		{
			// Arrange
			const Cache expectedLifecycle = Cache.PerHttpSession;
			const string expectedControllerName = "Controller6";
			const string expectedActionName = "Action6";
			var policyContainer = new PolicyContainer(expectedControllerName, expectedActionName, TestDataFactory.CreateValidPolicyAppender());

			// Act
			policyContainer
				.CacheResultsOf<RequireAllRolesPolicy>(Cache.PerHttpRequest)
				.CacheResultsOf<RequireAllRolesPolicy>(Cache.PerHttpSession);

			// Assert
			var policyResultCacheStrategy = policyContainer.CacheStrategies.Single();
			Assert.That(policyResultCacheStrategy.ControllerName, Is.EqualTo(expectedControllerName));
			Assert.That(policyResultCacheStrategy.ActionName, Is.EqualTo(expectedActionName));
			Assert.That(policyResultCacheStrategy.PolicyType, Is.EqualTo(typeof(RequireAllRolesPolicy)));
			Assert.That(policyResultCacheStrategy.CacheLifecycle, Is.EqualTo(expectedLifecycle));
			Assert.That(policyResultCacheStrategy.CacheLevel, Is.EqualTo(By.ControllerAction));
		}
	}

	[TestFixture]
	[Category("PolicyContainerSpec")]
	public class When_setting_the_cache_lifecycle_and_cache_level
	{
		[Test]
		public void Should_add_policyresult_cache_strategy_for_RequireRolePolicy_with_level_set_to_ControllerAction()
		{
			const By expectedLevel = By.ControllerAction;
			var policyContainer = new PolicyContainer("Controller", "Action", TestDataFactory.CreateValidPolicyAppender());

			// Act
			policyContainer.CacheResultsOf<RequireRolePolicy>(Cache.PerHttpRequest, expectedLevel);

			// Assert
			var policyResultCacheStrategy = policyContainer.CacheStrategies.Single();
			Assert.That(policyResultCacheStrategy.CacheLevel, Is.EqualTo(expectedLevel));
		}

		[Test]
		public void Should_add_policyresult_cache_strategy_for_RequireRolePolicy_with_level_set_to_Controller()
		{
			const By expectedLevel = By.Controller;
			var policyContainer = new PolicyContainer("Controller", "Action", TestDataFactory.CreateValidPolicyAppender());

			// Act
			policyContainer.CacheResultsOf<RequireRolePolicy>(Cache.PerHttpRequest, expectedLevel);

			// Assert
			var policyResultCacheStrategy = policyContainer.CacheStrategies.Single();
			Assert.That(policyResultCacheStrategy.CacheLevel, Is.EqualTo(expectedLevel));
		}

		[Test]
		public void Should_add_policyresult_cache_strategy_for_RequireRolePolicy_with_level_set_to_Policy()
		{
			const By expectedLevel = By.Policy;
			var policyContainer = new PolicyContainer("Controller", "Action", TestDataFactory.CreateValidPolicyAppender());

			// Act
			policyContainer.CacheResultsOf<RequireRolePolicy>(Cache.PerHttpRequest, expectedLevel);

			// Assert
			var policyResultCacheStrategy = policyContainer.CacheStrategies.Single();
			Assert.That(policyResultCacheStrategy.CacheLevel, Is.EqualTo(expectedLevel));
		}

		[Test]
		public void Should_update_existing_policyresult_cache_strategies()
		{
			// Arrange
			const Cache expectedLifecycle = Cache.PerHttpSession;
			const string expectedControllerName = "Controller6";
			const string expectedActionName = "Action6";
			var policyContainer = new PolicyContainer(expectedControllerName, expectedActionName, TestDataFactory.CreateValidPolicyAppender());

			// Act
			policyContainer
				.CacheResultsOf<RequireAllRolesPolicy>(Cache.PerHttpRequest, By.Controller)
				.CacheResultsOf<RequireAllRolesPolicy>(Cache.PerHttpSession, By.Policy);

			// Assert
			var policyResultCacheStrategy = policyContainer.CacheStrategies.Single();
			Assert.That(policyResultCacheStrategy.ControllerName, Is.EqualTo(expectedControllerName));
			Assert.That(policyResultCacheStrategy.ActionName, Is.EqualTo(expectedActionName));
			Assert.That(policyResultCacheStrategy.PolicyType, Is.EqualTo(typeof(RequireAllRolesPolicy)));
			Assert.That(policyResultCacheStrategy.CacheLifecycle, Is.EqualTo(expectedLifecycle));
			Assert.That(policyResultCacheStrategy.CacheLevel, Is.EqualTo(By.Policy));
		}
	}

	[TestFixture]
	[Category("PolicyContainerSpec")]
	public class When_clearing_the_cache_strategy
	{
		[Test]
		public void Should_clear_all_cache_strategies()
		{
			var policyContainer = new PolicyContainer("Controller", "Action", TestDataFactory.CreateValidPolicyAppender());
			policyContainer.CacheResultsOf<RequireRolePolicy>(Cache.PerHttpRequest);

			// Act
			policyContainer.ClearCacheStrategies();

			// Assert
			Assert.That(policyContainer.CacheStrategies.Any(), Is.False);
		}

		[Test]
		public void Should_clear_all_cache_strategies_for_policy()
		{
			var policyContainer = new PolicyContainer("Controller", "Action", TestDataFactory.CreateValidPolicyAppender());
			policyContainer.CacheResultsOf<RequireRolePolicy>(Cache.PerHttpRequest);
			policyContainer.CacheResultsOf<RequireAllRolesPolicy>(Cache.PerHttpRequest);

			// Act
			policyContainer.ClearCacheStrategyFor<RequireRolePolicy>();

			// Assert
			Assert.That(policyContainer.CacheStrategies.Single().PolicyType, Is.EqualTo(typeof(RequireAllRolesPolicy)));
		}
	}

	[TestFixture]
	[Category("PolicyContainerSpec")]
	public class When_enforcing_policies_with_default_cache_lifecycle_set
	{
		[Test]
		public void Should_use_cache_lifecycle_specified_when_adding_a_policy()
		{
			// Arrange
			const Cache defaultCacheLifecycle = Cache.PerHttpSession;
			const Cache specifiedCacheLifecycle = Cache.PerHttpRequest;
			
			var context = TestDataFactory.CreateSecurityContext(false);
			var securityPolicy = new IgnorePolicy();
			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
			policyContainer.SecurityConfigurationProvider = () => TestDataFactory.CreateValidSecurityConfiguration(configuration => configuration.Advanced.SetDefaultResultsCacheLifecycle(defaultCacheLifecycle));
			policyContainer.AddPolicy(securityPolicy).CacheResultsOf<IgnorePolicy>(specifiedCacheLifecycle);

			// Act
			var results1 = policyContainer.EnforcePolicies(context);
			var results2 = policyContainer.EnforcePolicies(context);

			SecurityCache.ClearCache(Lifecycle.HybridHttpContext); ;

			var results3 = policyContainer.EnforcePolicies(context);
			var results4 = policyContainer.EnforcePolicies(context);

			// Assert
			Assert.That(results1.Single(), Is.EqualTo(results2.Single()));
			Assert.That(results3.Single(), Is.EqualTo(results4.Single()));

			Assert.That(results1.Single(), Is.Not.EqualTo(results3.Single()), "Results should not be equal across requests.");
		}
	}
}