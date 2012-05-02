using System.Collections.Generic;
using FluentSecurity.ServiceLocation.LifeCycles;
using NUnit.Framework;

namespace FluentSecurity.Specification.ServiceLocation.LifeCycles
{
	[TestFixture]
	[Category("HybridHttpSessionLifecycleSpec")]
	public class When_getting_the_cache_from_HybridHttpSessionLifecycle
	{
		[Test]
		public void Should_return_cache_when_session_is_not_available()
		{
			// Arrange
			var lifecycle = new HybridHttpSessionLifecycle();
			HttpSessionLifecycle.HasSessionResolver = () => false;

			// Act
			var result = lifecycle.FindCache();

			// Assert
			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public void Should_return_cache_when_context_is_available()
		{
			// Arrange
			var dictionary = new Dictionary<object, object>();
			var lifecycle = new HybridHttpSessionLifecycle();
			HttpSessionLifecycle.HasSessionResolver = () => true;
			HttpSessionLifecycle.DictionaryResolver = () => dictionary;

			// Act
			var result = lifecycle.FindCache();

			// Assert
			Assert.That(result, Is.Not.Null);
		}
	}
}