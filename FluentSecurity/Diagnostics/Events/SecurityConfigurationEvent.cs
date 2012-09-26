using System;

namespace FluentSecurity.Diagnostics.Events
{
	public class SecurityConfigurationEvent : ISecurityEvent
	{
		public SecurityConfigurationEvent(Guid id, string message)
		{
			Id = id;
			Message = message;
		}

		public Guid Id { get; private set; }
		public string Message { get; private set; }
	}
}