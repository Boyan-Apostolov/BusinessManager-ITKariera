﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VocationManager.Data;
using VocationManager.Services.DTOs.Roles;
using VocationManager.Services.DTOs.Teams;
using VocationManager.Services.DTOs.Users;
using VocationManager.Services.RolesService;
using VocationManager.Services.UsersService;

namespace VocationManager.Services.TeamsService
{
    public class TeamsService : ITeamsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IRolesService _rolesService;

        public TeamsService(ApplicationDbContext dbContext,
            IMapper mapper, IRolesService rolesService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _rolesService = rolesService;
        }

        public async Task<string?> GetNameById(int id)
        {
            var team = await _dbContext
                .Teams
                .FirstOrDefaultAsync(r => r.Id == id);

            return team?.Name;
        }


        public async Task<ICollection<TeamDto>> GetAllAsync()
        {
            var teams = await _dbContext
                .Teams
                .Include(t => t.Projects)
                .Include(t => t.Users)
                .AsNoTracking()
                .ToArrayAsync();

            return _mapper.Map<List<TeamDto>>(teams);
        }

        public async Task<TeamDto?> GetByIdAsync(int teamId, bool disableTracking = true)
        {
            var teamsQueryable = _dbContext
                .Teams
                .Include(t => t.Projects)
                .Include(t => t.Users)
                .AsQueryable();
            if (disableTracking)
            {
                teamsQueryable = teamsQueryable.AsNoTracking();
            }

            var team = await teamsQueryable
                .FirstOrDefaultAsync(u => u.Id == teamId);

            var mappedTeam = _mapper.Map<TeamDto>(team);

            foreach (var user in mappedTeam.Users)
            {
                user.RoleName = await _rolesService.GetRoleNameByUserId(user.Id);
            }
            
            return mappedTeam;
        }

        public async Task<int> CreateAsync(CreateTeamDto teamDto)
        {
            var newTeam = _mapper.Map<Team>(teamDto);

            await _dbContext.Teams.AddAsync(newTeam);

            await _dbContext.SaveChangesAsync();

            return newTeam.Id;
        }

        public async Task EditAsync(TeamDto teamDto)
        {
            var foundTeam = await _dbContext
                .Teams
                .FirstOrDefaultAsync(r => r.Id == teamDto.Id);

            if (foundTeam == null) return;

            foundTeam.Name = teamDto.Name;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int teamId)
        {
            var foundTeam = await _dbContext
                .Teams
                .FirstOrDefaultAsync(r => r.Id == teamId);

            if (foundTeam == null) return;

            _dbContext.Teams.Remove(foundTeam);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<PaginatedTeamsCollectionDto?> GetPaginatedTeams(int? page, int? pageSize, string keyWord)
        {
            var teams = await GetAllAsync();
            var paginator = new Paginator(teams.Count, page, pageSize, "Teams", true);

            if (!string.IsNullOrWhiteSpace(keyWord))
            {
                keyWord = keyWord.Trim();
                teams = teams
                    .Where(u => u.Name.Contains(keyWord, StringComparison.InvariantCultureIgnoreCase))
                    .ToArray();
            }

            var paginatedTeams =
                teams
                    .OrderByDescending(t => t.Users.Count)
                    .ThenByDescending(t => t.Projects.Count)
                    .Skip((paginator.CurrentPage - 1) * paginator.PageSize)
                    .Take(paginator.PageSize);

            return new PaginatedTeamsCollectionDto()
            {
                Teams = paginatedTeams.ToList(),
                Paginator = paginator
            };
        }

        public IEnumerable<KeyValuePair<int, string>> GetAllAsKeyValuePairs()
        {
            return _dbContext
                .Teams
                .ToArray()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                })
                .OrderBy(x => x.Name)
                .ToList().Select(x => new KeyValuePair<int, string>(x.Id, x.Name));
        }

        public async Task AssignUserToTeam(string userId, int teamId)
        {
            var team = await _dbContext
                .Teams
                .Include(t => t.Users)
                .FirstOrDefaultAsync(t => t.Id == teamId);

            if (team == null) throw new InvalidOperationException("Team not found!");

            // ReSharper disable once SimplifyLinqExpressionUseAll
            if (!team.Users.Any(u => u.Id == userId))
            {
                var user = await _dbContext
                        .ApplicationUsers
                        .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null) throw new InvalidOperationException("User not found!");

                team.Users.Add(user);

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("User is already part of that team!");
            }
        }
    }
}
