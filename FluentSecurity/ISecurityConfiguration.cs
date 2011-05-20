using System.Collections.Generic;

namespace FluentSecurity
{
	public interface ISecurityConfiguration
	{
		IEnumerable<IPolicyContainer> PolicyContainers { get; }
		ISecurityServiceLocator ExternalServiceLocator { get; }
		bool IgnoreMissingConfiguration { get; }
		string WhatDoIHave();
	}
}