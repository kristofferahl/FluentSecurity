using System;
using FluentSecurity.Caching;
using FluentSecurity.Configuration;
using FluentSecurity.Core;

namespace FluentSecurity
{
	public class AdvancedConfigurationBase<TSecurityRuntime> where TSecurityRuntime : SecurityRuntimeBase
	{
		protected readonly TSecurityRuntime Runtime;

		protected internal AdvancedConfigurationBase(TSecurityRuntime runtime)
		{
			if (runtime == null) throw new ArgumentNullException("runtime");
			Runtime = runtime;
		}

		public void IgnoreMissingConfiguration()
		{
			Runtime.ShouldIgnoreMissingConfiguration = true;
		}

		public void ModifySecurityContext(Action<ISecurityContext> modifyer)
		{
			Runtime.SecurityContextModifyer = modifyer;
		}

		public void SetDefaultResultsCacheLifecycle(Cache lifecycle)
		{
			Runtime.DefaultResultsCacheLifecycle = lifecycle;
		}

		public void Conventions(Action<ConventionConfiguration> conventions)
		{
			if (conventions == null) throw new ArgumentNullException("conventions");
			Runtime.ApplyConfiguration(conventions);
		}
	}
}