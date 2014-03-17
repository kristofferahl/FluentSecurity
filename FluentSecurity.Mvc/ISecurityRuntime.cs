using System;
using System.Collections.Generic;
using FluentSecurity.Caching;

namespace FluentSecurity
{
	public interface ISecurityRuntime
	{
		Func<bool> IsAuthenticated { get; }
		Func<IEnumerable<object>> Roles { get; }
		ISecurityServiceLocator ExternalServiceLocator { get; }
		IEnumerable<Type> Profiles { get; }
		IEnumerable<IPolicyContainer> PolicyContainers { get; }
		IEnumerable<IConvention> Conventions { get; }
		Cache DefaultResultsCacheLifecycle { get; }
		Action<ISecurityContext> SecurityContextModifyer { get; }
		bool ShouldIgnoreMissingConfiguration { get; }
	}
}