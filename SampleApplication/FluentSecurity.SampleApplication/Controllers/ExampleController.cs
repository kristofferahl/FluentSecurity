using System.Web.Mvc;

namespace FluentSecurity.SampleApplication.Controllers
{
    public class ExampleController : Controller
    {
        public ActionResult MissingConfiguration()
        {
            return View();
        }

    	public ActionResult DenyAnonymousAccess()
    	{
			return View();
    	}

    	public ActionResult DenyAuthenticatedAccess()
    	{
    		return View();
    	}

    	public ActionResult RequireAdministratorRole()
    	{
    		return View();
    	}

    	public ActionResult RequirePublisherRole()
    	{
			return View();
    	}
    }
}