using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FluentSecurity.Caching;
using FluentSecurity.Configuration;
using FluentSecurity.Internals;
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
		public void Should_have_PolicyAppender_set_to_DefaultPolicyAppender()
		{
			// Act
			var policyContainer = (PolicyContainer) Because();

			// Assert
			Assert.That(policyContainer.PolicyAppender, Is.EqualTo(_expectedPolicyAppender));
		}

		[Test]
		public void Should_override_ToString()
		{
			// Act
			var policyContainer = (PolicyContainer)Because();

			// Assert
			Assert.That(policyContainer.ToString(), Is.EqualTo("FluentSecurity.PolicyContainer - SomeController - SomeAction"));
		}
	}

	[TestFixture]
	[Category("PolicContainerExtensionsSpec")]
	public class When_adding_a_policy_instance_to_a_policycontainer
	{
		private ISecurityPolicy _policy;
		private PolicyContainer _policyContainer;
		private IPolicyContainerConfiguration _return;

		[SetUp]
		public void SetUp()
		{
			// Arrange
			_policy = new DenyAnonymousAccessPolicy();
			_policyContainer = TestDataFactory.CreateValidPolicyContainer();

			// Act
			_return = _policyContainer.AddPolicy(_policy);
		}

		[Test]
		public void Should_have_1_policy()
		{
			Assert.That(_policyContainer.GetPolicies().Single(), Is.EqualTo(_policy));
		}

		[Test]
		public void Should_return_IPolicyContainerConfiguration()
		{
			Assert.That(_return, Is.Not.Null);
			Assert.That(_return, Is.AssignableTo<IPolicyContainerConfiguration>());
		}
	}

	[TestFixture]
	[Category("PolicContainerExtensionsSpec")]
	public class When_adding_a_policy_of_T_to_a_policycontainer
	{
		private PolicyContainer _policyContainer;
		private IPolicyContainerConfiguration _return;

		[SetUp]
		public void SetUp()
		{
			// Arrange
			_policyContainer = TestDataFactory.CreateValidPolicyContainer();

			// Act
			_return = _policyContainer.AddPolicy<SomePolicy>();
		}
		
		[Test]
		public void Should_have_a_lazy_policy_of_type_SomePolicy()
		{
			Assert.That(_policyContainer.GetPolicies().Single().GetType(), Is.EqualTo(typeof(LazySecurityPolicy<SomePolicy>)));
		}

		[Test]
		public void Should_return_IPolicyContainerConfiguration_of_T()
		{
			Assert.That(_return, Is.Not.Null);
			Assert.That(_return, Is.AssignableTo<IPolicyContainerConfiguration>());
			Assert.That(_return, Is.AssignableTo<IPolicyContainerConfiguration<SomePolicy>>());
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
	[Category("PolicContainerExtensionsSpec")]
	public class When_adding_two_policies_of_the_same_type_to_a_policycontainer
	{
		[Test]
		public void Should_have_1_policy_when_adding_policy_instances()
		{
			// Arrange
			var policyContainer = TestDataFactory.CreateValidPolicyContainer();

			// Act
			policyContainer
				.AddPolicy(new DenyAnonymousAccessPolicy())
				.AddPolicy(new DenyAnonymousAccessPolicy());

			// Assert
			Assert.That(policyContainer.GetPolicies().Count(), Is.EqualTo(1));
		}

		[Test]
		public void Should_have_1_policy_when_adding_policies_of_T()
		{
			// Arrange
			var policyContainer = TestDataFactory.CreateValidPolicyContainer();

			// Act
			policyContainer
				.AddPolicy<DenyAnonymousAccessPolicy>()
				.AddPolicy<DenyAnonymousAccessPolicy>();

			// Assert
			Assert.That(policyContainer.GetPolicies().Count(), Is.EqualTo(1));
		}

		[Test]
		public void Should_have_1_policy_when_adding_policy_instance_and_policy_of_T()
		{
			// Arrange
			var policyContainer = TestDataFactory.CreateValidPolicyContainer();

			// Act
			policyContainer
				.AddPolicy(new DenyAnonymousAccessPolicy())
				.AddPolicy<DenyAnonymousAccessPolicy>();

			// Assert
			Assert.That(policyContainer.GetPolicies().Count(), Is.EqualTo(1));
		}

		[Test]
		public void Should_have_1_policy_when_adding_policy_of_T_and_policy_instance()
		{
			// Arrange
			var policyContainer = TestDataFactory.CreateValidPolicyContainer();

			// Act
			policyContainer
				.AddPolicy<DenyAnonymousAccessPolicy>()
				.AddPolicy(new DenyAnonymousAccessPolicy());

			// Assert
			Assert.That(policyContainer.GetPolicies().Count(), Is.EqualTo(1));
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
		public void Should_remove_lazy_policy()
		{
			// Arrange
			_policyContainer.AddPolicy<Policy3>();

			// Act
			_policyContainer.RemovePolicy<Policy3>();

			// Assert
			Assert.That(_policyContainer.GetPolicies().First(), Is.EqualTo(_policy1));
			Assert.That(_policyContainer.GetPolicies().Last(), Is.EqualTo(_policy2));
			Assert.That(_policyContainer.GetPolicies().Count(), Is.EqualTo(2));
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
		public void Should_remove_lazy_policy_matching_predicate()
		{
			// Arrange
			_policyContainer.AddPolicy<Policy3>();

			// Act
			_policyContainer.RemovePolicy<Policy3>(p => p.Value == "SomeValue");

			// Assert
			Assert.That(_policyContainer.GetPolicies().First(), Is.EqualTo(_policy1));
			Assert.That(_policyContainer.GetPolicies().Last(), Is.EqualTo(_policy2));
			Assert.That(_policyContainer.GetPolicies().Count(), Is.EqualTo(2));
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
		public void Should_not_remove_lazy_policy_not_matching_predicate()
		{
			// Arrange
			_policyContainer.AddPolicy<Policy3>();

			// Act
			_policyContainer.RemovePolicy<Policy3>(p => p.Value == "X");

			// Assert
			Assert.That(_policyContainer.GetPolicies().ElementAt(0), Is.EqualTo(_policy1));
			Assert.That(_policyContainer.GetPolicies().ElementAt(1), Is.EqualTo(_policy2));
			Assert.That(_policyContainer.GetPolicies().ElementAt(2).GetPolicyType(), Is.EqualTo(typeof(Policy3)));
			Assert.That(_policyContainer.GetPolicies().Count(), Is.EqualTo(3));
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

		public class Policy3 : ISecurityPolicy
		{
			public string Value = "SomeValue";

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
			context.Expect(x => x.Runtime).Return(TestDataFactory.CreateSecurityRuntime()).Repeat.Once();
			context.Expect(x => x.CurrentUserIsAuthenticated()).Return(true).Repeat.Once();
			context.Expect(x => x.CurrentUserRoles()).Return(new List<object> { UserRole.Owner }.ToArray()).Repeat.Once();
			context.Replay();
			
			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
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
			context.Setup(x => x.Runtime).Returns(TestDataFactory.CreateSecurityRuntime());
			context.Setup(x => x.CurrentUserIsAuthenticated()).Returns(isAuthenticated);
			context.Setup(x => x.CurrentUserRoles()).Returns(roles);

			var policy = new Mock<ISecurityPolicy>();
			policy.Setup(x => x.Enforce(It.Is<ISecurityContext>(c => c.CurrentUserIsAuthenticated() == isAuthenticated && c.CurrentUserRoles() == roles))).Returns(PolicyResult.CreateSuccessResult(policy.Object));

			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
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
			var context = TestDataFactory.CreateSecurityContext(false);
			var policyContainer = TestDataFactory.CreateValidPolicyContainer();

			// Act & Assert
			Assert.Throws<ConfigurationErrorsException>(() => policyContainer.EnforcePolicies(context));
		}

		private class TestPolicy : ISecurityPolicy
		{
			public PolicyResult Enforce(ISecurityContext context)
			{
				// NOTE: OK to leave like this as tests depends on it.
				var authenticated = context.CurrentUserIsAuthenticated();
				var roles = context.CurrentUserRoles();
				return PolicyResult.CreateSuccessResult(this);
			}
		}
	}

	[Category("PolicyContainerSpec")]
	public class When_enforcing_lazy_policies
	{
		[Test]
		public void Should_load_lazy_policy_exactly_twice_during_execution_with_caching_off()
		{
			// Arrange
			var callsToContainer = 0;
			var policy = new LazyLoadedPolicy();
			FakeIoC.GetAllInstancesProvider = () =>
			{
				callsToContainer++;
				return new List<object> { policy };
			};
			SecurityConfigurator.Configure(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(TestDataFactory.ValidIsAuthenticatedFunction);
				configuration.ResolveServicesUsing(FakeIoC.GetAllInstances);
			});
			var context = new MockSecurityContext();
			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
			policyContainer.AddPolicy<LazyLoadedPolicy>();

			// Act
			policyContainer.EnforcePolicies(context);
			policyContainer.EnforcePolicies(context);

			// Assert
			Assert.That(callsToContainer, Is.EqualTo(2));
			Assert.That(policy.EnforceCallCount, Is.EqualTo(2), "Did not call enforce the expected amount of times");
		}

		[Test]
		public void Should_load_lazy_policy_exactly_once_during_execution_and_caching_on()
		{
			// Arrange
			var callsToContainer = 0;
			var policy = new LazyLoadedPolicy();
			FakeIoC.GetAllInstancesProvider = () =>
			{
				callsToContainer++;
				return new List<object> { policy };
			};
			SecurityConfigurator.Configure(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(TestDataFactory.ValidIsAuthenticatedFunction);
				configuration.ResolveServicesUsing(FakeIoC.GetAllInstances);
				configuration.Advanced.SetDefaultResultsCacheLifecycle(Cache.PerHttpRequest);
			});
			var context = new MockSecurityContext(runtime: SecurityConfiguration.Current.Runtime);
			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
			policyContainer.AddPolicy<LazyLoadedPolicy>();

			// Act
			policyContainer.EnforcePolicies(context);
			policyContainer.EnforcePolicies(context);

			// Assert
			Assert.That(callsToContainer, Is.EqualTo(1));
			Assert.That(policy.EnforceCallCount, Is.EqualTo(1), "Did not call enforce the expected amount of times");
		}

		[Test]
		public void Should_load_lazy_policy_with_cache_key_exactly_twice_during_execution_with_caching_off()
		{
			// Arrange
			var callsToContainer = 0;
			var policy = new LazyLoadedPolicyWithCacheKey();
			FakeIoC.GetAllInstancesProvider = () =>
			{
				callsToContainer++;
				return new List<object> { policy };
			};
			SecurityConfigurator.Configure(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(TestDataFactory.ValidIsAuthenticatedFunction);
				configuration.ResolveServicesUsing(FakeIoC.GetAllInstances);
			});
			var context = new MockSecurityContext();
			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
			policyContainer.AddPolicy<LazyLoadedPolicyWithCacheKey>();

			// Act
			policyContainer.EnforcePolicies(context);
			policyContainer.EnforcePolicies(context);

			// Assert
			Assert.That(callsToContainer, Is.EqualTo(2));
			Assert.That(policy.CacheKeyCallCount, Is.EqualTo(2), "Did not get the custom cache key the expected amount of times");
			Assert.That(policy.EnforceCallCount, Is.EqualTo(2), "Did not call enforce the expected amount of times");
		}

		[Test]
		public void Should_load_lazy_policy_with_cache_key_exactly_twice_during_execution_with_caching_on()
		{
			// Arrange
			var callsToContainer = 0;
			var policy = new LazyLoadedPolicyWithCacheKey();
			FakeIoC.GetAllInstancesProvider = () =>
			{
				callsToContainer++;
				return new List<object> { policy };
			};
			SecurityConfigurator.Configure(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(TestDataFactory.ValidIsAuthenticatedFunction);
				configuration.ResolveServicesUsing(FakeIoC.GetAllInstances);
				configuration.Advanced.SetDefaultResultsCacheLifecycle(Cache.PerHttpRequest);
			});
			var context = new MockSecurityContext(runtime: SecurityConfiguration.Current.Runtime);
			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
			policyContainer.AddPolicy<LazyLoadedPolicyWithCacheKey>();

			// Act
			policyContainer.EnforcePolicies(context);
			policyContainer.EnforcePolicies(context);

			// Assert
			Assert.That(callsToContainer, Is.EqualTo(2));
			Assert.That(policy.CacheKeyCallCount, Is.EqualTo(2), "Did not get the custom cache key the expected amount of times");
			Assert.That(policy.EnforceCallCount, Is.EqualTo(1), "Did not call enforce the expected amount of times");
		}

		[Test]
		public void Should_enforce_lazy_policy_with_cache_key_exactly_twice_during_execution_with_caching_on()
		{
			// Arrange
			var callsToContainer = 0;
			var policy = new LazyLoadedPolicyWithCacheKey();
			FakeIoC.GetAllInstancesProvider = () =>
			{
				callsToContainer++;
				return new List<object> { policy };
			};
			SecurityConfigurator.Configure(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(TestDataFactory.ValidIsAuthenticatedFunction);
				configuration.ResolveServicesUsing(FakeIoC.GetAllInstances);
				configuration.Advanced.SetDefaultResultsCacheLifecycle(Cache.PerHttpRequest);
			});
			var context = new MockSecurityContext(runtime: SecurityConfiguration.Current.Runtime);
			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
			policyContainer.AddPolicy<LazyLoadedPolicyWithCacheKey>();

			// Act
			policy.CacheKey = "101";
			policyContainer.EnforcePolicies(context);
			policyContainer.EnforcePolicies(context);
			policyContainer.EnforcePolicies(context);

			policy.CacheKey = "102";
			policyContainer.EnforcePolicies(context);
			policyContainer.EnforcePolicies(context);

			// Assert
			Assert.That(callsToContainer, Is.EqualTo(5));
			Assert.That(policy.CacheKeyCallCount, Is.EqualTo(5), "Did not get the custom cache key the expected amount of times");
			Assert.That(policy.EnforceCallCount, Is.EqualTo(2), "Did not call enforce the expected amount of times");
		}

		public class LazyLoadedPolicy : ISecurityPolicy
		{
			public int EnforceCallCount { get; private set; }

			public PolicyResult Enforce(ISecurityContext context)
			{
				EnforceCallCount++;
				return PolicyResult.CreateSuccessResult(this);
			}
		}

		public class LazyLoadedPolicyWithCacheKey : ISecurityPolicy, ICacheKeyProvider
		{
			public string CacheKey { get; set; }
			public int EnforceCallCount { get; private set; }
			public int CacheKeyCallCount { get; private set; }

			public LazyLoadedPolicyWithCacheKey()
			{
				CacheKey = "1";
			}

			public PolicyResult Enforce(ISecurityContext context)
			{
				EnforceCallCount++;
				return PolicyResult.CreateSuccessResult(this);
			}

			public string Get(ISecurityContext securityContext)
			{
				CacheKeyCallCount++;
				return CacheKey;
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
			var context = TestDataFactory.CreateSecurityContext(false);
			context.Runtime.As<SecurityRuntime>().DefaultResultsCacheLifecycle = Cache.DoNotCache;
			var firstPolicy = new IgnorePolicy();			
			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
			policyContainer.AddPolicy(firstPolicy);

			// Act

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
			context.Runtime.As<SecurityRuntime>().DefaultResultsCacheLifecycle = Cache.PerHttpRequest;
			var firstPolicy = new IgnorePolicy();
			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
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
			context.Runtime.As<SecurityRuntime>().DefaultResultsCacheLifecycle = Cache.PerHttpSession;
			var firstPolicy = new IgnorePolicy();
			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
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
		public void Should_add_policyresult_cache_strategy_for_RequireAnyRolePolicy_with_lifecycle_set_to_DoNotCache()
		{
			const Cache expectedLifecycle = Cache.DoNotCache;
			const string expectedControllerName = "Controller1";
			const string expectedActionName = "Action1";

			var policyContainer = new PolicyContainer(expectedControllerName, expectedActionName, TestDataFactory.CreateValidPolicyAppender());

			// Act
			policyContainer.Cache<RequireAnyRolePolicy>(expectedLifecycle);

			// Assert
			var policyResultCacheStrategy = policyContainer.CacheStrategies.Single();
			Assert.That(policyResultCacheStrategy.ControllerName, Is.EqualTo(expectedControllerName));
			Assert.That(policyResultCacheStrategy.ActionName, Is.EqualTo(expectedActionName));
			Assert.That(policyResultCacheStrategy.PolicyType, Is.EqualTo(typeof(RequireAnyRolePolicy)));
			Assert.That(policyResultCacheStrategy.CacheLifecycle, Is.EqualTo(expectedLifecycle));
			Assert.That(policyResultCacheStrategy.CacheLevel, Is.EqualTo(By.ControllerAction));
		}

		[Test]
		public void Should_add_policyresult_cache_strategy_for_Policy_T_with_lifecycle_set_to_DoNotCache()
		{
			const Cache expectedLifecycle = Cache.DoNotCache;
			const string expectedControllerName = "Controller1";
			const string expectedActionName = "Action1";

			var policyContainer = new PolicyContainer(expectedControllerName, expectedActionName, TestDataFactory.CreateValidPolicyAppender());

			// Act
			policyContainer.AddPolicy<RequireAnyRolePolicy>().DoNotCache();

			// Assert
			var policyResultCacheStrategy = policyContainer.CacheStrategies.Single();
			Assert.That(policyResultCacheStrategy.ControllerName, Is.EqualTo(expectedControllerName));
			Assert.That(policyResultCacheStrategy.ActionName, Is.EqualTo(expectedActionName));
			Assert.That(policyResultCacheStrategy.PolicyType, Is.EqualTo(typeof(RequireAnyRolePolicy)));
			Assert.That(policyResultCacheStrategy.CacheLifecycle, Is.EqualTo(expectedLifecycle));
			Assert.That(policyResultCacheStrategy.CacheLevel, Is.EqualTo(By.ControllerAction));
		}

		[Test]
		public void Should_add_policyresult_cache_strategy_for_RequireAnyRolePolicy_with_lifecycle_set_to_PerHttpRequest()
		{
			const Cache expectedLifecycle = Cache.PerHttpRequest;
			const string expectedControllerName = "Controller2";
			const string expectedActionName = "Action2";
			
			var policyContainer = new PolicyContainer(expectedControllerName, expectedActionName, TestDataFactory.CreateValidPolicyAppender());

			// Act
			policyContainer.Cache<RequireAnyRolePolicy>(expectedLifecycle);

			// Assert
			var policyResultCacheStrategy = policyContainer.CacheStrategies.Single();
			Assert.That(policyResultCacheStrategy.ControllerName, Is.EqualTo(expectedControllerName));
			Assert.That(policyResultCacheStrategy.ActionName, Is.EqualTo(expectedActionName));
			Assert.That(policyResultCacheStrategy.PolicyType, Is.EqualTo(typeof(RequireAnyRolePolicy)));
			Assert.That(policyResultCacheStrategy.CacheLifecycle, Is.EqualTo(expectedLifecycle));
			Assert.That(policyResultCacheStrategy.CacheLevel, Is.EqualTo(By.ControllerAction));
		}

		[Test]
		public void Should_add_policyresult_cache_strategy_for_Policy_T_with_lifecycle_set_to_PerHttpRequest()
		{
			const Cache expectedLifecycle = Cache.PerHttpRequest;
			const string expectedControllerName = "Controller2";
			const string expectedActionName = "Action2";

			var policyContainer = new PolicyContainer(expectedControllerName, expectedActionName, TestDataFactory.CreateValidPolicyAppender());

			// Act
			policyContainer.AddPolicy<RequireAnyRolePolicy>().CachePerHttpRequest();

			// Assert
			var policyResultCacheStrategy = policyContainer.CacheStrategies.Single();
			Assert.That(policyResultCacheStrategy.ControllerName, Is.EqualTo(expectedControllerName));
			Assert.That(policyResultCacheStrategy.ActionName, Is.EqualTo(expectedActionName));
			Assert.That(policyResultCacheStrategy.PolicyType, Is.EqualTo(typeof(RequireAnyRolePolicy)));
			Assert.That(policyResultCacheStrategy.CacheLifecycle, Is.EqualTo(expectedLifecycle));
			Assert.That(policyResultCacheStrategy.CacheLevel, Is.EqualTo(By.ControllerAction));
		}

		[Test]
		public void Should_add_policyresult_cache_strategy_for_RequireAnyRolePolicy_with_lifecycle_set_to_PerHttpSession()
		{
			const Cache expectedLifecycle = Cache.PerHttpSession;
			const string expectedControllerName = "Controller3";
			const string expectedActionName = "Action3";

			var policyContainer = new PolicyContainer(expectedControllerName, expectedActionName, TestDataFactory.CreateValidPolicyAppender());

			// Act
			policyContainer.Cache<RequireAnyRolePolicy>(expectedLifecycle);

			// Assert
			var policyResultCacheStrategy = policyContainer.CacheStrategies.Single();
			Assert.That(policyResultCacheStrategy.ControllerName, Is.EqualTo(expectedControllerName));
			Assert.That(policyResultCacheStrategy.ActionName, Is.EqualTo(expectedActionName));
			Assert.That(policyResultCacheStrategy.PolicyType, Is.EqualTo(typeof(RequireAnyRolePolicy)));
			Assert.That(policyResultCacheStrategy.CacheLifecycle, Is.EqualTo(expectedLifecycle));
			Assert.That(policyResultCacheStrategy.CacheLevel, Is.EqualTo(By.ControllerAction));
		}

		[Test]
		public void Should_add_policyresult_cache_strategy_for_Policy_T_with_lifecycle_set_to_PerHttpSession()
		{
			const Cache expectedLifecycle = Cache.PerHttpSession;
			const string expectedControllerName = "Controller3";
			const string expectedActionName = "Action3";

			var policyContainer = new PolicyContainer(expectedControllerName, expectedActionName, TestDataFactory.CreateValidPolicyAppender());

			// Act
			policyContainer.AddPolicy<RequireAnyRolePolicy>().CachePerHttpSession();

			// Assert
			var policyResultCacheStrategy = policyContainer.CacheStrategies.Single();
			Assert.That(policyResultCacheStrategy.ControllerName, Is.EqualTo(expectedControllerName));
			Assert.That(policyResultCacheStrategy.ActionName, Is.EqualTo(expectedActionName));
			Assert.That(policyResultCacheStrategy.PolicyType, Is.EqualTo(typeof(RequireAnyRolePolicy)));
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
			policyContainer.Cache<DenyAnonymousAccessPolicy>(expectedLifecycle);

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
				.Cache<RequireAllRolesPolicy>(Cache.PerHttpRequest)
				.Cache<RequireAnyRolePolicy>(Cache.PerHttpSession);

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
			Assert.That(strategy2.PolicyType, Is.EqualTo(typeof(RequireAnyRolePolicy)));
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
				.Cache<RequireAllRolesPolicy>(Cache.PerHttpRequest)
				.Cache<RequireAllRolesPolicy>(Cache.PerHttpSession);

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
		public void Should_add_policyresult_cache_strategy_for_RequireAnyRolePolicy_with_level_set_to_ControllerAction()
		{
			const By expectedLevel = By.ControllerAction;
			var policyContainer = new PolicyContainer("Controller", "Action", TestDataFactory.CreateValidPolicyAppender());

			// Act
			policyContainer.Cache<RequireAnyRolePolicy>(Cache.PerHttpRequest, expectedLevel);

			// Assert
			var policyResultCacheStrategy = policyContainer.CacheStrategies.Single();
			Assert.That(policyResultCacheStrategy.CacheLevel, Is.EqualTo(expectedLevel));
		}

		[Test]
		public void Should_add_policyresult_cache_strategy_for_RequireAnyRolePolicy_with_level_set_to_Controller()
		{
			const By expectedLevel = By.Controller;
			var policyContainer = new PolicyContainer("Controller", "Action", TestDataFactory.CreateValidPolicyAppender());

			// Act
			policyContainer.Cache<RequireAnyRolePolicy>(Cache.PerHttpRequest, expectedLevel);

			// Assert
			var policyResultCacheStrategy = policyContainer.CacheStrategies.Single();
			Assert.That(policyResultCacheStrategy.CacheLevel, Is.EqualTo(expectedLevel));
		}

		[Test]
		public void Should_add_policyresult_cache_strategy_for_RequireAnyRolePolicy_with_level_set_to_Policy()
		{
			const By expectedLevel = By.Policy;
			var policyContainer = new PolicyContainer("Controller", "Action", TestDataFactory.CreateValidPolicyAppender());

			// Act
			policyContainer.Cache<RequireAnyRolePolicy>(Cache.PerHttpRequest, expectedLevel);

			// Assert
			var policyResultCacheStrategy = policyContainer.CacheStrategies.Single();
			Assert.That(policyResultCacheStrategy.CacheLevel, Is.EqualTo(expectedLevel));
		}

		[Test]
		public void Should_add_policyresult_cache_strategy_for_Policy_T_with_lifecycle_set_to_DoNotCache_and_level_set_to_ControllerAction()
		{
			const By expectedLevel = By.ControllerAction;
			var policyContainer = new PolicyContainer("Controller", "Action", TestDataFactory.CreateValidPolicyAppender());

			// Act
			policyContainer.AddPolicy<RequireAnyRolePolicy>().DoNotCache(expectedLevel);

			// Assert
			var policyResultCacheStrategy = policyContainer.CacheStrategies.Single();
			Assert.That(policyResultCacheStrategy.PolicyType, Is.EqualTo(typeof(RequireAnyRolePolicy)));
			Assert.That(policyResultCacheStrategy.CacheLifecycle, Is.EqualTo(Cache.DoNotCache));
			Assert.That(policyResultCacheStrategy.CacheLevel, Is.EqualTo(expectedLevel));
		}

		[Test]
		public void Should_add_policyresult_cache_strategy_for_Policy_T_with_lifecycle_set_to_PerHttpRequest_and_level_set_to_Controller()
		{
			const By expectedLevel = By.Controller;
			var policyContainer = new PolicyContainer("Controller", "Action", TestDataFactory.CreateValidPolicyAppender());

			// Act
			policyContainer.AddPolicy<RequireAnyRolePolicy>().CachePerHttpRequest(expectedLevel);

			// Assert
			var policyResultCacheStrategy = policyContainer.CacheStrategies.Single();
			Assert.That(policyResultCacheStrategy.PolicyType, Is.EqualTo(typeof(RequireAnyRolePolicy)));
			Assert.That(policyResultCacheStrategy.CacheLifecycle, Is.EqualTo(Cache.PerHttpRequest));
			Assert.That(policyResultCacheStrategy.CacheLevel, Is.EqualTo(expectedLevel));
		}

		[Test]
		public void Should_add_policyresult_cache_strategy_for_Policy_T_with_lifecycle_set_to_PerHttpSession_and_level_set_to_Policy()
		{
			const By expectedLevel = By.Policy;
			var policyContainer = new PolicyContainer("Controller", "Action", TestDataFactory.CreateValidPolicyAppender());

			// Act
			policyContainer.AddPolicy<RequireAnyRolePolicy>().CachePerHttpSession(expectedLevel);

			// Assert
			var policyResultCacheStrategy = policyContainer.CacheStrategies.Single();
			Assert.That(policyResultCacheStrategy.PolicyType, Is.EqualTo(typeof(RequireAnyRolePolicy)));
			Assert.That(policyResultCacheStrategy.CacheLifecycle, Is.EqualTo(Cache.PerHttpSession));
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
				.Cache<RequireAllRolesPolicy>(Cache.PerHttpRequest, By.Controller)
				.Cache<RequireAllRolesPolicy>(Cache.PerHttpSession, By.Policy);

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
			policyContainer.Cache<RequireAnyRolePolicy>(Cache.PerHttpRequest);

			// Act
			policyContainer.ClearCacheStrategies();

			// Assert
			Assert.That(policyContainer.CacheStrategies.Any(), Is.False);
		}

		[Test]
		public void Should_clear_all_cache_strategies_for_policy()
		{
			var policyContainer = new PolicyContainer("Controller", "Action", TestDataFactory.CreateValidPolicyAppender());
			policyContainer.Cache<RequireAnyRolePolicy>(Cache.PerHttpRequest);
			policyContainer.Cache<RequireAllRolesPolicy>(Cache.PerHttpRequest);

			// Act
			policyContainer.ClearCacheStrategyFor<RequireAnyRolePolicy>();

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
			context.Runtime.As<SecurityRuntime>().DefaultResultsCacheLifecycle = defaultCacheLifecycle;
			var securityPolicy = new IgnorePolicy();
			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
			policyContainer.AddPolicy(securityPolicy).Cache<IgnorePolicy>(specifiedCacheLifecycle);

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