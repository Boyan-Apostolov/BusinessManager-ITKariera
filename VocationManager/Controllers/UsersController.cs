using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VocationManager.Data;
using VocationManager.Services.DTOs;

namespace VocationManager.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicationUsers.ToListAsync());
        }

        [Authorize(Roles = "CEO,Team_Lead")]
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.ApplicationUsers == null)
            {
                return NotFound();
            }

            var userDto = await _context.ApplicationUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userDto == null)
            {
                return NotFound();
            }

            return View(userDto);
        }

        [Authorize(Roles = "CEO")]
        public IActionResult Create()
        {
            ViewBag.AvailableRoles = GetAllAsKeyValuePairs();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> Create(CreateUserDto userDto)
        {
            if (ModelState.IsValid)
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
                    var role = await GetNameById(userDto.SelectedRole);
                    await _userManager.AddToRoleAsync(userToBeCreated, role);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AvailableRoles = GetAllAsKeyValuePairs();
            return View(userDto);
        }

        //TODO: Move to roles service
        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return _context
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

        public async Task<string> GetNameById(string id)
        {
            return (
                await _context
                .Roles
                .FindAsync(id)
                ).Name;
        }

        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.ApplicationUsers == null)
            {
                return NotFound();
            }

            var userDto = await _context.ApplicationUsers.FindAsync(id);
            if (userDto == null)
            {
                return NotFound();
            }

            return View(new BaseUserDto(userDto));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> Edit(string id, BaseUserDto userDto)
        {
            if (id != userDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var domainUser = await _context.ApplicationUsers
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (domainUser == null)
                {
                    return NotFound();
                }

                domainUser.FirstName = userDto.FirstName;
                domainUser.LastName = userDto.LastName;
                domainUser.Email = userDto.Email;
                domainUser.NormalizedEmail = userDto.Email.Normalize();
                domainUser.UserName = userDto.Username;
                domainUser.NormalizedUserName = userDto.Username.Normalize();

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(userDto);
        }


        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.ApplicationUsers == null)
            {
                return NotFound();
            }

            var userDto = await _context.ApplicationUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userDto == null)
            {
                return NotFound();
            }

            return View(userDto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.ApplicationUsers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.userDto'  is null.");
            }
            var userDto = await _context.ApplicationUsers.FindAsync(id);
            if (userDto != null)
            {
                _context.ApplicationUsers.Remove(userDto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.ApplicationUsers.Any(e => e.Id == id);
        }
    }
}
