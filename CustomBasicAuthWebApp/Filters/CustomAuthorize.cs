using System.Web.Mvc;

namespace CustomBasicAuthWebApp.Filters
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // In case of accessing any of the private pages without an established session,
            // the application will redirect the user to the login form.
            // This behabior is provided by the AuthorizeAttribute by default on his OnAuthorization base.
            base.OnAuthorization(filterContext);

            if (filterContext.HttpContext.User.Identity.IsAuthenticated &&
            filterContext.Result is HttpUnauthorizedResult)
            {
                // In case of accessing any of these private pages with an established session
                // but without the right role, the application will not allow the user to see the page.
                // The application will return a relevant status code indicating that access was denied
                filterContext.Result = new RedirectResult("~/Home/Denied");
            }

        }
    }
}