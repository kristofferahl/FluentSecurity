using System;
using System.Linq;
using FluentSecurity.Caching;
using FluentSecurity.Configuration;
using FluentSecurity.Core;

namespace FluentSecurity
{
	public class SecurityRuntime : SecurityRuntimeBase
	{
		public SecurityRuntime(ISecurityCache cache, ITypeFactory typeFactory) : base(cache, typeFactory) {}

		public void ApplyConfiguration(Action<ViolationConfiguration> violationConfiguration)
		{
			if (violationConfiguration == null) throw new ArgumentNullException("violationConfiguration");
			var conventionsConfiguration = new ConventionConfiguration(_conventions);
			var configuration = new ViolationConfiguration(conventionsConfiguration);
			violationConfiguration.Invoke(configuration);
		}
	}
}