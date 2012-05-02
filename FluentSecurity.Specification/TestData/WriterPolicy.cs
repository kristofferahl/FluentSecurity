using FluentSecurity.Policy;

namespace FluentSecurity.Specification.TestData
{
	public class WriterPolicy : ISecurityPolicy
	{
		public PolicyResult Enforce(ISecurityContext context)
		{
			return PolicyResult.CreateSuccessResult(this);
		}
	}
}