using System;
using FluentSecurity.Caching;

namespace FluentSecurity.Configuration
{
	public class AdvancedConfiguration : IAdvancedConfiguration
	{
		internal AdvancedConfiguration()
		{
			SetDefaultResultsCacheLifecycle(Cache.DoNotCache);
		}

		public Cache DefaultResultsCacheLifecycle { get; private set; }
		public Action<ISecurityContext> SecurityContextModifyer { get; private set; }

		public void SetDefaultResultsCacheLifecycle(Cache lifecycle)
		{
			DefaultResultsCacheLifecycle = lifecycle;
		}

		public void ModifySecurityContext(Action<ISecurityContext> modifyer)
		{
			SecurityContextModifyer = modifyer;
		}
	}
}