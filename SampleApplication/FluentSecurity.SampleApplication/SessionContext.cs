using System.Web;
using FluentSecurity.SampleApplication.Models;

namespace FluentSecurity.SampleApplication.SessionContext
{
	public static class Current
	{
		public static IUser User
		{
			get
			{
				var user = HttpContext.Current.Session["CurrentUser"] as IUser;
				return user;
			}
			set
			{
				HttpContext.Current.Session["CurrentUser"] = value;
			}
		}
	}
}