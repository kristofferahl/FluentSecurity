using System;

namespace FluentSecurity.Policy
{
	public interface ILazySecurityPolicy : ISecurityPolicy
	{
		Type PolicyType { get; }
		ISecurityPolicy Load();
	}
}