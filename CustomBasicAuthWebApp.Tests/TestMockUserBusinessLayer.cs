using CustomBasicAuthWebApp.Models;
using System.Collections.Generic;
using System.Linq;


namespace CustomBasicAuthWebApp.Tests
{
    class TestMockUserBusinessLayer : IUserBusinessLayer
    {
        List<User> Users = new List<User>();

        public TestMockUserBusinessLayer() { }

        public TestMockUserBusinessLayer(List<User> users)
        {
            Users = users;
        }

        public User AddUser(User user)
        {
            Users.Add(user);

            return user;
        }

        public User GetUser(string username)
        {
            User user = Users.FirstOrDefault(u => u.Username.ToLower() == username.ToLower());
            return user;
        }

        public List<User> GetUsers()
        {
            return Users;
        }

        public void RemoveUser(User user)
        {
            var item = Users.FirstOrDefault(u => u.Username.ToLower() == user.Username.ToLower());
            if (item != null)
            {
                Users.Remove(item);
            }
        }

        public User UpdateUser(User user)
        {
            var userFoud = Users.FirstOrDefault(u => u.Username.ToLower() == user.Username.ToLower());
            if (userFoud != null)
            {
                if (user.Password != null) userFoud.Password = user.Password;
                if (user.Roles != null) userFoud.Roles = user.Roles;
            }

            return user;
        }
    }
}
