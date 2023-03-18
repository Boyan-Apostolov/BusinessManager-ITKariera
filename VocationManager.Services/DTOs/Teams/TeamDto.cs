using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VocationManager.Data;

namespace VocationManager.Services.DTOs.Teams
{
    public class TeamDto
    {
        public TeamDto()
        {
            Projects = new List<Project>();
            Users = new List<ApplicationUser>();
        }

        public int Id { get; set; }

        [Display(Name = "Team Name")]
        public string Name { get; set; }

        public ICollection<Project> Projects { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
