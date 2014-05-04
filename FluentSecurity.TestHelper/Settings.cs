using System;
using FluentSecurity.Core;

namespace FluentSecurity.TestHelper
{
	public static class Settings
	{
		static Settings()
		{
			DefaultExpectationViolationHandler = configuration => new DefaultExpectationViolationHandler();
			DefaultExpectationGroupBuilder = configuration => new ExpectationGroupBuilder(configuration.ServiceLocator.Resolve<IActionResolver>());
			DefaultExpectationVerifyer = (configuration, handler) => new ExpectationVerifyer(configuration, handler);
		}

		public static Func<ISecurityConfiguration, IExpectationViolationHandler, IExpectationVerifyer> DefaultExpectationVerifyer { get; set; }
		public static Func<ISecurityConfiguration, IExpectationViolationHandler> DefaultExpectationViolationHandler { get; set; }
		public static Func<ISecurityConfiguration, IExpectationGroupBuilder> DefaultExpectationGroupBuilder { get; set; }
	}
}