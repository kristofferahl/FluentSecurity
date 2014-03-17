using System;

namespace FluentSecurity.Diagnostics.Events
{
	public class ConfigurationEvent : SecurityEvent
	{
		public ConfigurationEvent(Guid correlationId, string message) : base(correlationId, message) {}
	}
}