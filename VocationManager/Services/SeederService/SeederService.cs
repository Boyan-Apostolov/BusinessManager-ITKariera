using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VocationManager.Data;

namespace VocationManager.Services.SeederService
{
    public class SeederService : ISeederService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly List<string> DefaultRoles;
        private readonly List<UserWithRoleDto> DefaultUsers;

        public SeederService(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            DefaultRoles = new List<string>
            {
                "CEO", "Developer", "Team Lead", "Unassigned"
            };

            DefaultUsers = new List<UserWithRoleDto>
            {
                new UserWithRoleDto()
                {
                    Username = "admin",
                    Email = "admin@voc-manager.com",
                    FirstName = "Global",
                    LastName = "Administrator",
                    Password = "Admin123",
                    RoleName = DefaultRoles[0],
                },
                new UserWithRoleDto()
                {
                    Username = "dev",
                    Email = "dev@voc-manager.com",
                    FirstName = "Jeorge",
                    LastName = "Devvy",
                    Password = "Developer123",
                    RoleName = DefaultRoles[1],
                },
                new UserWithRoleDto()
                {
                    Username = "team-lead",
                    Email = "team-lead@voc-manager.com",
                    FirstName = "Michael",
                    LastName = "Bossy",
                    Password = "Team-lead123",
                    RoleName = DefaultRoles[2],
                },
                new UserWithRoleDto()
                {
                    Username = "new-user",
                    Email = "user-user@voc-manager.com",
                    FirstName = "New",
                    LastName = "User",
                    Password = "New-user123",
                    RoleName = DefaultRoles[3],
                },
            };
        }
        public async Task InitiateSeed()
        {
            await SeedRoles();
            await SeedUsers();
        }

        public async Task SeedRoles()
        {
            if (!await _roleManager.Roles.AnyAsync())
            {
                DefaultRoles.ForEach(async rn =>
                {
                    await _dbContext.Roles.AddAsync(new IdentityRole()
                    {
                        Name = rn,
                        NormalizedName = rn.Normalize(),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                    });
                });

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task  SeedUsers()
        {
            if (!_userManager.Users.Any())
            {
                foreach (var user in DefaultUsers)
                {
                    var userToBeCreated = new ApplicationUser()
                    {
                        UserName = user.Username,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                    };

                    var createdUser = await _userManager.CreateAsync(userToBeCreated, user.Password);

                    if (createdUser.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(userToBeCreated, user.RoleName);
                    }
                }

                await _dbContext.SaveChangesAsync();
            }
        }
    }

    public class UserWithRoleDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string RoleName { get; set; }

        public string Password { get; set; }
    }
}
