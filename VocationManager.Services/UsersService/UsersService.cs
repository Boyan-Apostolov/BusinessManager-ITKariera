using AutoMapper;
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
        private readonly IMapper _mapper;

        public UsersService(ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ICollection<BaseUserDto>> GetAllAsync()
        {
            var users = await _dbContext
                .ApplicationUsers
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<ICollection<BaseUserDto>>(users);

        }

        public async Task<BaseUserDto?> GetByIdAsync(string userId, bool disableTracking = true)
        {
            var usersQueryable = _dbContext
                    .ApplicationUsers
                    .AsNoTracking();
            if (disableTracking)
            {
                usersQueryable = usersQueryable.AsNoTracking();
            }

            var user = await usersQueryable
                .FirstOrDefaultAsync(u => u.Id == userId);

            return _mapper.Map<BaseUserDto>(user);
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
            var user = await _dbContext
                .ApplicationUsers
                .FirstOrDefaultAsync(u=>u.Id == userId);
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
