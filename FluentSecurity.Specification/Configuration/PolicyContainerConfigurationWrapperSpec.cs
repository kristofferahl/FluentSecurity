using System;
using FluentSecurity.Caching;
using FluentSecurity.Configuration;
using FluentSecurity.Policy;
using Moq;
using NUnit.Framework;

namespace FluentSecurity.Specification.Configuration
{
	[TestFixture]
	[Category("PolicyContainerConfigurationWrapperSpec")]
	public class When_performing_actions_on_PolicyContainerConfigurationWrapper
	{
		private Mock<IPolicyContainerConfiguration> _inner;
		private PolicyContainerConfigurationWrapper<IgnorePolicy> _wrapper;

		[SetUp]
		public void SetUp()
		{
			_inner = new Mock<IPolicyContainerConfiguration>();
			_wrapper = new PolicyContainerConfigurationWrapper<IgnorePolicy>(_inner.Object);
		}

		[Test]
		public void Should_add_policy_to_inner_configuration()
		{
			// Arrange
			var expectedPolicy = new IgnorePolicy();

			// Act
			_wrapper.AddPolicy(expectedPolicy);
			_wrapper.AddPolicy<IgnorePolicy>();

			// Assert
			_inner.Verify(x => x.AddPolicy(expectedPolicy), Times.Exactly(1));
			_inner.Verify(x => x.AddPolicy<IgnorePolicy>(), Times.Exactly(1));
		}

		[Test]
		public void Should_remove_policy_from_inner_configuration()
		{
			// Arrange
			Func<IgnorePolicy, bool> expectedPredicate = p => true;

			// Act
			_wrapper.RemovePolicy(expectedPredicate);

			// Assert
			_inner.Verify(x => x.RemovePolicy(expectedPredicate), Times.Exactly(1));
		}

		[Test]
		public void Should_set_cache_lifecycle_on_inner_configuration()
		{
			// Arrange
			const Cache expectedLifecycle = Cache.PerHttpRequest;
			const By expectedLevel = By.Controller;

			// Act
			_wrapper.Cache<IgnorePolicy>(expectedLifecycle);
			_wrapper.Cache<IgnorePolicy>(expectedLifecycle, expectedLevel);

			// Assert
			_inner.Verify(x => x.Cache<IgnorePolicy>(expectedLifecycle), Times.Exactly(1));
			_inner.Verify(x => x.Cache<IgnorePolicy>(expectedLifecycle, expectedLevel), Times.Exactly(1));
		}

		[Test]
		public void Should_clear_cache_strategies_on_inner_configuration()
		{
			// Act
			_wrapper.ClearCacheStrategies();
			_wrapper.ClearCacheStrategyFor<IgnorePolicy>();

			// Assert
			_inner.Verify(x => x.ClearCacheStrategies(), Times.Exactly(1));
			_inner.Verify(x => x.ClearCacheStrategyFor<IgnorePolicy>(), Times.Exactly(1));
		}
	}
}