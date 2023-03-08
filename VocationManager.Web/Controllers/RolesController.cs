using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VocationManager.Services.DTOs.Roles;
using VocationManager.Services.DTOs.Users;
using VocationManager.Services.RolesService;

namespace VocationManager.Web.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        public async Task<IActionResult> Index(int? page = 1, int? pageSize = 10, string keyword = null)
        {
            //var users = await _usersService.GetPaginatedAndFilteredUsers(page, pageSize, keyword);
            var roles = await _rolesService.GetAllAsync();
            var orderedRoles = roles.OrderByDescending(x => x.UsersCount).ToArray();
            return View(orderedRoles);
        }

        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> Details(string id)
        {
            var role = await _rolesService.GetByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [Authorize(Roles = "CEO")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> Create(BaseRoleDto roleDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var roleExists = await _rolesService.RoleExists(roleDto.Name);
                    if (roleExists) throw new InvalidOperationException("Role already exists!");

                    await _rolesService.CreateAsync(roleDto);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }

            if (ModelState.ErrorCount != 0)
            {
                return View(roleDto);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _rolesService.GetByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> Edit(string id, BaseRoleDto roleDto)
        {
            if (id != roleDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _rolesService.EditAsync(roleDto);
                return RedirectToAction(nameof(Index));
            }

            return View(roleDto);
        }


        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _rolesService.GetByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _rolesService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
