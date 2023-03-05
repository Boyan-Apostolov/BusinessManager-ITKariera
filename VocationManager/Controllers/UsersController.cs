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
using VocationManager.Services.UsersService;

namespace VocationManager.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUsersService _usersService;

        public UsersController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IUsersService usersService)
        {
            _context = context;
            _userManager = userManager;
            _usersService = usersService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _usersService.GetAllAsync();
            return View(users);
        }

        [Authorize(Roles = "CEO,Team_Lead")]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _usersService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
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
                try
                {
                    await _usersService.CreateAsync(userDto);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }

            if (ModelState.ErrorCount != 0)
            {
                ViewBag.AvailableRoles = GetAllAsKeyValuePairs();
                return View(userDto);
            }

            return RedirectToAction(nameof(Index));
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
            var user = await _usersService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(new BaseUserDto(user));
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
            var user = await _usersService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _usersService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
