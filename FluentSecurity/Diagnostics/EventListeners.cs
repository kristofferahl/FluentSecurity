using System;
using FluentSecurity.Diagnostics.Events;

namespace FluentSecurity.Diagnostics
{
	public static class EventListeners
	{
		static EventListeners()
		{
			Reset();
		}

		public static Action<SecurityRuntimeEvent> RuntimeEventListener;
		public static Action<SecurityRuntimePolicyEvent> RuntimePolicyEventListener;
		public static Action<SecurityConfigurationEvent> ConfigurationEventListener;

		public static void Reset()
		{
			RuntimeEventListener = null;
			RuntimePolicyEventListener = null;
			ConfigurationEventListener = null;
		}
	}
}