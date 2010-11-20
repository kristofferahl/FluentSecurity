using System;
using System.Collections.Generic;

namespace FluentSecurity
{
	public interface ISecurityConfiguration
	{
		ISecurityConfiguration Configure(Action<ConfigurationExpression> configurationExpression);
		ISecurityConfiguration Reset();
		string WhatDoIHave();
		IEnumerable<IPolicyContainer> PolicyContainers { get; }
		bool IgnoreMissingConfiguration { get; }
		IPolicyManager PolicyManager { get; }
		IWhatDoIHaveBuilder WhatDoIHaveBuilder { get; }
	}
}