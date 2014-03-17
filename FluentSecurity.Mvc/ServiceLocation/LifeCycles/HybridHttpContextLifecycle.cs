namespace FluentSecurity.ServiceLocation.LifeCycles
{
	internal class HybridHttpContextLifecycle : ILifecycle
	{
		private readonly HttpContextLifecycle _http;
		private readonly ThreadLocalStorageLifecycle _nonHttp;

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