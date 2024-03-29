﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using VacationManager.Models;
using BusinessManager.Services.DTOs.Roles;
using BusinessManager.Services.DTOs.Teams;
using BusinessManager.Services.DTOs.TimeOffs;
using BusinessManager.Services.TimeOffsService;
using System.Security.Claims;
using BusinessManager.Services.DTOs.Projects;
using BusinessManager.Services.FileUploadService;

namespace VacationManager.Controllers
{
    [Authorize]
    public class TimeOffsController : Controller
    {
        private readonly ITimeOffsService _timeOffsService;
        private readonly IFileUploadService _fileUploadService;

        public TimeOffsController(ITimeOffsService timeOffsService, 
            IFileUploadService fileUploadService)
        {
            _timeOffsService = timeOffsService;
            _fileUploadService = fileUploadService;
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

        public async Task<FileContentResult> DownloadTimeOffFile(string path)
        {
            return await _fileUploadService.DownloadFileAsync(path);
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
            ModelState.Remove(nameof(TimeOffRequestDto.RequestedBy));
            ModelState.Remove(nameof(TimeOffRequestDto.ExternalFileUrl));

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

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _timeOffsService.DeleteAsync(id);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            await _timeOffsService.ApproveRequest(id);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeclineRequest(int id)
        {
            await _timeOffsService.DeclineRequest(id);
            return Ok();
        }
    }
}
