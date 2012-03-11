namespace FluentSecurity.ServiceLocation.LifeCycles
{
	internal class HybridHttpContextLifecycle : ILifecycle
	{
		private readonly ILifecycle _http;
		private readonly ILifecycle _nonHttp;

		public HybridHttpContextLifecycle()
		{
			_http = new HttpContextLifecycle();
			_nonHttp = new ThreadLocalStorageLifecycle();
		}

		public IObjectCache FindCache()
		{
			return HttpContextLifecycle.HasContext()
				? _http.FindCache()
				: _nonHttp.FindCache();
		}
	}
}