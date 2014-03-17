using FluentSecurity.Core;

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
	}
}