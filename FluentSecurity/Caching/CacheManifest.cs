using System;

namespace FluentSecurity.Caching
{
	public class CacheManifest
	{
		public CacheManifest(string controllerName, string actionName, Type policyType, Cache cacheLifecycle)
		{
			ControllerName = controllerName;
			ActionName = actionName;
			PolicyType = policyType;
			CacheLifecycle = cacheLifecycle;
		}

		public string ControllerName { get; private set; }
		public string ActionName { get; private set; }
		public Type PolicyType { get; private set; }
		public Cache CacheLifecycle { get; private set; }
	}
}