using System.Collections.Generic;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	public interface ISecurityConfiguration
	{
		ISecurityRuntime Runtime { get; }
		IServiceLocator ServiceLocator { get; }
		IEnumerable<IPolicyContainer> PolicyContainers { get; }
		string WhatDoIHave();
	}
}