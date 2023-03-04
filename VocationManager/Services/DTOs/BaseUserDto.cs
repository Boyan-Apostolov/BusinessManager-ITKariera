﻿using VocationManager.Data;

namespace VocationManager.Services.DTOs
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

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
