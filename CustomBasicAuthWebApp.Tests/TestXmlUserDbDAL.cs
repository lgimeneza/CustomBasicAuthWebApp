using CustomBasicAuthWebApp.DataAccessLayer;
using CustomBasicAuthWebApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;


namespace CustomBasicAuthWebApp.Tests
{
    [TestClass]
    public class TestXmlUserDbDAL
    {
        [TestMethod]
        public void XmlUserDbDAL_ShouldReturnAllUsers()
        {
            var testUsers = GetTestUsers();
            ConfigurationManager.AppSettings["XmlDataPath"] = "/test_users.xml";

            XmlUserDbDAL db = new XmlUserDbDAL();
            db.Users = testUsers;
            db.SaveChanges();

            XmlUserDbDAL data = new XmlUserDbDAL();
            var result = data.Users;
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<User>));
            Assert.AreEqual(testUsers.Count, result.Count);
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
    }
}
