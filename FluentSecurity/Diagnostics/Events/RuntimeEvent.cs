using System;

namespace FluentSecurity.Diagnostics.Events
{
	public class RuntimeEvent : SecurityEvent
	{
		public RuntimeEvent(Guid correlationId, string message) : base(correlationId, message) {}
	}
}