using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Configuration;
using FluentSecurity.Policy;
using FluentSecurity.Policy.Contexts;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy
{
	[TestFixture]
	[Category("LazySecurityPolicySpec")]
	public class When_creating_a_LazySecurityPolicy
	{
		[Test]
		public void Should_expose_the_actual_type()
		{
			Assert.That(new LazySecurityPolicy<IgnorePolicy>().PolicyType, Is.EqualTo(typeof(IgnorePolicy)));
			Assert.That(new LazySecurityPolicy<DenyAnonymousAccessPolicy>().PolicyType, Is.EqualTo(typeof(DenyAnonymousAccessPolicy)));
		}
	}

	[TestFixture]
	[Category("LazySecurityPolicySpec")]
	public class When_loading_a_LazySecurityPolicy
	{
		[Test]
		public void Should_handle_loading_policy_with_empty_constructor()
		{
			// Arrange
			SecurityConfigurator.Configure<MvcConfiguration>(configuration => {});
			var lazySecurityPolicy = new LazySecurityPolicy<PolicyWithEmptyConstructor>();

			// Act
			var policy = lazySecurityPolicy.Load();
			
			// Assert
			Assert.That(policy, Is.TypeOf<PolicyWithEmptyConstructor>());
		}

		[Test]
		public void Should_handle_loading_policy_with_empty_constructor_based_on_SecurityPolicyBase()
		{
			// Arrange
			SecurityConfigurator.Configure<MvcConfiguration>(configuraiton => {});
			var lazySecurityPolicy = new LazySecurityPolicy<PolicyWithBaseClass>();

			// Act
			var policy = lazySecurityPolicy.Load();

			// Assert
			Assert.That(policy, Is.TypeOf<PolicyWithBaseClass>());
		}

		[Test]
		public void Should_handle_loading_policy_from_container()
		{
			// Arrange
			var expectedPolicy = new PolicyWithConstructorArguments("arg1", "arg2", "arg3");
			FakeIoC.GetAllInstancesProvider = () => new List<object> { expectedPolicy };
			SecurityConfigurator.Configure<MvcConfiguration>(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(() => true);
				configuration.ResolveServicesUsing(FakeIoC.GetAllInstances);
			});
			var lazySecurityPolicy = new LazySecurityPolicy<PolicyWithConstructorArguments>();

			// Act
			var policy = lazySecurityPolicy.Load();

			// Assert
			Assert.That(policy, Is.EqualTo(expectedPolicy));
		}

		[Test]
		public void Should_return_null_when_loading_policy_with_constructor_arguments()
		{
			// Arrange
			SecurityConfigurator.Configure<MvcConfiguration>(configuration => {});
			var lazySecurityPolicy = new LazySecurityPolicy<PolicyWithConstructorArguments>();

			// Act
			var policy = lazySecurityPolicy.Load();

			// Assert
			Assert.That(policy, Is.Null);
		}

		[Test]
		public void Should_return_null_when_no_policy_is_returned_by_service_locator()
		{
			// Arrange
			SecurityConfigurator.Configure<MvcConfiguration>(configuration => configuration.ResolveServicesUsing(t => Enumerable.Empty<object>()));
			var lazySecurityPolicy = new LazySecurityPolicy<PolicyWithConstructorArguments>();

			// Act
			var policy = lazySecurityPolicy.Load();

			// Assert
			Assert.That(policy, Is.Null);
		}
	}

	[TestFixture]
	[Category("LazySecurityPolicySpec")]
	public class When_enforcing_a_LazySecurityPolicy
	{
		[SetUp]
		public void SetUp()
		{
			SecurityConfigurator.Configure<MvcConfiguration>(configuration => {});
		}

		[Test]
		public void Should_throw_when_no_policy_was_loaded()
		{
			// Arrange
			var lazySecurityPolicy = new LazySecurityPolicy<PolicyWithConstructorArguments>();
			var context = new MockSecurityContext();

			// Act & assert
			var exception = Assert.Throws<InvalidOperationException>(() => lazySecurityPolicy.Enforce(context));
			Assert.That(exception.Message, Is.EqualTo("A policy of type FluentSecurity.Specification.Policy.PolicyWithConstructorArguments could not be loaded! Make sure the policy has an empty constructor or is registered in your IoC-container."));
		}

		[Test]
		public void Should_load_and_enforce_policy_with_failed_result()
		{
			// Arrange
			var lazySecurityPolicy = new LazySecurityPolicy<PolicyWithEmptyConstructor>();
			var context = new MockSecurityContext(isAuthenticated: false);

			// Act
			var result = lazySecurityPolicy.Enforce(context);

			// Assert
			Assert.That(result.PolicyType, Is.EqualTo(typeof(PolicyWithEmptyConstructor)));
			Assert.That(result.ViolationOccured, Is.True);
		}

		[Test]
		public void Should_load_and_enforce_policy_with_success_result()
		{
			// Arrange
			SecurityConfigurator.Configure<MvcConfiguration>(configuraiton => {});
			var lazySecurityPolicy = new LazySecurityPolicy<PolicyWithBaseClass>();
			var context = new MockSecurityContext(isAuthenticated: true);

			// Act
			var result = lazySecurityPolicy.Enforce(context);

			// Assert
			Assert.That(result.PolicyType, Is.EqualTo(typeof(PolicyWithBaseClass)));
			Assert.That(result.ViolationOccured, Is.False);
		}
	}

	public class PolicyWithEmptyConstructor : BasePolicy {}

	public class PolicyWithConstructorArguments : BasePolicy
	{
		public PolicyWithConstructorArguments(string arg1, string arg2, string arg3) {}
	}

	public class PolicyWithBaseClass : SecurityPolicyBase<MvcSecurityContext>
	{
		public override PolicyResult Enforce(MvcSecurityContext context)
		{
			return context.CurrentUserIsAuthenticated()
				? PolicyResult.CreateSuccessResult(this)
				: PolicyResult.CreateFailureResult(this, "Access denied");
		}
	}

	public abstract class BasePolicy : ISecurityPolicy
	{
		public PolicyResult Enforce(ISecurityContext context)
		{
			return context.CurrentUserIsAuthenticated()
				? PolicyResult.CreateSuccessResult(this)
				: PolicyResult.CreateFailureResult(this, "Access denied");
		}
	}
}