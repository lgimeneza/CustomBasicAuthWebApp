using System;
using System.Collections.Generic;

namespace CustomBasicAuthWebApp.Models
{
    public interface IUserBusinessLayer
    {
        User AddUser(User user);
        User GetUser(string username);
        List<User> GetUsers();
        void RemoveUser(User user);
        User UpdateUser(User user);
    }
}