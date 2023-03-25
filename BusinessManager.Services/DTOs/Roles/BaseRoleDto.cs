using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessManager.Services.DTOs.Roles
{
    public class BaseRoleDto
    {
        public BaseRoleDto()
        {
            Id = "new-role";
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}
