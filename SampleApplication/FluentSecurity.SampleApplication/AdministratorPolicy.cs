using FluentSecurity.Policy;
using FluentSecurity.SampleApplication.Models;

namespace FluentSecurity.SampleApplication
{
	public class AdministratorPolicy : ISecurityPolicy
	{
		public PolicyResult Enforce(ISecurityContext context)
		{
			var innerPolicy = new RequireAllRolesPolicy(UserRole.Administrator);
			var result = innerPolicy.Enforce(context);
			
			return result.ViolationOccured ? PolicyResult.CreateFailureResult(this, result.Message) : PolicyResult.CreateSuccessResult(this);
		}
	}
}