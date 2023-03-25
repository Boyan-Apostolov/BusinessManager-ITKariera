using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessManager.Data;
using BusinessManager.Services.DTOs.Users;
using BusinessManager.Services.RolesService;

namespace BusinessManager.Services.UsersService
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IRolesService _rolesService;

        public UsersService(ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IRolesService rolesService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = mapper;
            _rolesService = rolesService;
        }

        public async Task<ICollection<BaseUserDto>> GetAllAsync()
        {
            var users = await _dbContext
                .ApplicationUsers
                .Include(u => u.Team)
                .AsNoTracking()
                .ToListAsync();

            var usersDtos = _mapper.Map<List<BaseUserDto>>(users);
            usersDtos.ForEach(u =>
            {
                u.RoleName = _rolesService.GetRoleNameByUserId(u.Id).GetAwaiter().GetResult();
            });
            return usersDtos;

        }

        public async Task<PaginatedUsersCollectionDto> GetPaginatedAndFilteredUsers(int? page, int? pageSize, string keyword)
        {
            var users = await GetAllAsync();
            var paginator = new Paginator(users.Count, page, pageSize, "Users", true);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                users = users
                    .Where(u => u.Username.Contains(keyword, StringComparison.InvariantCultureIgnoreCase)
                                || u.FirstName.Contains(keyword, StringComparison.InvariantCultureIgnoreCase)
                                || u.LastName.Contains(keyword, StringComparison.InvariantCultureIgnoreCase)
                                || u.RoleName.Contains(keyword, StringComparison.InvariantCultureIgnoreCase))
                    .ToArray();
            }

            var paginatedUsers =
                users
                    .Skip((paginator.CurrentPage - 1) * paginator.PageSize)
                    .Take(paginator.PageSize);

            return new PaginatedUsersCollectionDto()
            {
                Users = paginatedUsers.ToList(),
                Paginator = paginator
            };
        }

        public async Task<BaseUserDto?> GetByIdAsync(string userId)
        {
            var user = await _dbContext
                .ApplicationUsers
                .Include(u => u.Team)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var mappedUser = _mapper.Map<BaseUserDto>(user);
            mappedUser.RoleName = await _rolesService.GetRoleNameByUserId(mappedUser.Id);
            return mappedUser;
        }

        public async Task CreateAsync(CreateUserDto userDto)
        {
            var userToBeCreated = new ApplicationUser()
            {
                UserName = userDto.Username,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
            };

            var createdUser = await _userManager.CreateAsync(userToBeCreated, userDto.Password);

            if (createdUser.Succeeded)
            {
                var role = await _rolesService.GetNameById(userDto.SelectedRole);
                await _userManager.AddToRoleAsync(userToBeCreated, role);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task EditAsync(BaseUserDto userDto)
        {
            var domainUser = await _dbContext
                .ApplicationUsers
                .FirstOrDefaultAsync(u => u.Id == userDto.Id);

            if (domainUser == null) return;

            domainUser.FirstName = userDto.FirstName;
            domainUser.LastName = userDto.LastName;
            domainUser.Email = userDto.Email;
            domainUser.NormalizedEmail = userDto.Email.Normalize();
            domainUser.UserName = userDto.Username;
            domainUser.NormalizedUserName = userDto.Username.Normalize();

            var currentRoleName = await _rolesService.GetRoleNameByUserId(userDto.Id);
            var newRole = await _rolesService.GetNameById(userDto.RoleName);

            if (currentRoleName != newRole)
            {
                await _userManager.RemoveFromRoleAsync(domainUser, currentRoleName);
                await _userManager.AddToRoleAsync(domainUser, newRole);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(string userId)
        {
            var user = await _dbContext
                .ApplicationUsers
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return;

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UserExists(string userId)
        {
            return await _dbContext
                .ApplicationUsers
                .AnyAsync(u => u.Id == userId);
        }
    }
}
