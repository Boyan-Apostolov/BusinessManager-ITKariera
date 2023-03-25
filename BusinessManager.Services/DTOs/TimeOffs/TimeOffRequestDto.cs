using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Models;
using BusinessManager.Data;
using BusinessManager.Data.Enums;

namespace BusinessManager.Services.DTOs.TimeOffs
{
    public class TimeOffRequestDto
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime From { get; set; }

        [DataType(DataType.Date)]
        public DateTime? To { get; set; }

        public bool IsHalfDay { get; set; }

        public TimeOffType Type { get; set; }

        public bool? IsApproved { get; set; }

        public string RequestedById { get; set; }
        public ApplicationUser RequestedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ExternalFileUrl { get; set; }
    }
}
