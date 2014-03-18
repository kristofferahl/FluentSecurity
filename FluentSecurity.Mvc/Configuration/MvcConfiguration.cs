using FluentSecurity.Core;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity.Configuration
{
	public class MvcConfiguration : ConfigurationExpression, IFluentConfiguration
	{
		public MvcConfiguration()
		{
			Initialize(new SecurityRuntime());
		}

		public ISecurityRuntime GetRuntime()
		{
			return Runtime;
		}

		public IRegistry GetRegistry()
		{
			return new MvcRegistry();
		}

		public ILifecycleResolver GetLifecycleResolver()
		{
			return new MvcLifecycleResolver();
		}
	}
}