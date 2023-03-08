using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VocationManager.Data;
using VocationManager.Services.DTOs.Users;

namespace VocationManager.Services.UsersService
{
    public interface IUsersService
    {
        Task<ICollection<BaseUserDto>> GetAllAsync();
        Task<PaginatedUsersCollectionDto> GetPaginatedAndFilteredUsers(int? page, int? pageSize, string keyword);
        Task<BaseUserDto?> GetByIdAsync(string userId, bool disableTracking = true);
        Task CreateAsync(CreateUserDto userDto);
        Task EditAsync(BaseUserDto userDto);
        Task DeleteAsync(string userId);
        Task<bool> UserExists(string userId);
    }
}
