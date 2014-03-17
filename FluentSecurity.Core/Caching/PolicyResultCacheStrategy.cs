using System;

namespace FluentSecurity.Caching
{
	public class PolicyResultCacheStrategy
	{
		public PolicyResultCacheStrategy(string controllerName, string actionName, Type policyType, Cache cacheLifecycle, By cacheLevel = By.ControllerAction)
		{
			ControllerName = controllerName;
			ActionName = actionName;
			PolicyType = policyType;
			CacheLifecycle = cacheLifecycle;
			CacheLevel = cacheLevel;
		}

		public string ControllerName { get; private set; }
		public string ActionName { get; private set; }
		public Type PolicyType { get; private set; }
		public Cache CacheLifecycle { get; private set; }
		public By CacheLevel { get; private set; }
	}
}