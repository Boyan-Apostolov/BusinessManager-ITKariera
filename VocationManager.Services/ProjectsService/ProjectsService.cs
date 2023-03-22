using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VocationManager.Data;
using System.Data;
using VocationManager.Services.DTOs.Roles;
using VocationManager.Services.DTOs.Users;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using VocationManager.Services.DTOs.Projects;
using VocationManager.Services.RolesService;

namespace VocationManager.Services.ProjectsService
{
    public class ProjectsService : IProjectsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProjectsService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<string?> GetNameById(string id)
        {
            var role = await _dbContext
                .Roles
                .FirstOrDefaultAsync(r => r.Id == id);

            return role?.Name;
        }

        public async Task<ICollection<ProjectDto>> GetAllAsync()
        {
            var projects = await _dbContext
                .Projects
                .AsNoTracking()
                .ToArrayAsync();

            var projectsDtos = _mapper.Map<List<ProjectDto>>(projects);

            return projectsDtos;
        }

        public async Task<ProjectDto?> GetByIdAsync(int projectId, bool disableTracking = true)
        {
            var projectsQueryable = _dbContext
                .Projects
                .Include(p => p.Team)
                .AsQueryable();
            if (disableTracking)
            {
                projectsQueryable = projectsQueryable.AsNoTracking();
            }

            var project = await projectsQueryable
                .FirstOrDefaultAsync(u => u.Id == projectId);

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<int> CreateAsync(CreateProjectDto projectDto)
        {
            var projectToAdd = _mapper.Map<Project>(projectDto);

            await _dbContext.Projects.AddAsync(projectToAdd);

            await _dbContext.SaveChangesAsync();

            return projectToAdd.Id;
        }

        public async Task EditAsync(ProjectDto projectDto)
        {
            var foundProject = await _dbContext
                .Projects
                .FirstOrDefaultAsync(r => r.Id == projectDto.Id);

            if (foundProject == null) return;

            foundProject.Name = projectDto.Name;
            foundProject.Description = projectDto.Description;
            foundProject.Status = projectDto.Status;
            foundProject.Priority = projectDto.Priority;


            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int projectId)
        {
            var foundProject = await _dbContext
                .Projects
                .FirstOrDefaultAsync(r => r.Id == projectId);

            if (foundProject == null) return;

            _dbContext.Projects.Remove(foundProject);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<PaginatedProjectsCollectionDto?> GetPaginatedProjects(int? page, int? pageSize, string keyword)
        {
            var projects = await GetAllAsync();
            var paginator = new Paginator(projects.Count, page, pageSize, "Projects", true);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                projects = projects
                    .Where(u => u.Name.Contains(keyword, StringComparison.InvariantCultureIgnoreCase)
                                || u.Description.Contains(keyword, StringComparison.InvariantCultureIgnoreCase)
                                || u.Status.ToString().Contains(keyword, StringComparison.InvariantCultureIgnoreCase)
                                || u.Priority.ToString().Contains(keyword, StringComparison.InvariantCultureIgnoreCase))
                    .ToArray();
            }

            var paginatedProjects =
                projects
                    .Skip((paginator.CurrentPage - 1) * paginator.PageSize)
                    .Take(paginator.PageSize);

            return new PaginatedProjectsCollectionDto()
            {
                Projects = paginatedProjects.ToList(),
                Paginator = paginator
            };
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllEnumValuesAsKeyValuePairs<T>()
        {
            var kvps = new List<KeyValuePair<string, string>>();
            foreach (var statusName in Enum.GetNames(typeof(T)))
            {
                kvps.Add(new KeyValuePair<string, string>(statusName, statusName));
            }

            return kvps;
        }
    }
}
