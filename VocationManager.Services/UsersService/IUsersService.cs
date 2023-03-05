using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VocationManager.Data;
using VocationManager.Services.DTOs;

namespace VocationManager.Services.UsersService
{
    public interface IUsersService
    {
        Task<ICollection<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser?> GetByIdAsync(string userId);
        Task CreateAsync(CreateUserDto userDto);
        Task DeleteAsync(string userId);
        Task<bool> UserExists(string userId);
    }
}
