using FluentSecurity.Caching;
using FluentSecurity.Diagnostics;
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
			SecurityDoctor.Reset();
			new SecurityCache(new MvcLifecycleResolver()).Clear(Lifecycle.HybridHttpContext);
			new SecurityCache(new MvcLifecycleResolver()).Clear(Lifecycle.HybridHttpSession);
		}
	}
}