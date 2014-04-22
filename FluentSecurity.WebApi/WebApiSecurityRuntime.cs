using FluentSecurity.Caching;
using FluentSecurity.Core;

namespace FluentSecurity.WebApi
{
	public class WebApiSecurityRuntime : SecurityRuntimeBase
	{
		public WebApiSecurityRuntime(ISecurityCache cache, ITypeFactory typeFactory) : base(cache, typeFactory) {}
	}
}