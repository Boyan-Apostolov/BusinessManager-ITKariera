using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VocationManager.Services.DTOs.Teams;

namespace VocationManager.Services.DTOs.TimeOffs
{
    public class PaginatedTimeOffsCollectionDto
    {
        public ICollection<TimeOffRequestDto> Requests { get; set; }

        public Paginator Paginator { get; set; }
    }
}
