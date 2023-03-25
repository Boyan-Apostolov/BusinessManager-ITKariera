using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessManager.Services.DTOs.Roles
{
    public class RoleDto : BaseRoleDto
    {
        public int UsersCount { get; set; }
    }
}
