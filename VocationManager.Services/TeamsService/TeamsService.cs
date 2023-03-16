using AutoMapper;
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

namespace VocationManager.Services.TeamsService
{
    public class TeamsService : ITeamsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public TeamsService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
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
                .AsQueryable();
            if (disableTracking)
            {
                teamsQueryable = teamsQueryable.AsNoTracking();
            }

            var team = await teamsQueryable
                .FirstOrDefaultAsync(u => u.Id == teamId);

            return _mapper.Map<TeamDto>(team);
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
                    .Skip((paginator.CurrentPage - 1) * paginator.PageSize)
                    .Take(paginator.PageSize);

            return new PaginatedTeamsCollectionDto()
            {
                Teams = paginatedTeams.ToList(),
                Paginator = paginator
            };
        }
    }
}
