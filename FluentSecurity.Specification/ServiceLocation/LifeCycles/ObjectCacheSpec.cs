using System;
using FluentSecurity.ServiceLocation.LifeCycles;
using Moq;
using NUnit.Framework;

namespace FluentSecurity.Specification.ServiceLocation.LifeCycles
{
	[TestFixture]
	[Category("ObjectCacheSpec")]
	public class When_setting_an_object_in_the_ObjectCache
	{
		[Test]
		public void Should_not_add_to_cache_when_instance_is_null()
		{
			// Arrange
			var key = Guid.NewGuid();
			var objectCache = new ObjectCache();

			// Act
			objectCache.Set(key, null);

			// Assert
			Assert.That(objectCache.Count, Is.EqualTo(0));
		}

		[Test]
		public void Should_throw_when_key_is_not_unique()
		{
			// Arrange
			var key = Guid.NewGuid();
			var objectCache = new ObjectCache();
			var obj1 = new object();
			var obj2 = new object();

			objectCache.Set(key, obj1);

			// Act & assert
			Assert.Throws<ArgumentException>(() => objectCache.Set(key, obj2));
		}

		[Test]
		public void Should_dispose_IDisposable_instances_when_cleared()
		{
			// Arrange
			var key = Guid.NewGuid();
			
			var disposable = new Mock<IDisposable>();
			disposable.Setup(x => x.Dispose());

			var objectCache = new ObjectCache();
			objectCache.Set(key, disposable.Object);

			// Act
			objectCache.Clear();

			// Assert
			Assert.That(objectCache.Count, Is.EqualTo(0));
			disposable.Verify(x => x.Dispose(), Times.Exactly(1));
		}
	}
}