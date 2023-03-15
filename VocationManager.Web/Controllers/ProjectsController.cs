using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VocationManager.Data;
using VocationManager.Services.DTOs.Projects;
using VocationManager.Services.DTOs.Roles;
using VocationManager.Services.DTOs.Users;
using VocationManager.Services.ProjectsService;
using VocationManager.Services.RolesService;

namespace VocationManager.Web.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectsService _projectsService;

        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }

        public async Task<IActionResult> Index(int? page = 1, int? pageSize = 10, string keyword = "")
        {
            var projects = await _projectsService.GetPaginatedProjects(page, pageSize, keyword);
            return View(projects);
        }
        
        public async Task<IActionResult> Details(int id)
        {
            var project = await _projectsService.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [Authorize(Roles = "CEO,Team_Lead")]
        public IActionResult Create()
        {
            ViewBag.AvailableStatuses = _projectsService.GetAllEnumValuesAsKeyValuePairs<ProjectStatusType>();
            ViewBag.AvailablePriorities = _projectsService.GetAllEnumValuesAsKeyValuePairs<ProjectPriority>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CEO,Team_Lead")]
        public async Task<IActionResult> Create(CreateProjectDto projectDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _projectsService.CreateAsync(projectDto);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }

            if (ModelState.ErrorCount != 0)
            {
                ViewBag.AvailableStatuses = _projectsService.GetAllEnumValuesAsKeyValuePairs<ProjectStatusType>();
                ViewBag.AvailablePriorities = _projectsService.GetAllEnumValuesAsKeyValuePairs<ProjectPriority>();
                return View(projectDto);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "CEO,Team_Lead")]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _projectsService.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            ViewBag.AvailableStatuses = _projectsService.GetAllEnumValuesAsKeyValuePairs<ProjectStatusType>();
            ViewBag.AvailablePriorities = _projectsService.GetAllEnumValuesAsKeyValuePairs<ProjectPriority>();
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CEO,Team_Lead")]
        public async Task<IActionResult> Edit(int id, ProjectDto projectDto)
        {
            if (id != projectDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _projectsService.EditAsync(projectDto);
                return RedirectToAction(nameof(Index));
            }

            return View(projectDto);
        }


        [Authorize(Roles = "CEO,Team_Lead")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _projectsService.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CEO,Team_Lead")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _projectsService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
