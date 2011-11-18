namespace FluentSecurity
{
	public static class PolicyExecutionMode
	{
		static PolicyExecutionMode()
		{
			ShouldStopOnFirstViolation = false;
		}

		internal static bool ShouldStopOnFirstViolation { get; private set; }

		public static void StopOnFirstViolation(bool value)
		{
			ShouldStopOnFirstViolation = value;
		}
	}
}