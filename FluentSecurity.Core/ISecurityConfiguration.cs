using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	public interface ISecurityConfiguration
	{
		ISecurityRuntime Runtime { get; }
		IServiceLocator ServiceLocator { get; }
		ISecurityContext CreateContext();
		string WhatDoIHave();
	}
}