using System;

namespace FluentSecurity.Diagnostics.Events
{
	public class SecurityRuntimeEvent : ISecurityEvent
	{
		public SecurityRuntimeEvent(Guid id, string message)
		{
			Id = id;
			Message = message;
		}

		public Guid Id { get; private set; }
		public string Message { get; private set; }
	}
}