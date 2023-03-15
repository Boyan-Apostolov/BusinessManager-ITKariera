using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VocationManager.Data;

namespace VocationManager.Services.DTOs.Projects
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public ProjectStatusType Status { get; set; }
        public ProjectPriority Priority { get; set; }
    }
}
