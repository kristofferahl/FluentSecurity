using FluentSecurity.Core;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity.WebApi
{
	public class WebApiSecurityRuntime : SecurityRuntimeBase
	{
		public WebApiSecurityRuntime(IContainer container) : base(container) {}
	}
}