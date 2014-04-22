using FluentSecurity.Core;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public interface IPolicyViolationHandlerConvention : IConvention
	{
		object GetHandlerFor(PolicyViolationException exception);
	}
}