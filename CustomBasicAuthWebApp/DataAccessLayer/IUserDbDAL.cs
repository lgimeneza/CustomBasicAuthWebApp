using System.Collections.Generic;
using CustomBasicAuthWebApp.Models;

namespace CustomBasicAuthWebApp.DataAccessLayer
{
    /// <summary>
    /// Represents Data Access Layer for User Model
    /// </summary>
    public interface IUserDbDAL
    {
        List<User> Users { get; set; }

        void SaveChanges();
    }
}