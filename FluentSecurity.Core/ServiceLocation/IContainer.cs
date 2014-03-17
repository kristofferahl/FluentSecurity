using System;
using System.Collections.Generic;

namespace FluentSecurity.ServiceLocation
{
	public interface IContainer
	{
		void Register<TTypeToResolve>(Func<IContainer, object> instanceExpression, Lifecycle lifecycle = Lifecycle.Transient);
		TTypeToResolve Resolve<TTypeToResolve>();
		object Resolve(Type typeToResolve);
		IEnumerable<TTypeToResolve> ResolveAll<TTypeToResolve>();
		IEnumerable<object> ResolveAll(Type typeToResolve);
		void SetPrimarySource(Func<IContainer, IContainerSource> source);
	}
}