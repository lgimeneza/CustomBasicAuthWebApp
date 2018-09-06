using CustomBasicAuthWebApp.Filters;
using System.Web.Http;

namespace CustomBasicAuthWebApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{username}",
                defaults: new { username = RouteParameter.Optional }
            );

            config.Filters.Add(new BasicAuthentication());
        }
    }
}
