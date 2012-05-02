using FluentSecurity.ServiceLocation;
using FluentSecurity.ServiceLocation.LifeCycles;
using NUnit.Framework;

namespace FluentSecurity.Specification.ServiceLocation
{
	[TestFixture]
	[Category("LifecycleExtensionsSpec")]
	public class When_getting_the_lifecycle
	{
		[Test]
		public void Should_get_transient_lifecycle()
		{
			// Arrange
			const Lifecycle lifecycle = Lifecycle.Transient;

			// Act
			var result = lifecycle.Get();

			// Assert
			Assert.That(result, Is.TypeOf(typeof(TransientLifecycle)));
		}

		[Test]
		public void Should_get_singleton_lifecycle()
		{
			// Arrange
			const Lifecycle lifecycle = Lifecycle.Singleton;

			// Act
			var result = lifecycle.Get();

			// Assert
			Assert.That(result, Is.TypeOf(typeof(SingletonLifecycle)));
		}

		[Test]
		public void Should_get_hybrid_http_context_lifecycle()
		{
			// Arrange
			const Lifecycle lifecycle = Lifecycle.HybridHttpContext;

			// Act
			var result = lifecycle.Get();

			// Assert
			Assert.That(result, Is.TypeOf(typeof(HybridHttpContextLifecycle)));
		}

		[Test]
		public void Should_get_hybrid_http_session_lifecycle()
		{
			// Arrange
			const Lifecycle lifecycle = Lifecycle.HybridHttpSession;

			// Act
			var result = lifecycle.Get();

			// Assert
			Assert.That(result, Is.TypeOf(typeof(HybridHttpSessionLifecycle)));
		}

		[Test]
		public void Should_always_return_the_same_instance_of_each_lifecycle()
		{
			// Assert
			Assert.That(Lifecycle.Transient.Get(), Is.EqualTo(Lifecycle.Transient.Get()));
			Assert.That(Lifecycle.Singleton.Get(), Is.EqualTo(Lifecycle.Singleton.Get()));
			Assert.That(Lifecycle.HybridHttpContext.Get(), Is.EqualTo(Lifecycle.HybridHttpContext.Get()));
			Assert.That(Lifecycle.HybridHttpSession.Get(), Is.EqualTo(Lifecycle.HybridHttpSession.Get()));
		}
	}
}