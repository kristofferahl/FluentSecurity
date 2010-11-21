using System.Collections.Generic;
using FluentSecurity.Policy;
using FluentSecurity.SampleApplication.Models;

namespace FluentSecurity.SampleApplication
{
	public class AdministratorPolicy : RequireRolePolicy
	{
		public AdministratorPolicy() : base(new List<object> { UserRole.Administrator }.ToArray()) {}
	}
}