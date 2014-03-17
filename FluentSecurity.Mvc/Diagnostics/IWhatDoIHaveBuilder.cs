namespace FluentSecurity.Diagnostics
{
	public interface IWhatDoIHaveBuilder
	{
		string WhatDoIHave(ISecurityConfiguration configuration);
	}
}