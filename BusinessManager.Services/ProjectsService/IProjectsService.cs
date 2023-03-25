using BusinessManager.Services.DTOs.Projects;
using PaginatedRolesCollectionDto = BusinessManager.Services.DTOs.Roles.PaginatedRolesCollectionDto;

namespace BusinessManager.Services.ProjectsService
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
