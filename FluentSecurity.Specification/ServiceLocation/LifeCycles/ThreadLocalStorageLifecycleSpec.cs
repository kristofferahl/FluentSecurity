using System.Threading.Tasks;
using FluentSecurity.ServiceLocation.LifeCycles;
using NUnit.Framework;

namespace FluentSecurity.Specification.ServiceLocation.LifeCycles
{
	[TestFixture]
	[Category("ThreadLocalStorageLifecycleSpec")]
	public class When_getting_the_cache_from_ThreadLocalStorageLifecycle
	{
		[Test]
		public void Should_get_same_cache_while_on_the_same_thread()
		{
			// Act
			var cache1 = new ThreadLocalStorageLifecycle().FindCache();
			var cache2 = new ThreadLocalStorageLifecycle().FindCache();

			// Assert
			Assert.That(cache1, Is.EqualTo(cache2));
		}

		[Test]
		public void Should_get_different_caches_for_2_threads()
		{
			// Arrange & act
			var task1 = new Task<IObjectCache>(() => new ThreadLocalStorageLifecycle().FindCache());
			task1.Start();

			var task2 = new Task<IObjectCache>(() => new ThreadLocalStorageLifecycle().FindCache());
			task2.Start();

			Task.WaitAll(task1, task2);

			var cache1 = task1.Result;
			var cache2 = task2.Result;

			// Assert
			Assert.That(cache1, Is.Not.EqualTo(cache2));
		}
	}
}