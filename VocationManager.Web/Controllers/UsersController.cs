using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VocationManager.Data;
using VocationManager.Services.DTOs.Users;
using VocationManager.Services.RolesService;
using VocationManager.Services.TeamsService;
using VocationManager.Services.UsersService;

namespace VocationManager.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IRolesService _rolesService;
        private readonly ITeamsService _teamsService;

        public UsersController(IUsersService usersService,
            IRolesService rolesService, ITeamsService teamsService)
        {
            _usersService = usersService;
            _rolesService = rolesService;
            _teamsService = teamsService;
        }

        public async Task<IActionResult> Index(int? page = 1, int? pageSize = 10, string keyword = null)
        {
            var users = await _usersService.GetPaginatedAndFilteredUsers(page, pageSize, keyword);
            return View(users);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _usersService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.AvailableTeams = _teamsService.GetAllAsKeyValuePairs();
            return View(user);
        }

        [Authorize(Roles = "CEO")]
        public IActionResult Create()
        {
            ViewBag.AvailableRoles = _rolesService.GetAllAsKeyValuePairs();
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
                ViewBag.AvailableRoles = _rolesService.GetAllAsKeyValuePairs();
                return View(userDto);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _usersService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.AvailableRoles = _rolesService.GetAllAsKeyValuePairs();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> Edit(string id, BaseUserDto userDto)
        {
            ModelState.Remove(nameof(BaseUserDto.Team));
            ModelState.Remove(nameof(BaseUserDto.TeamId));

            if (id != userDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _usersService.EditAsync(userDto);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AvailableRoles = _rolesService.GetAllAsKeyValuePairs();
            return View(userDto);
        }


        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> Delete(string id)
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

        [HttpPost]
        public async Task<IActionResult> AssignUserToTeam(string userId, int teamId)
        {
            await _teamsService.AssignUserToTeam(userId, teamId);
            return Ok(200);
        }    
        
        [HttpPost]
        public async Task<IActionResult> RemoveUserFromTeam(string userId, int teamId)
        {
            await _teamsService.RemoveUserFromTeam(userId, teamId);
            return Ok(200);
        }
    }
}
