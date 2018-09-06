using CustomBasicAuthWebApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace CustomBasicAuthWebApp.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User u, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // Check the username and password provided in the view
                UserBusinessLayer userBL = new UserBusinessLayer();
                List<User> users = userBL.GetUsers();
                var username = users.Where(item => item.Username.ToLower().Equals(u.Username.ToLower()) && item.Password.Equals(u.Password)).FirstOrDefault();
                if (username != null)
                {
                    FormsAuthentication.SetAuthCookie(u.Username, false);

                    // Once the user logs-in successfully, the application will redirect him
                    // to the page that was intended to access. 
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction(returnUrl.Split('/')[2], returnUrl.Split('/')[1]);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View(u);
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        [AllowAnonymous]
        public ActionResult Denied()
        {
            return View();
        }

    }
}