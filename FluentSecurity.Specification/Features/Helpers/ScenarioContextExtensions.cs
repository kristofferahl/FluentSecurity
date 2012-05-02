using TechTalk.SpecFlow;

namespace FluentSecurity.Specification.Features.Helpers
{
	public static class ScenarioContextExtensions
	{
		public static Givens<T> Givens<T>(this ScenarioContext scenario) where T : class
		{
			Givens<T> givens;
			ScenarioContext.Current.TryGetValue(out givens);
			if (givens == null)
			{
				givens = new Givens<T>();
				ScenarioContext.Current.Set(givens);
			}
			return givens;
		}
	}
}