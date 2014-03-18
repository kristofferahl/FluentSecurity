using System;
using System.Collections.Generic;

namespace FluentSecurity.ServiceLocation
{
	public interface IServiceLocator
	{
		object Resolve(Type typeToResolve);
		TTypeToResolve Resolve<TTypeToResolve>();
		IEnumerable<object> ResolveAll(Type typeToResolve);
		IEnumerable<TTypeToResolve> ResolveAll<TTypeToResolve>();
	}
}