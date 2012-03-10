using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using FluentSecurity.ServiceLocation.LifeCycles;
using Moq;
using NUnit.Framework;

namespace FluentSecurity.Specification.ServiceLocation.LifeCycles
{
	[TestFixture]
	[Category("HttpSessionLifecycleSpec")]
	public class When_checking_if_session_is_available_for_HttpSessionLifecycle
	{
		[Test]
		public void Should_return_false_when_not_available()
		{
			// Arrange
			var lifecycle = new HttpSessionLifecycle(); // Will reset to original state

			// Act
			var result = lifecycle.HasSession();

			// Assert
			Assert.That(result, Is.False);
		}
	}

	[TestFixture]
	[Category("HttpSessionLifecycleSpec")]
	public class When_getting_the_cache_from_HttpSessionLifecycle
	{
		[Test]
		public void Should_throw_when_session_is_not_available()
		{
			// Arrange
			var lifecycle = new HttpSessionLifecycle();
			HttpSessionLifecycle.HasSessionResolver = () => false;

			// Act & assert
			Assert.Throws<InvalidOperationException>(() => lifecycle.FindCache());
		}

		[Test]
		public void Should_return_new_cache_when_session_is_available()
		{
			// Arrange
			var dictionary = new Dictionary<string, object>();
			var lifecycle = new HttpSessionLifecycle();
			HttpSessionLifecycle.HasSessionResolver = () => true;
			HttpSessionLifecycle.HttpSessionStateResolver = () => new TestHttpSessionState(dictionary);

			// Act
			var result = lifecycle.FindCache() as ObjectCache;

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Count, Is.EqualTo(0));
		}

		[Test]
		public void Should_return_existing_cache_when_session_is_available()
		{
			// Arrange
			var cache = new ObjectCache();
			cache.Set(Guid.NewGuid(), new object());

			var dictionary = new Dictionary<string, object>();
			dictionary.Add(HttpContextLifecycle.CacheKey, cache);

			var lifecycle = new HttpSessionLifecycle();
			HttpSessionLifecycle.HasSessionResolver = () => true;
			HttpSessionLifecycle.HttpSessionStateResolver = () => new TestHttpSessionState(dictionary);

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
			var lifecycle = new HttpSessionLifecycle();
			HttpSessionLifecycle.HasSessionResolver = () => true;

			// Act
			var exception = Assert.Throws<NullReferenceException>(() => lifecycle.FindCache());

			// Assert
			Assert.That(exception.Message, Is.EqualTo("Object reference not set to an instance of an object."));
		}

		public class TestHttpSessionState : HttpSessionStateBase
		{
			private static readonly object Locker = new object();
			private readonly Dictionary<string, object> _sessionStorage;

			public TestHttpSessionState(Dictionary<string, object> dictionary)
			{
				_sessionStorage = dictionary;
			}

			public override object this[string name]
			{
				get
				{
					return _sessionStorage.ContainsKey(name) ? _sessionStorage[name] : null;
				}
				set
				{
					_sessionStorage[name] = value;
				}
			}

			public override void Add(string name, object value)
			{
				_sessionStorage[name] = value;
			}

			public override object SyncRoot
			{
				get
				{
					return Locker;
				}
			}
		}
	}
}