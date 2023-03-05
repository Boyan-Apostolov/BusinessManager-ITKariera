using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VocationManager.Data;
using VocationManager.Services.DTOs;

namespace VocationManager.Services.UsersService
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersService(ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<ICollection<ApplicationUser>> GetAllAsync()
        {
            return await _dbContext
                .ApplicationUsers
                .ToListAsync();
        }

        public async Task<ApplicationUser?> GetByIdAsync(string userId)
        {
            return await
                _dbContext
                    .ApplicationUsers
                    .FirstOrDefaultAsync(u => u.Id == userId);
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
                //var role = await GetNameById(userDto.SelectedRole);
                //await _userManager.AddToRoleAsync(userToBeCreated, role);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(string userId)
        {
            var user = await GetByIdAsync(userId);
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
