using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CustomBasicAuthWebApp.Controllers;
using CustomBasicAuthWebApp.Models;
using System.Web.Http.Results;
using System.Net;
using CustomBasicAuthWebApp.ViewModels;

namespace CustomBasicAuthWebApp.Tests
{
    [TestClass]
    public class TestUserController
    {
        [TestMethod]
        public void GetUsers_ShouldReturnAllUsers()
        {
            var testUsers = GetTestUsers();
            var testUserBL = new TestMockUserBusinessLayer(testUsers);
            var controller = new UserController(testUserBL);

            var result = controller.Get() as List<UserViewModel>;
            Assert.AreEqual(testUsers.Count, result.Count);
        }

        [TestMethod]
        public void GetUser_ShouldReturnCorrectUser()
        {
            var testUsers = GetTestUsers();
            var testUserBL = new TestMockUserBusinessLayer(testUsers);
            var controller = new UserController(testUserBL);

            var result = controller.Get("ADMIN") as OkNegotiatedContentResult<UserViewModel>;
            Assert.IsNotNull(result);
            Assert.AreEqual(testUsers[0].Username, result.Content.Username);
        }

        [TestMethod]
        public void GetUser_ShouldNotFindUsers()
        {
            var testUsers = GetTestUsers();
            var testUserBL = new TestMockUserBusinessLayer(testUsers);
            var controller = new UserController(testUserBL);

            var result = controller.Get("somefakeuser");
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PostUser_ShouldReturnSameUser()
        {
            var testUsers = GetTestUsers();
            var testUserBL = new TestMockUserBusinessLayer(testUsers);
            var controller = new UserController(testUserBL);

            var u = GetTestUser();

            var result = controller.Post(u) as CreatedAtRouteNegotiatedContentResult<User>;
            var uFinded = controller.Get(u.Username);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "DefaultApi");
            Assert.AreEqual(result.RouteValues["username"], result.Content.Username);
            Assert.AreEqual(result.Content.Username, u.Username);
            Assert.IsNotNull(uFinded);
        }

        [TestMethod]
        public void PutUser_ShouldReturnStatusCode()
        {
            var testUsers = GetTestUsers();
            var testUserBL = new TestMockUserBusinessLayer(testUsers);
            var controller = new UserController(testUserBL);

            var u = GetTestUser();

            var result = controller.Put(u.Username, u) as StatusCodeResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod]
        public void PutUser_ShouldFail_WhenDifferentUserName()
        {
            var testUsers = GetTestUsers();
            var testUserBL = new TestMockUserBusinessLayer(testUsers);
            var controller = new UserController(testUserBL);

            var u = GetTestUser();

            var badresult = controller.Put("somefakeuser", u);
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteUser_ShouldReturnOK()
        {
            var testUsers = GetTestUsers();
            var testUserBL = new TestMockUserBusinessLayer(testUsers);
            var controller = new UserController(testUserBL);
            var result = controller.Delete("page1") as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
        }

        private List<User> GetTestUsers()
        {
            var testUsers = new List<User>
            {
                new User{ Username="ADMIN", Password="123"},
                new User{ Username="page1", Password="123"}
            };

            return testUsers;
        }

        private User GetTestUser()
        {
            return new User { Username = "page2", Password = "123" };
        }

    }
}
