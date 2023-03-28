using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Models;
using BusinessManager.Data;
using BusinessManager.Data.Enums;
using Microsoft.AspNetCore.Http;

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

        [Display(Name = "Upload external file")]
        public IFormFile? ExternalFile { get; set; }
    }
}
