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
    public class CreateTimeOffRequestDto
    {
        [DataType(DataType.Date)]
        public DateTime From { get; set; }

        [DataType(DataType.Date)]
        public DateTime? To { get; set; }

        [Display(Name = "Is half day?")]
        public bool IsHalfDay { get; set; }

        public TimeOffType Type { get; set; }
    }
}
