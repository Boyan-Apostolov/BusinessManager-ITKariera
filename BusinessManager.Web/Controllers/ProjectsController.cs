using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessManager.Data.Enums;
using BusinessManager.Services.DTOs.Projects;
using BusinessManager.Services.DTOs.Roles;
using BusinessManager.Services.DTOs.Users;
using BusinessManager.Services.ProjectsService;
using BusinessManager.Services.RolesService;
using BusinessManager.Services.TeamsService;

namespace BusinessManager.Web.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectsService _projectsService;
        private readonly ITeamsService _teamsService;

        public ProjectsController(IProjectsService projectsService,
            ITeamsService teamsService)
        {
            _projectsService = projectsService;
            _teamsService = teamsService;
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

            ViewBag.AvailableTeams = _teamsService.GetAllAsKeyValuePairs();
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
            var newProjectId = 0;
            if (ModelState.IsValid)
            {
                try
                {
                    newProjectId = await _projectsService.CreateAsync(projectDto);
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

            return RedirectToAction(nameof(Details), new { id = newProjectId });
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
            ModelState.Remove(nameof(ProjectDto.Team));
            ModelState.Remove(nameof(ProjectDto.TeamId));

            if (id != projectDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _projectsService.EditAsync(projectDto);
                return RedirectToAction(nameof(Details), new { id = projectDto.Id });
            }

            ViewBag.AvailableStatuses = _projectsService.GetAllEnumValuesAsKeyValuePairs<ProjectStatusType>();
            ViewBag.AvailablePriorities = _projectsService.GetAllEnumValuesAsKeyValuePairs<ProjectPriority>();
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

        [HttpPost]
        public async Task<IActionResult> AssignProjectToTeam(int projectId, int teamId)
        {
            await _teamsService.AssignProjectToTeam(projectId, teamId);
            return Ok(200);
        }
    }
}
