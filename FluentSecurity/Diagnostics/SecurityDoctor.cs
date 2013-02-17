using System;
using System.Collections.Generic;
using FluentSecurity.Diagnostics.Events;

namespace FluentSecurity.Diagnostics
{
	public static class SecurityDoctor
	{
		static SecurityDoctor()
		{
			Reset();
		}

		internal static IList<ISecurityEventListener> Listeners { get; private set; }

		public static void Register(Action<ISecurityEvent> eventListener)
		{
			Register(new AnonymousSecurityEventListener(eventListener));
		}

		public static void Register(ISecurityEventListener eventListener)
		{
			if (Listeners == null) Listeners = new List<ISecurityEventListener>();
			Listeners.Add(eventListener);
		}

		public static void Reset()
		{
			Listeners = null;
		}
	}
}