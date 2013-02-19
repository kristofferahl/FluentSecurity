using System;
using FluentSecurity.Diagnostics.Events;

namespace FluentSecurity.Diagnostics
{
	public class AnonymousSecurityEventListener : ISecurityEventListener
	{
		public Action<ISecurityEvent> EventListener { get; private set; }

		public AnonymousSecurityEventListener(Action<ISecurityEvent> eventListener)
		{
			if (eventListener == null)
				throw new ArgumentNullException("eventListener");

			EventListener = eventListener;
		}

		public void Handle(ISecurityEvent securityEvent)
		{
			try
			{
				EventListener.Invoke(securityEvent);
			}
			catch {}
		}
	}
}