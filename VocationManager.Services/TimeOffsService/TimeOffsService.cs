using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using VocationManager.Data;
using VocationManager.Services.DTOs.Teams;
using VocationManager.Services.DTOs.TimeOffs;
using System.Net.Http;
using System.Security.Claims;
using VacationManager.Models;
using VocationManager.Data.Enums;

namespace VocationManager.Services.TimeOffsService
{
    public class TimeOffsService : ITimeOffsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public TimeOffsService(ApplicationDbContext dbContext,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<ICollection<TimeOffRequestDto>> GetAllAsync(string userId)
        {
            var user = await _dbContext
                .ApplicationUsers
                .Include(u => u.Team)
                .ThenInclude(t => t.Users)
                .SingleAsync(u => u.Id == userId);
            var roles = await _userManager.GetRolesAsync(user);

            //TODO: Get All based on current role
            var requestsQueryable = _dbContext
                .TimeOffs
                .Include(t => t.RequestedBy)
                .AsNoTracking();

            var foundRequests = new List<TimeOff>();
            if (roles.Contains(Enum.GetName(DefaultRoles.CEO)) || roles.Contains(Enum.GetName(DefaultRoles.Team_Lead)))
            {
                if (roles.Contains(Enum.GetName(DefaultRoles.Team_Lead)))
                {
                    var usersInCurrentLeaderTeam = user.Team.Users.Select(u => u.Id);
                    foundRequests = await requestsQueryable
                        .Where(r => usersInCurrentLeaderTeam.Contains(r.RequestedById))
                        .ToListAsync();
                }
                else
                {
                    foundRequests = await requestsQueryable.ToListAsync();
                }
            }
            else
            {
                foundRequests = await requestsQueryable
                    .Where(r => r.RequestedById == userId)
                    .ToListAsync();
            }

            return _mapper.Map<List<TimeOffRequestDto>>(foundRequests);
        }

        public async Task<TimeOffRequestDto?> GetByIdAsync(int requestId, bool disableTracking = true)
        {
            var requestQueryable = _dbContext
                .TimeOffs
                .Include(t => t.RequestedBy)
                .AsQueryable();
            if (disableTracking)
            {
                requestQueryable = requestQueryable.AsNoTracking();
            }

            var request = await requestQueryable
                .FirstOrDefaultAsync(u => u.Id == requestId);

            var mappedRequest = _mapper.Map<TimeOffRequestDto>(request);

            return mappedRequest;
        }

        public async Task<int> CreateAsync(string userId, CreateTimeOffRequestDto timeOffRequestDto)
        {
            timeOffRequestDto.CreatedOn = DateTime.Now;
            timeOffRequestDto.ExternalFileUrl = "https://example.com";

            var newRequest = _mapper.Map<TimeOff>(timeOffRequestDto);
            newRequest.RequestedById = userId;

            await _dbContext.TimeOffs.AddAsync(newRequest);

            await _dbContext.SaveChangesAsync();

            return newRequest.Id;
        }

        public async Task EditAsync(TimeOffRequestDto timeOffRequestDto)
        {
            var foundRequest = await _dbContext
                .TimeOffs
                .FirstOrDefaultAsync(r => r.Id == timeOffRequestDto.Id);

            if (foundRequest == null) return;

            foundRequest.From = timeOffRequestDto.From;
            foundRequest.To = timeOffRequestDto.To;
            foundRequest.IsHalfDay = timeOffRequestDto.IsHalfDay;
            foundRequest.IsApproved = timeOffRequestDto.IsApproved;
            foundRequest.Type = timeOffRequestDto.Type;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int requestId)
        {
            var foundRequest = await _dbContext
                .TimeOffs
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (foundRequest == null) return;

            _dbContext.TimeOffs.Remove(foundRequest);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<PaginatedTimeOffsCollectionDto?> GetPaginatedRequests(string userId, int? page, int? pageSize, string keyWord)
        {
            var requests = await GetAllAsync(userId);
            var paginator = new Paginator(requests.Count, page, pageSize, "TimeOffs", true);

            if (!string.IsNullOrWhiteSpace(keyWord))
            {
               //Try parse date and filter
            }

            var paginatedRequests =
                requests
                    .Skip((paginator.CurrentPage - 1) * paginator.PageSize)
                    .Take(paginator.PageSize);

            return new PaginatedTimeOffsCollectionDto()
            {
                Requests = paginatedRequests.ToList(),
                Paginator = paginator
            };
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllRequestTypesAsKeyValuePairs()
        {
            var kvps = new List<KeyValuePair<string, string>>();
            foreach (var statusName in Enum.GetNames(typeof(TimeOffType)))
            {
                kvps.Add(new KeyValuePair<string, string>(statusName, statusName));
            }

            return kvps;
        }
    }
}
