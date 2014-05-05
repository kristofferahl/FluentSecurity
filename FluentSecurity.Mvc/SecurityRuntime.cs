using FluentSecurity.Core;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	public class SecurityRuntime : SecurityRuntimeBase
	{
		public SecurityRuntime(IContainer container) : base(container) {}
	}
}