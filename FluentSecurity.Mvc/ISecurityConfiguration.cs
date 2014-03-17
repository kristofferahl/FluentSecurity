using System.Collections.Generic;
using System.Reflection;

namespace FluentSecurity
{
	public interface ISecurityConfiguration
	{
		ISecurityRuntime Runtime { get; }
		IEnumerable<IPolicyContainer> PolicyContainers { get; }
		void AssertAllActionsAreConfigured();
		void AssertAllActionsAreConfigured(Assembly[] assemblies);
		string WhatDoIHave();
	}
}