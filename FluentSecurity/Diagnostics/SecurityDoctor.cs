using System;
using System.Collections.Generic;
using FluentSecurity.Diagnostics.Events;
using FluentSecurity.Scanning;

namespace FluentSecurity.Diagnostics
{
	public static class SecurityDoctor
	{
		static SecurityDoctor()
		{
			Reset();
		}

		public static void ScanForEventListeners()
		{
			var assemblyScanner = new AssemblyScanner();
			assemblyScanner.AssembliesFromApplicationBaseDirectory();
			assemblyScanner.With(new SecurityEventListenerScanner(IgnoreTypeLoadExceptions));
			var eventListeners = assemblyScanner.Scan();

			foreach (var eventListenerType in eventListeners)
			{
				var eventListener = (ISecurityEventListener) Activator.CreateInstance(eventListenerType);
				Register(eventListener);
			}
		}

		public static bool IgnoreTypeLoadExceptions { get; set; }
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
			IgnoreTypeLoadExceptions = true;
			Listeners = null;
		}
	}
}