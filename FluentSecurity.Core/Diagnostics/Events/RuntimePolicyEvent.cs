using System;

namespace FluentSecurity.Diagnostics.Events
{
	public class RuntimePolicyEvent : SecurityEvent
	{
		public RuntimePolicyEvent(Guid correlationId, string message) : base(correlationId, message) {}
	}
}