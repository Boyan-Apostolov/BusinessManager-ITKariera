using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocationManager.Services.DTOs.Roles
{
    public class PaginatedRolesCollectionDto
    {
        public ICollection<RoleDto> Roles { get; set; }

        public Paginator Paginator { get; set; }
    }
}
