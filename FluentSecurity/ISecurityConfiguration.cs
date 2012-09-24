using System.Collections.Generic;

namespace FluentSecurity
{
	public interface ISecurityConfiguration
	{
		IAdvanced Advanced { get; }
		IEnumerable<IPolicyContainer> PolicyContainers { get; }
		ISecurityServiceLocator ExternalServiceLocator { get; }
		string WhatDoIHave();
	}
}