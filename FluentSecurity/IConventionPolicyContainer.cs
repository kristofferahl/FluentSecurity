using FluentSecurity.Policy;

namespace FluentSecurity
{
	public interface  IConventionPolicyContainer
	{
		IConventionPolicyContainer AddPolicy(ISecurityPolicy securityPolicy);
	}
}