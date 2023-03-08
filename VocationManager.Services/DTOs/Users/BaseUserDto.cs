using System.ComponentModel.DataAnnotations;
using VocationManager.Data;

namespace VocationManager.Services.DTOs.Users
{
    public class BaseUserDto
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
