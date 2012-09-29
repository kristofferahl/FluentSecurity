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

		internal static Action<ISecurityEvent> Current;

		public static void Register(Action<ISecurityEvent> eventListener)
		{
			Current = eventListener;
		}

		public static void Reset()
		{
			Current = null;
		}
	}
}