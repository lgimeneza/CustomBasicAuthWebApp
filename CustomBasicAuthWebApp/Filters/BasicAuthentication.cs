using CustomBasicAuthWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace CustomBasicAuthWebApp.Filters
{
    /// <summary>
    /// Handles the HTTP basic authentication inplementing a custom AuthorizeAttibute
    /// </summary>
    public class BasicAuthentication : AuthorizeAttribute
    {
        public new string Roles { get; set; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authHeader = actionContext.Request.Headers.Authorization;

            if (authHeader != null)
            {
                var authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                var decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                var usernamePasswordArray = decodedAuthenticationToken.Split(':');
                var userName = usernamePasswordArray[0].ToLower();
                var password = usernamePasswordArray[1];

                UserBusinessLayer userBL = new UserBusinessLayer();
                List<User> users = userBL.GetUsers();
                User user = users.Where(item => item.Username.ToLower().Equals(userName) && item.Password.Equals(password)).FirstOrDefault();

                // Check if user-role and password provided are authorized
                if (user != null && isRoleAuth(user))
                {
                    var principal = new GenericPrincipal(new GenericIdentity(userName), null);
                    Thread.CurrentPrincipal = principal;

                    return;
                }
            }

            HandleUnathorized(actionContext);
        }

        private static void HandleUnathorized(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        private bool isRoleAuth(User user)
        {
            if (Roles == null) return true;

            if (Roles != null && user.Roles.Contains(Roles)) return true;

            return false;
        }
    }
}