using FluentSecurity.Diagnostics.Events;

namespace FluentSecurity.Diagnostics
{
	public interface ISecurityEventListener
	{
		void Handle(ISecurityEvent securityEvent);
	}
}