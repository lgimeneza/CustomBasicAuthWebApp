using CustomBasicAuthWebApp.DataAccessLayer;
using System.Collections.Generic;
using System.Linq;

namespace CustomBasicAuthWebApp.Models
{
    /// <summary>
    /// Represents BusinessLayer of User Model
    /// </summary>
    public class UserBusinessLayer : IUserBusinessLayer
    {
        private IUserDbDAL _userDbDAL;

        public UserBusinessLayer()
        {
            _userDbDAL = new XmlUserDbDAL();
        }

        public UserBusinessLayer(IUserDbDAL userDbDAL)
        {
            _userDbDAL = userDbDAL;
        }

        public List<User> GetUsers()
        {
            return _userDbDAL.Users.ToList();
        }

        public User GetUser(string username)
        {
            User user = _userDbDAL.Users.FirstOrDefault(u => u.Username.ToLower() == username.ToLower());
            return user;
        }

        public User AddUser(User user)
        {
            var userFoud = _userDbDAL.Users.FirstOrDefault(u => u.Username.ToLower() == user.Username.ToLower());
            if (userFoud != null) throw new System.Exception("User already exist");

            _userDbDAL.Users.Add(user);
            _userDbDAL.SaveChanges();
            return user;
        }

        public User UpdateUser(User user)
        {
            var userFoud = _userDbDAL.Users.FirstOrDefault(u => u.Username.ToLower() == user.Username.ToLower());
            if (userFoud != null)
            {
                if (user.Password != null) userFoud.Password = user.Password;
                if (user.Roles != null) userFoud.Roles = user.Roles;
                _userDbDAL.SaveChanges();
            }
           
            return user;
        }

        public void RemoveUser(User user)
        {
            var item = _userDbDAL.Users.FirstOrDefault(u => u.Username.ToLower() == user.Username.ToLower());
            if (item != null)
            {
                _userDbDAL.Users.Remove(item);
                _userDbDAL.SaveChanges();
            }
        }

    }
}