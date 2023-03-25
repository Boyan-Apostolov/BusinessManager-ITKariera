﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Models;
using VocationManager.Data;

namespace VocationManager.Services.DTOs.TimeOffs
{
    public class CreateTimeOffRequestDto
    {
        public DateTime From { get; set; }

        public DateTime? To { get; set; }

        public bool IsHalfDay { get; set; }

        public TimeOff Type { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ExternalFileUrl { get; set; }
    }
}
