using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VocationManager.Data;
using System.Data;
using VocationManager.Services.DTOs.Roles;
using VocationManager.Services.DTOs.Users;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace VocationManager.Services.RolesService
{
    public class RolesService : IRolesService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public RolesService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
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

        public async Task<ICollection<BaseUserDto>> GetUsersByRoleId(string roleId)
        {
            var userIds = _dbContext
                .UserRoles
                .AsNoTracking()
                .Where(r => r.RoleId == roleId)
                .Select(r => r.UserId);

            var users = await _dbContext
                .ApplicationUsers
                .AsNoTracking()
                .Where(u => userIds.Contains(u.Id))
                .ToArrayAsync();

            return _mapper.Map<ICollection<BaseUserDto>>(users);
        }

        public async Task<ICollection<RoleDto>> GetAllAsync()
        {
            var roles = await _dbContext
                .Roles
                .AsNoTracking()
                .ToArrayAsync();

            var rolesDtos = _mapper.Map<List<RoleDto>>(roles);
            rolesDtos.ForEach(r =>
            {
                r.UsersCount = GetUsersByRoleId(r.Id).GetAwaiter().GetResult().Count;
            });

            return rolesDtos;
        }

        public async Task<RoleDto?> GetByIdAsync(string roleId, bool disableTracking = true)
        {
            var rolesQueryable = _dbContext
                .Roles
                .AsNoTracking();
            if (disableTracking)
            {
                rolesQueryable = rolesQueryable.AsNoTracking();
            }

            var role = await rolesQueryable
                .FirstOrDefaultAsync(u => u.Id == roleId);

            return _mapper.Map<RoleDto>(role);
        }

        public async Task CreateAsync(BaseRoleDto roleDto)
        {
            await _dbContext.Roles.AddAsync(new IdentityRole()
            {
                Name = roleDto.Name,
                NormalizedName = roleDto.Name.Normalize(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            });

            await _dbContext.SaveChangesAsync();
        }

        public async Task EditAsync(BaseRoleDto roleDto)
        {
            var foundRole = await _dbContext
                .Roles
                .FirstOrDefaultAsync(r => r.Id == roleDto.Id);

            if (foundRole == null) return;

            foundRole.Name = roleDto.Name;
            foundRole.NormalizedName = roleDto.Name.Normalize();

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(string roleId)
        {
            var foundRole = await _dbContext
                .Roles
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if (foundRole == null) return;

            var connectedUserRoles = await _dbContext
                .UserRoles
                .Where(r => r.RoleId == roleId)
                .ToArrayAsync();

            _dbContext.UserRoles.RemoveRange(connectedUserRoles);
            _dbContext.Roles.Remove(foundRole);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> RoleExists(string roleName)
            => await _dbContext
                .Roles
                .AnyAsync(r => r.Name == roleName);

        public async Task<PaginatedRolesCollectionDto?> GetPaginatedRoles(int? page, int? pageSize)
        {
            var roles = await GetAllAsync();
            var paginator = new Paginator(roles.Count, page, pageSize, "Roles", false);

            var paginatedRoles =
                roles
                    .OrderByDescending(r => r.UsersCount)
                    .Skip((paginator.CurrentPage - 1) * paginator.PageSize)
                    .Take(paginator.PageSize);

            return new PaginatedRolesCollectionDto()
            {
                Roles = paginatedRoles.ToList(),
                Paginator = paginator
            };
        }
    }
}
