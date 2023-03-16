using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VocationManager.Services.DTOs.Roles;

namespace VocationManager.Services.DTOs.Teams
{
    public class PaginatedTeamsCollectionDto
    {
        public ICollection<TeamDto> Teams { get; set; }

        public Paginator Paginator { get; set; }
    }
}
