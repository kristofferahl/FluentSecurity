using System;
using System.Collections.Generic;

namespace FluentSecurity
{
	public interface ISecurityConfiguration
	{
		ISecurityConfiguration Configure(Action<ConfigurationExpression> configurationExpression);
		ISecurityConfiguration Reset();
		IEnumerable<IPolicyContainer> PolicyContainers { get; }
		ISecurityServiceLocator ExternalServiceLocator { get; }
		bool IgnoreMissingConfiguration { get; }
		string WhatDoIHave();
	}
}