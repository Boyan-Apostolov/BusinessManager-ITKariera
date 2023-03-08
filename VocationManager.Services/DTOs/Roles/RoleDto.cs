using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocationManager.Services.DTOs.Roles
{
    public class RoleDto : BaseRoleDto
    {
        public int UsersCount { get; set; }
    }
}
