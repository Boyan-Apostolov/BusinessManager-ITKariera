using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VocationManager.Services.DTOs.Teams;

namespace VocationManager.Services.TeamsService
{
    public interface ITeamsService
    {
        Task<string?> GetNameById(int id);
        Task<ICollection<TeamDto>> GetAllAsync();
        Task<TeamDto?> GetByIdAsync(int teamId, bool disableTracking = true);
        Task CreateAsync(CreateTeamDto teamDto);
        Task EditAsync(TeamDto teamDto);
        Task DeleteAsync(int teamId);
        Task<PaginatedTeamsCollectionDto?> GetPaginatedTeams(int? page, int? pageSize, string keyWord);
    }
}
