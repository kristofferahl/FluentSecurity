namespace FluentSecurity.Specification.Helpers
{
	public static class TestingExtensions
	{
		public static T As<T>(this object obj) where T : class
		{
			return obj as T;
		}
	}
}