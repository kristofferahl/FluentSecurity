using FluentSecurity.Caching;
using FluentSecurity.Core;

namespace FluentSecurity
{
	public class SecurityRuntime : SecurityRuntimeBase
	{
		public SecurityRuntime(ISecurityCache cache, ITypeFactory typeFactory) : base(cache, typeFactory) {}
	}
}