using System.Collections.Generic;
using CustomBasicAuthWebApp.Models;

namespace CustomBasicAuthWebApp.DataAccessLayer
{
    public interface IUserDbDAL
    {
        List<User> Users { get; set; }

        void SaveChanges();
    }
}