using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CustomBasicAuthWebApp.Models
{
    /// <summary>
    /// Represents User Model
    /// </summary>
    public class User
    {
        // The user model will have a “username” field, a “roles” field and a “password” field.
        [Required(ErrorMessage = Constants.ErrMsgUserNameRequired)]
        [MinLength(3, ErrorMessage = Constants.ErrMsgMinLength)]
        [StringLength(50, ErrorMessage = Constants.ErrMsgMaxLength)]
        public string Username { get; set; }

        [Required(ErrorMessage = Constants.ErrMsgPasswordRequired)]
        [MinLength(3, ErrorMessage = Constants.ErrMsgMinLength)]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = Constants.ErrMsgMaxLength)]
        public string Password { get; set; }

        public List<string> Roles { get; set; }
    }
}