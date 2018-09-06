using CustomBasicAuthWebApp.Filters;
using CustomBasicAuthWebApp.Models;
using CustomBasicAuthWebApp.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace CustomBasicAuthWebApp.Controllers
{
    // The ADMIN role allows the user to read, modify and create other users through the REST API
    // The authentication will be done using HTTP basic authentication. 
    [BasicAuthentication(Roles = "ADMIN")]
    public class UserController : ApiController
    {
        private IUserBusinessLayer userBL;

        public UserController()
        {
            userBL = new UserBusinessLayer();
        }

        public UserController(IUserBusinessLayer userBL)
        {
            this.userBL = userBL;
        }

        // GET: api/User
        [HttpGet]
        public IEnumerable<UserViewModel> Get()
        {
            List<User> users = userBL.GetUsers();
            List<UserViewModel> vmUsers = new List<UserViewModel>();
            foreach(User u in users)
            {
                UserViewModel vmUser = new UserViewModel();
                vmUser.Username = u.Username;
                vmUser.Roles = u.Roles;
                vmUsers.Add(vmUser);
            }

            return vmUsers;
        }

        // GET: api/User/username
        [HttpGet]
        [ResponseType(typeof(UserViewModel))]
        public IHttpActionResult Get(string username)
        {
            User user = userBL.GetUser(username);
            if (user == null)
            {
                return NotFound();
            }

            UserViewModel vmUser = new UserViewModel();
            vmUser.Username = user.Username;
            vmUser.Roles = user.Roles;

            return Ok(vmUser);
        }

        // POST: api/User
        [HttpPost]
        [ResponseType(typeof(User))]
        public IHttpActionResult Post([FromBody]User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid passed data");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User userFound = userBL.GetUser(user.Username);

            if (userFound != null)
            {
                return Conflict();
            }

            User u = userBL.AddUser(user);

            return CreatedAtRoute("DefaultApi", new { username = user.Username }, u);
        }

        // PUT: api/User/username
        [HttpPut]
        public IHttpActionResult Put(string username, [FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (username != user.Username)
            {
                return BadRequest();
            }

            User u = userBL.UpdateUser(user);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/User/username
        [HttpDelete]
        [ResponseType(typeof(User))]
        public IHttpActionResult Delete(string username)
        {
            User user = userBL.GetUser(username);
            if (user == null)
            {
                return NotFound();
            }

            userBL.RemoveUser(user);

            return StatusCode(HttpStatusCode.NoContent);
        }

    }
}
