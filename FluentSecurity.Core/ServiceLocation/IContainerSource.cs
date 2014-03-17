using System;
using System.Collections.Generic;

namespace FluentSecurity.ServiceLocation
{
	public interface IContainerSource
	{
		object Resolve(Type typeToResolve);
		IEnumerable<object> ResolveAll(Type typeToResolve);
	}
}