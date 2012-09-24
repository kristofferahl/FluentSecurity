using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FluentSecurity.SampleApplication.Areas.ExampleArea.Controllers
{
    public class BaseController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }

        public ActionResult Details()
        {
            return View();
        }
    }
}
