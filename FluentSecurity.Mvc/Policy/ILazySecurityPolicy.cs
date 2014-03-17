using System;

namespace FluentSecurity.Policy
{
	internal interface ILazySecurityPolicy : ISecurityPolicy
	{
		Type PolicyType { get; }
		ISecurityPolicy Load();
	}
}