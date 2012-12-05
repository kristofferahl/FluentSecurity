using System.Collections.Generic;

namespace FluentSecurity.SampleApplication.Models
{
	public class User : IUser
	{
		public User()
		{
			Roles = new List<UserRole>();
		}

		public IEnumerable<UserRole> Roles { get; set; }
	}
}