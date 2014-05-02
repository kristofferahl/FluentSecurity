using System;

namespace FluentSecurity.Core
{
	public interface IControllerNameResolver<in TContext> : IControllerNameResolver
	{
		string Resolve(TContext context);
	}

	public interface IControllerNameResolver
	{
		string Resolve(Type controllerType);
	}
}