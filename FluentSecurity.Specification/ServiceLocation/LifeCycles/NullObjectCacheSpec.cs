using FluentSecurity.ServiceLocation.LifeCycles;
using NUnit.Framework;

namespace FluentSecurity.Specification.ServiceLocation.LifeCycles
{
	[TestFixture]
	[Category("NullObjectCacheSpec")]
	public class When_using_NullObjectCache
	{
		[Test]
		public void Shoul_always_return_null()
		{
			// Arrange
			var nullObjectCache = new NullObjectCache();
			nullObjectCache.Clear();

			// Act
			nullObjectCache.Set(null, null);
			nullObjectCache.Set("", "");

			// Assert
			Assert.That(nullObjectCache.Get(null), Is.Null);
			Assert.That(nullObjectCache.Get(""), Is.Null);
		}
	}
}