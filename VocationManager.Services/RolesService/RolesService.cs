using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VocationManager.Data;
using System.Data;

namespace VocationManager.Services.RolesService
{
    public class RolesService : IRolesService
    {
        private readonly ApplicationDbContext _dbContext;

        public RolesService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string?> GetNameById(string id)
        {
            var role = await _dbContext
                .Roles
                .FirstOrDefaultAsync(r => r.Id == id);

            return role?.Name;
        }
        public async Task<string> GetRoleNameByUserId(string userId)
        {
            var userRoles = await _dbContext
                    .UserRoles
                    .Where(ur => ur.UserId == userId)
                    .Select(ur => ur.RoleId)
                    .ToArrayAsync();
            var roleName = await _dbContext
                .Roles
                .Where(r => userRoles.Contains(r.Id))
                .Select(r => r.Name)
                .FirstOrDefaultAsync();

            return roleName;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return _dbContext
                .Roles
                .ToArray()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                })
                .OrderBy(x => x.Name)
                .ToList().Select(x => new KeyValuePair<string, string>(x.Id, x.Name));
        }
    }
}
