using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using VacationManager.Models;
using VocationManager.Services.DTOs.Roles;
using VocationManager.Services.DTOs.Teams;
using VocationManager.Services.DTOs.TimeOffs;
using VocationManager.Services.TimeOffsService;
using System.Security.Claims;

namespace VacationManager.Controllers
{
    [Authorize]
    public class TimeOffsController : Controller
    {
        private readonly ITimeOffsService _timeOffsService;

        public TimeOffsController(ITimeOffsService timeOffsService)
        {
            _timeOffsService = timeOffsService;
        }

        public async Task<IActionResult> Index(int? page = 1, int? pageSize = 10, string keyWord = "")
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var requests = await _timeOffsService.GetPaginatedRequests(userId, page, pageSize, keyWord);
            return View(requests);
        }

        public IActionResult Create()
        {
            ViewBag.RequestTypes = _timeOffsService.GetAllRequestTypesAsKeyValuePairs();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTimeOffRequestDto timeOffDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    await _timeOffsService.CreateAsync(userId, timeOffDto);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }

            if (ModelState.ErrorCount != 0)
            {
                ViewBag.RequestTypes = _timeOffsService.GetAllRequestTypesAsKeyValuePairs();
                return View(timeOffDto);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var request = await _timeOffsService.GetByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            ViewBag.RequestTypes = _timeOffsService.GetAllRequestTypesAsKeyValuePairs();
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TimeOffRequestDto requestDto)
        {
            if (id != requestDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _timeOffsService.EditAsync(requestDto);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.RequestTypes = _timeOffsService.GetAllRequestTypesAsKeyValuePairs();
            return View(requestDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var request = await _timeOffsService.GetByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _timeOffsService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
