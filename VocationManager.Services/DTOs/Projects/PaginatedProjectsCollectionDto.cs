using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VocationManager.Services.DTOs.Roles;

namespace VocationManager.Services.DTOs.Projects
{
    public class PaginatedProjectsCollectionDto
    {
        public ICollection<ProjectDto> Projects { get; set; }

        public Paginator Paginator { get; set; }
    }
}
