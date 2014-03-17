using FluentSecurity.ServiceLocation.LifeCycles;

namespace FluentSecurity.ServiceLocation
{
	public enum Lifecycle
	{
		Transient,
		Singleton,
		HybridHttpContext,
		HybridHttpSession
	}

	internal static class Lifecycle<TLifecycle> where TLifecycle : ILifecycle, new()
	{
		static Lifecycle()
		{
			Instance = new TLifecycle();
		}

		public static ILifecycle Instance { get; private set; }
	}
}