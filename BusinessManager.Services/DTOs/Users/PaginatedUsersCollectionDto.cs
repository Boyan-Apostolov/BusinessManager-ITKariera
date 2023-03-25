using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessManager.Services.DTOs.Users
{
    public class PaginatedUsersCollectionDto
    {
        public ICollection<BaseUserDto> Users { get; set; }

        public Paginator Paginator { get; set; }
    }
}
