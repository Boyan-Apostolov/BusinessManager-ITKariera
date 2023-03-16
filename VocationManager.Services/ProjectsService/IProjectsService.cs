using VocationManager.Services.DTOs.Projects;
using PaginatedRolesCollectionDto = VocationManager.Services.DTOs.Roles.PaginatedRolesCollectionDto;

namespace VocationManager.Services.ProjectsService
{
    public interface IProjectsService
    {
        Task<string?> GetNameById(string id);

        Task<ICollection<ProjectDto>> GetAllAsync();
        
        Task<ProjectDto?> GetByIdAsync(int projectId, bool disableTracking = true);
        
        Task<int> CreateAsync(CreateProjectDto roleDto);
        
        Task EditAsync(ProjectDto roleDto);
        
        Task DeleteAsync(int projectId);
        
        Task<PaginatedProjectsCollectionDto?> GetPaginatedProjects(int? page, int? pageSize, string keyword);

        IEnumerable<KeyValuePair<string, string>> GetAllEnumValuesAsKeyValuePairs<T>();
    }
}
