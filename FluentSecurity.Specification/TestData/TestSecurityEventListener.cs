using FluentSecurity.Diagnostics;
using FluentSecurity.Diagnostics.Events;

namespace FluentSecurity.Specification.TestData
{
	public class TestSecurityEventListener : ISecurityEventListener
	{
		public void Handle(ISecurityEvent securityEvent) {}
	}
}