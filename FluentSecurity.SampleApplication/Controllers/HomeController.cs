using System.Web.Mvc;
using FluentSecurity.SampleApplication.Models;

namespace FluentSecurity.SampleApplication.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			var outModel = new HomeView
				{
					WhatDoIHave = FluentSecurity.WhatDoIHave()
				};
			return View(outModel);
		}
	}
}