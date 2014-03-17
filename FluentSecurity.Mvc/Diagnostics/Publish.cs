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

		public static void ConfigurationEvent(Func<string> message)
		{
			PublishEvent(() => new ConfigurationEvent(SecurityConfigurator.CorrelationId, message.Invoke()));
		}

		public static TResult ConfigurationEvent<TResult>(Func<TResult> action, Func<TResult, string> message)
		{
			return PublishEventWithTiming(action, result => new ConfigurationEvent(SecurityConfigurator.CorrelationId, message.Invoke(result)));
		}

		private static void PublishEvent<TEvent>(Func<TEvent> eventBuilder) where TEvent : ISecurityEvent
		{
			var listeners = SecurityDoctor.Current.Listeners;
			if (listeners == null) return;
			
			var @event = eventBuilder.Invoke();
			
			foreach (var listener in listeners)
				listener.Handle(@event);
		}

		private static TResult PublishEventWithTiming<TEvent, TResult>(Func<TResult> action, Func<TResult, TEvent> eventBuilder) where TEvent : SecurityEvent
		{
			var listeners = SecurityDoctor.Current.Listeners;
			if (listeners == null) return action.Invoke();

			var stopwatch = new Stopwatch();
			
			stopwatch.Start();
			var result = action.Invoke();
			stopwatch.Stop();

			var @event = eventBuilder.Invoke(result);
			@event.CompletedInMilliseconds = stopwatch.ElapsedMilliseconds;

			foreach (var listener in listeners)
				listener.Handle(@event);

			return result;
		}
	}
}