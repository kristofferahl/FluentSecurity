using System;

namespace FluentSecurity.Diagnostics.Events
{
	public interface ISecurityEvent
	{
		Guid Id { get; }
		string Message { get; }
	}
}