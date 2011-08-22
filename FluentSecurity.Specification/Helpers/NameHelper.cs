namespace FluentSecurity.Specification.Helpers
{
	internal static class NameHelper<TController>
	{
		public static string Controller()
		{
			return typeof (TController).FullName;
		}
	}
}