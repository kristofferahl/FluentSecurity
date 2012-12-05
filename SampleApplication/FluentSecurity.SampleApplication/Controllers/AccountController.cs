using System.Collections.Generic;
using System.Web.Mvc;
using FluentSecurity.SampleApplication.Models;
using FluentSecurity.SampleApplication.Helpers;

namespace FluentSecurity.SampleApplication.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult LogInAsAdministrator()
        {
			SessionContext.Current.User = new User
				{
					Roles = new List<UserRole>
						{
							UserRole.Administrator
						}
				};

			return Redirect(Url.Action<HomeController>(x => x.Index()));
        }

		public ActionResult LogInAsPublisher()
		{
			SessionContext.Current.User = new User
			{
				Roles = new List<UserRole>
						{
							UserRole.Publisher
						}
			};

			return Redirect(Url.Action<HomeController>(x => x.Index()));
		}

		public ActionResult LogOut()
		{
			SessionContext.Current.User = null;

			return Redirect(Url.Action<HomeController>(x => x.Index()));
		}
    }
}