using System;
using System.Collections.Generic;
using System.Web;
using FluentSecurity.ServiceLocation.LifeCycles;
using NUnit.Framework;

namespace FluentSecurity.Specification.ServiceLocation.LifeCycles
{
	[TestFixture]
	[Category("HttpContextLifecycleSpec")]
	public class When_checking_if_HttpContext_is_available_to_HttpContextLifecycle
	{
		[Test]
		public void Should_return_false_when_not_available()
		{
			// Arrange
			HttpContext.Current = null;
			new HttpContextLifecycle(); // Will reset to original state

			// Act
			var result = HttpContextLifecycle.HasContext();

			// Assert
			Assert.That(result, Is.False);
		}
	}

	[TestFixture]
	[Category("HttpContextLifecycleSpec")]
	public class When_getting_the_cache_from_HttpContextLifecycle
	{
		[Test]
		public void Should_throw_when_context_is_not_available()
		{
			// Arrange
			var lifecycle = new HttpContextLifecycle();
			HttpContextLifecycle.HasContextResolver = () => false;

			// Act & assert
			Assert.Throws<InvalidOperationException>(() => lifecycle.FindCache());
		}

		[Test]
		public void Should_return_new_cache_when_context_is_available()
		{
			// Arrange
			var dictionary = new Dictionary<object, object>();
			var lifecycle = new HttpContextLifecycle();
			HttpContextLifecycle.HasContextResolver = () => true;
			HttpContextLifecycle.DictionaryResolver = () => dictionary;

			// Act
			var result = lifecycle.FindCache() as ObjectCache;
			
			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Count, Is.EqualTo(0));
		}

		[Test]
		public void Should_return_existing_cache_when_context_is_available()
		{
			// Arrange
			var cache = new ObjectCache();
			cache.Set(Guid.NewGuid(), new object());
			
			var dictionary = new Dictionary<object, object>();
			dictionary.Add(HttpContextLifecycle.CacheKey, cache);
			
			var lifecycle = new HttpContextLifecycle();
			HttpContextLifecycle.HasContextResolver = () => true;
			HttpContextLifecycle.DictionaryResolver = () => dictionary;

			// Act
			var result = lifecycle.FindCache() as ObjectCache;

			// Assert
			Assert.That(result, Is.EqualTo(cache));
			Assert.That(result.Count, Is.EqualTo(1));
		}

		[Test]
		public void Should_try_to_get_cache_from_context_when_context_is_available()
		{
			// Arrange
			var lifecycle = new HttpContextLifecycle();
			HttpContextLifecycle.HasContextResolver = () => true;

			// Act
			var exception = Assert.Throws<NullReferenceException>(() => lifecycle.FindCache());

			// Assert
			Assert.That(exception.Message, Is.EqualTo("Object reference not set to an instance of an object."));
		}
	}
}