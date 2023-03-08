using System.ComponentModel.DataAnnotations;
using VocationManager.Data;

namespace VocationManager.Services.DTOs.Users
{
    public class BaseUserDto
    {
        public BaseUserDto()
        {

        }

        public BaseUserDto(ApplicationUser domainUser)
        {
            Id = domainUser.Id;
            Username = domainUser.UserName;
            Email = domainUser.Email;
            FirstName = domainUser.FirstName;
            LastName = domainUser.LastName;
        }
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
