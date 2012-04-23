using System.Collections.Generic;
using FluentSecurity.Configuration;

namespace FluentSecurity
{
	public interface ISecurityConfiguration
	{
		IAdvancedConfiguration Advanced { get; }
		IEnumerable<IPolicyContainer> PolicyContainers { get; }
		ISecurityServiceLocator ExternalServiceLocator { get; }
		bool IgnoreMissingConfiguration { get; }
		string WhatDoIHave();
	}
}