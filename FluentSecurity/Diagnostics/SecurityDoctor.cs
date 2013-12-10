using System;
using System.Collections.Generic;
using FluentSecurity.Diagnostics.Events;
using FluentSecurity.Scanning;

namespace FluentSecurity.Diagnostics
{
	public class SecurityDoctor
	{
		public static SecurityDoctor Current { get; private set; }

		static SecurityDoctor()
		{
			Reset();
		}

		public static void Register(Action<ISecurityEvent> eventListener)
		{
			Current.RegisterListener(eventListener);
		}

		public static void Register(ISecurityEventListener eventListener)
		{
			Current.RegisterListener(eventListener);
		}

		public static void Reset()
		{
			Current = new SecurityDoctor();
		}

		public bool ScannedForEventListeners { get; private set; }
		public bool ScanForEventListenersOnConfigure { get; set; }
		public Action<AssemblyScanner> EventListenerScannerSetup { get; set; }
		internal IList<ISecurityEventListener> Listeners { get; private set; }

		public SecurityDoctor()
		{
			Listeners = null;
			ScanForEventListenersOnConfigure = true;
			ScannedForEventListeners = false;
		}

		public void ScanForEventListeners()
		{
			var assemblyScanner = new AssemblyScanner();
			
			if (EventListenerScannerSetup == null)
				assemblyScanner.AssembliesFromApplicationBaseDirectory();
			else
				EventListenerScannerSetup.Invoke(assemblyScanner);
			
			assemblyScanner.With<SecurityEventListenerScanner>();
			var eventListeners = assemblyScanner.Scan();

			foreach (var eventListenerType in eventListeners)
			{
				var eventListener = (ISecurityEventListener) Activator.CreateInstance(eventListenerType);
				RegisterListener(eventListener);
			}

			ScannedForEventListeners = true;
		}

		public void RegisterListener(Action<ISecurityEvent> eventListener)
		{
			RegisterListener(new AnonymousSecurityEventListener(eventListener));
		}

		public void RegisterListener(ISecurityEventListener eventListener)
		{
			if (Listeners == null) Listeners = new List<ISecurityEventListener>();
			Listeners.Add(eventListener);
		}
	}
}