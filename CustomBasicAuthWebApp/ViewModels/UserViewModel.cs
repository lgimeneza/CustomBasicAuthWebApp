using System.Collections.Generic;

namespace CustomBasicAuthWebApp.ViewModels
{
    /// <summary>
    /// Implemets the view model for the presentation layer
    /// </summary>
    public class UserViewModel
    {
        public string Username { get; set; }

        public List<string> Roles { get; set; }

        // The password field is write-only and will not be exposed on read operations
    }
}