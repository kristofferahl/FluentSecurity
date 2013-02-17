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

		internal static List<Action<ISecurityEvent>> Listeners { get; private set; }

		public static void Register(Action<ISecurityEvent> eventListener)
		{
			if (Listeners == null) Listeners = new List<Action<ISecurityEvent>>();
			Listeners.Add(eventListener);
		}

		public static void Reset()
		{
			Listeners = null;
		}
	}
}