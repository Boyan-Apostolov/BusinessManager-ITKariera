using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VocationManager.Data;
using VocationManager.Services.DTOs.Projects;
using VocationManager.Services.DTOs.Users;

namespace VocationManager.Services.DTOs.Teams
{
    public class TeamDto
    {
        public TeamDto()
        {
            Projects = new List<ProjectDto>();
            Users = new List<BaseUserDto>();
        }

        public int Id { get; set; }

        [Display(Name = "Team Name")]
        public string Name { get; set; }

        public ICollection<ProjectDto> Projects { get; set; }

        public ICollection<BaseUserDto> Users { get; set; }
    }
}
