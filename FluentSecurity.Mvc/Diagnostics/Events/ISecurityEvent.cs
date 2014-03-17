using System;

namespace FluentSecurity.Diagnostics.Events
{
	public interface ISecurityEvent
	{
		Guid CorrelationId { get; }
		string Message { get; }
		long? CompletedInMilliseconds { get; }
	}
}