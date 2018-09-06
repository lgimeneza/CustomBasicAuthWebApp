using CustomBasicAuthWebApp.Filters;
using System.Web.Mvc;

namespace CustomBasicAuthWebApp.Controllers
{
    public class PrivateController : Controller
    {
        // Page 1: To access this page, the logged user needs to have the role PAGE_1 
        [CustomAuthorize(Roles = "PAGE_1")]
        public ActionResult Page_1()
        {
            return View();
        }

        // Page 2: To access this page, the logged user needs to have the role PAGE_2 
        [CustomAuthorize(Roles = "PAGE_2")]
        public ActionResult Page_2()
        {
            return View();
        }

        // Page 3: To access this page, the logged user needs to have the role PAGE_3 
        [CustomAuthorize(Roles = "PAGE_3")]
        public ActionResult Page_3()
        {
            return View();
        }
    }
}