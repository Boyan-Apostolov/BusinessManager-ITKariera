﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VocationManager.Services.DTOs.Roles;
using VocationManager.Services.DTOs.Teams;
using VocationManager.Services.RolesService;
using VocationManager.Services.TeamsService;

namespace VocationManager.Web.Controllers
{
    [Authorize]
    public class TeamsController : Controller
    {
        private readonly ITeamsService _teamsService;

        public TeamsController(ITeamsService teamsService)
        {
            _teamsService = teamsService;
        }

        public async Task<IActionResult> Index(int? page = 1, int? pageSize = 10, string keyWord = "")
        {
            var teams = await _teamsService.GetPaginatedTeams(page, pageSize, keyWord);
            return View(teams);
        }

        public async Task<IActionResult> Details(int id)
        {
            var team = await _teamsService.GetByIdAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        [Authorize(Roles = "CEO,Team_Lead")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CEO,Team_Lead")]
        public async Task<IActionResult> Create(CreateTeamDto teamDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _teamsService.CreateAsync(teamDto);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }

            if (ModelState.ErrorCount != 0)
            {
                return View(teamDto);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "CEO,Team_Lead")]
        public async Task<IActionResult> Edit(int id)
        {
            var team = await _teamsService.GetByIdAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CEO,Team_Lead")]
        public async Task<IActionResult> Edit(int id, TeamDto teamDto)
        {
            if (id != teamDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _teamsService.EditAsync(teamDto);
                return RedirectToAction(nameof(Index));
            }

            return View(teamDto);
        }


        [Authorize(Roles = "CEO,Team_Lead")]
        public async Task<IActionResult> Delete(int id)
        {
            var team = await _teamsService.GetByIdAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CEO,Team_Lead")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _teamsService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
