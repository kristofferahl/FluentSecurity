using System;

namespace FluentSecurity.Diagnostics.Events
{
	public class SecurityRuntimePolicyEvent : SecurityRuntimeEvent
	{
		public SecurityRuntimePolicyEvent(Guid id, string message) : base(id, message) {}
	}
}