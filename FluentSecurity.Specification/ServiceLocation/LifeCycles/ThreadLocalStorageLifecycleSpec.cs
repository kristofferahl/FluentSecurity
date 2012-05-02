using System.Threading;
using System.Threading.Tasks;
using FluentSecurity.ServiceLocation.LifeCycles;
using NUnit.Framework;

namespace FluentSecurity.Specification.ServiceLocation.LifeCycles
{
	[TestFixture]
	[Category("ThreadLocalStorageLifecycleSpec")]
	public class When_getting_the_cache_from_ThreadLocalStorageLifecycle
	{
		private ThreadLocalStorageLifecycle _lifecycle;
		private IObjectCache _cache1;
		private IObjectCache _cache2;
		private IObjectCache _cache3;

		[SetUp]
		public void SetUp()
		{
			_lifecycle = new ThreadLocalStorageLifecycle();
		}

		private void findCache1()
		{
			_cache1 = _lifecycle.FindCache();
			var cache1 = _lifecycle.FindCache();
			
			Assert.AreSame(_cache1, cache1);
		}

		private void findCache2()
		{
			_cache2 = _lifecycle.FindCache();
			var cache2 = _lifecycle.FindCache();

			Assert.AreSame(_cache2, cache2);
		}

		private void findCache3()
		{
			_cache3 = _lifecycle.FindCache();

			Assert.AreSame(_cache3, _lifecycle.FindCache());
			Assert.AreSame(_cache3, _lifecycle.FindCache());
			Assert.AreSame(_cache3, _lifecycle.FindCache());
		}
		
		[Test]
		public void Should_get_different_caches_for_each_thread()
		{
			var t1 = new Thread(findCache1);
			var t2 = new Thread(findCache2);
			var t3 = new Thread(findCache3);

			t1.Start();
			t2.Start();
			t3.Start();

			t1.Join();
			t2.Join();
			t3.Join();

			Assert.AreNotSame(_cache1, _cache2);
			Assert.AreNotSame(_cache1, _cache3);
			Assert.AreNotSame(_cache2, _cache3);
		}
	}
}