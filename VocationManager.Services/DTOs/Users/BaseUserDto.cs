using System.ComponentModel.DataAnnotations;
using VocationManager.Data;
using VocationManager.Services.DTOs.Teams;

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
        public TeamDto Team { get; set; }
        public int? TeamId { get; set; }
    }
}
