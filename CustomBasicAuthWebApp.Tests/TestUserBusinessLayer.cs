using CustomBasicAuthWebApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CustomBasicAuthWebApp.Tests
{
    [TestClass]
    public class TestUserBusinessLayer
    {
        [TestMethod]
        public void GetUsers_ShouldReturnAllUsers()
        {
            var testUsers = GetTestUsers();

            var testMockUserDbDAL = new TestMockUserDbDAL(testUsers);
            var testUserBL = new UserBusinessLayer(testMockUserDbDAL);

            var result = testUserBL.GetUsers() as List<User>;
            Assert.AreEqual(testUsers.Count, result.Count);
        }

        [TestMethod]
        public void GetUser_ShouldReturnCorrectUser()
        {
            var testUsers = GetTestUsers();

            var testMockUserDbDAL = new TestMockUserDbDAL(testUsers);
            var testUserBL = new UserBusinessLayer(testMockUserDbDAL);

            var result = testUserBL.GetUser("ADMIN") as User;
            Assert.IsNotNull(result);
            Assert.AreEqual(testUsers[0].Username, result.Username);
        }

        [TestMethod]
        public void GetUser_ShouldNotFindUsers()
        {
            var testUsers = GetTestUsers();

            var testMockUserDbDAL = new TestMockUserDbDAL(testUsers);
            var testUserBL = new UserBusinessLayer(testMockUserDbDAL);

            var result = testUserBL.GetUser("somefakeuser");
            Assert.IsNull(result);
        }

        //public User AddUser(User user)
        [TestMethod]
        public void AddUser_ShouldAddUser()
        {
            var testUsers = GetTestUsers();

            var testMockUserDbDAL = new TestMockUserDbDAL(testUsers);
            var testUserBL = new UserBusinessLayer(testMockUserDbDAL);

            var u = GetTestUser();

            var result = testUserBL.AddUser(u) as User;
            var uFinded = testUserBL.GetUser(u.Username);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Username, u.Username);
            Assert.IsNotNull(uFinded);
        }

        [TestMethod]
        public void PutUser_ShouldUpdateTheUser()
        {
            var testUsers = GetTestUsers();

            var testMockUserDbDAL = new TestMockUserDbDAL(testUsers);
            var testUserBL = new UserBusinessLayer(testMockUserDbDAL);

            var u = GetTestUpdateUser();
            testUserBL.AddUser(u);
            var result = testUserBL.UpdateUser(u) as User;
            var uFinded = testUserBL.GetUser(u.Username) as User;
            Assert.IsNotNull(result);
            Assert.AreEqual(u.Password, uFinded.Password);
        }

        [TestMethod]
        public void RemoveUser_ShouldDeleteTheUser()
        {
            var testUsers = GetTestUsers();

            var testMockUserDbDAL = new TestMockUserDbDAL(testUsers);
            var testUserBL = new UserBusinessLayer(testMockUserDbDAL);

            var u = GetTestUpdateUser();

            testUserBL.RemoveUser(u);
            var uFinded = testUserBL.GetUser(u.Username);
            Assert.IsNull(uFinded);
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

        private User GetTestUpdateUser()
        {
            return new User { Username = "page2", Password = "456" };
        }
    }
}
