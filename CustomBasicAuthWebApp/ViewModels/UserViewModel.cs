using System.Collections.Generic;

// The password field is write-only and will not be exposed on read operations
namespace CustomBasicAuthWebApp.ViewModels
{
    /// <summary>
    /// Implemets the view model for the presentation layer
    /// The password field is write-only and will not be exposed on read operations
    /// </summary>
    public class UserViewModel
    {
        public string Username { get; set; }

        public List<string> Roles { get; set; }
    }
}