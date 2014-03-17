using FluentSecurity.Caching;
using FluentSecurity.Diagnostics;
using FluentSecurity.Internals;
using FluentSecurity.ServiceLocation;
using NUnit.Framework;

namespace FluentSecurity.Specification // Do not change the namespace
{
	[SetUpFixture]
	public class ResetFixture
	{
		[SetUp]
		public void Reset()
		{
			ServiceLocator.Reset();
			SecurityDoctor.Reset();
			SecurityCache.ClearCache(Lifecycle.HybridHttpContext);
			SecurityCache.ClearCache(Lifecycle.HybridHttpSession);
		}
	}
}