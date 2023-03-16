using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VocationManager.Services.DTOs.Roles;
using VocationManager.Services.DTOs.Users;
using PaginatedRolesCollectionDto = VocationManager.Services.DTOs.Roles.PaginatedRolesCollectionDto;

namespace VocationManager.Services.RolesService
{
    public interface IRolesService
    {
        Task<string> GetRoleNameByUserId(string userId);

        Task<string?> GetNameById(string id);

        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();

        Task<ICollection<RoleDto>> GetAllAsync();
        
        Task<RoleDto?> GetByIdAsync(string roleId, bool disableTracking = true);
        
        Task<string> CreateAsync(BaseRoleDto roleDto);
        
        Task EditAsync(BaseRoleDto roleDto);
        
        Task DeleteAsync(string roleId);
        
        Task<bool> RoleExists(string roleName);

        Task<PaginatedRolesCollectionDto?> GetPaginatedRoles(int? page, int? pageSize);

        Task<ICollection<BaseUserDto>> GetUsersByRoleId(string roleId);
    }
}
