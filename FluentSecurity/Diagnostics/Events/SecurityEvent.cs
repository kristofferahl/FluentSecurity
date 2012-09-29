using System;

namespace FluentSecurity.Diagnostics.Events
{
	public abstract class SecurityEvent : ISecurityEvent
	{
		protected SecurityEvent(Guid correlationId, string message)
		{
			CorrelationId = correlationId;
			Message = message;
		}

		public Guid CorrelationId { get; private set; }
		public string Message { get; private set; }
		public long? CompletedInMilliseconds { get; set; }
	}
}