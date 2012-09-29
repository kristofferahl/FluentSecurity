using System;
using System.Diagnostics;
using FluentSecurity.Diagnostics.Events;

namespace FluentSecurity.Diagnostics
{
	public static class Publish
	{
		public static void RuntimeEvent(Func<string> message, ISecurityContext context)
		{
			PublishEvent(() => new RuntimeEvent(context.Id, message.Invoke()));
		}

		public static TResult RuntimeEvent<TResult>(Func<TResult> action, Func<TResult, string> message, ISecurityContext context)
		{
			return PublishEventWithTiming(action, result => new RuntimeEvent(context.Id, message.Invoke(result)));
		}

		public static void RuntimePolicyEvent(Func<string> message, ISecurityContext context)
		{
			PublishEvent(() => new RuntimePolicyEvent(context.Id, message.Invoke()));
		}

		public static TResult RuntimePolicyEvent<TResult>(Func<TResult> action, Func<TResult, string> message, ISecurityContext context)
		{
			return PublishEventWithTiming(action, result => new RuntimePolicyEvent(context.Id, message.Invoke(result)));
		}

		public static void ConfigurationEvent(Func<string> message, ISecurityContext context)
		{
			PublishEvent(() => new ConfigurationEvent(context.Id, message.Invoke()));
		}

		private static void PublishEvent<TEvent>(Func<TEvent> eventBuilder) where TEvent : ISecurityEvent
		{
			var listener = EventListeners.Current;
			if (listener == null) return;
			var @event = eventBuilder.Invoke();
			listener.Invoke(@event);
		}

		private static TResult PublishEventWithTiming<TEvent, TResult>(Func<TResult> action, Func<TResult, TEvent> eventBuilder) where TEvent : SecurityEvent
		{
			var listener = EventListeners.Current;
			if (listener == null) return action.Invoke();

			var stopwatch = new Stopwatch();
			
			stopwatch.Start();
			var result = action.Invoke();
			stopwatch.Stop();

			var @event = eventBuilder.Invoke(result);
			@event.CompletedInMilliseconds = stopwatch.ElapsedMilliseconds;
			listener.Invoke(@event);

			return result;
		}
	}
}