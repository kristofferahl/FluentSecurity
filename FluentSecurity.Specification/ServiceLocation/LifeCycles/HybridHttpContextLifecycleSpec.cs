using System;
using System.Collections.Generic;
using FluentSecurity.ServiceLocation.LifeCycles;
using NUnit.Framework;

namespace FluentSecurity.Specification.ServiceLocation.LifeCycles
{
	[TestFixture]
	[Category("HybridHttpContextLifecycleSpec")]
	public class When_getting_the_cache_from_HybridHttpContextLifecycle
	{
		[Test]
		public void Should_return_cache_when_context_is_not_available()
		{
			// Arrange
			var lifecycle = new HybridHttpContextLifecycle();
			HttpContextLifecycle.HasContextResolver = () => false;

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
			var lifecycle = new HybridHttpContextLifecycle();
			HttpContextLifecycle.HasContextResolver = () => true;
			HttpContextLifecycle.DictionaryResolver = () => dictionary;

			// Act
			var result = lifecycle.FindCache();

			// Assert
			Assert.That(result, Is.Not.Null);
		}
	}
}