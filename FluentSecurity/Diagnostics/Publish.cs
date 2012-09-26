using System;
using FluentSecurity.Diagnostics.Events;

namespace FluentSecurity.Diagnostics
{
	public static class Publish
	{
		public static void RuntimeEvent(Func<string> message, ISecurityContext context)
		{
			PublishEvent(EventListeners.RuntimeEventListener, () => new SecurityRuntimeEvent(context.Id, message.Invoke()));
		}

		public static void RuntimePolicyEvent(Func<string> message, ISecurityContext context)
		{
			PublishEvent(EventListeners.RuntimePolicyEventListener, () => new SecurityRuntimePolicyEvent(context.Id, message.Invoke()));
		}

		public static void ConfigurationEvent(Func<string> message, ISecurityContext context)
		{
			PublishEvent(EventListeners.ConfigurationEventListener, () => new SecurityConfigurationEvent(context.Id, message.Invoke()));
		}

		private static void PublishEvent<TEvent>(Action<TEvent> listener, Func<TEvent> eventBuilder) where TEvent : ISecurityEvent
		{
			if (listener == null) return;
			var @event = eventBuilder.Invoke();
			listener.Invoke(@event);
		}
	}
}