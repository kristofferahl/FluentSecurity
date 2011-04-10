using System;

namespace FluentSecurity.TestHelper
{
	public static class Settings
	{
		static Settings()
		{
			SetDefaults();
		}

		private static void SetDefaults()
		{
			DefaultExpectationViolationHandler = new DefaultExpectationViolationHandler();
			DefaultExpectationGroupBuilder = new ExpectationGroupBuilder();
			DefaultExpectationVerifyerConstructor = (configuration, handler) => new ExpectationVerifyer(configuration, handler);
		}

		public static Func<ISecurityConfiguration, IExpectationViolationHandler, IExpectationVerifyer> DefaultExpectationVerifyerConstructor { get; set; }
		public static IExpectationViolationHandler DefaultExpectationViolationHandler { get; set; }
		public static IExpectationGroupBuilder DefaultExpectationGroupBuilder { get; set; }
	}
}