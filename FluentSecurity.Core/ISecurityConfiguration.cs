using System.Collections.Generic;

namespace FluentSecurity
{
	public interface ISecurityConfiguration
	{
		ISecurityRuntime Runtime { get; }
		IEnumerable<IPolicyContainer> PolicyContainers { get; }
		string WhatDoIHave();
	}
}