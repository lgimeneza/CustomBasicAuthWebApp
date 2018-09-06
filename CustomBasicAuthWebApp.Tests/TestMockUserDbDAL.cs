using CustomBasicAuthWebApp.DataAccessLayer;
using CustomBasicAuthWebApp.Models;
using System.Collections.Generic;


namespace CustomBasicAuthWebApp.Tests
{
    class TestMockUserDbDAL : IUserDbDAL
    {
        public TestMockUserDbDAL(List<User> users)
        {
            Users = users;
        }

        public List<User> Users { get; set; }

        public void SaveChanges(){}
    }
}
