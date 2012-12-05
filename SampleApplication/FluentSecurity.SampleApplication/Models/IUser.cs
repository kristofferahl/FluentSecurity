using System.Collections.Generic;

namespace FluentSecurity.SampleApplication.Models
{
	public interface IUser
	{
		IEnumerable<UserRole> Roles { get; set; }
	}
}