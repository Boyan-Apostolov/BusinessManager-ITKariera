using System;
using System.ComponentModel.DataAnnotations;
using BusinessManager.Data;
using BusinessManager.Data.Enums;

namespace VacationManager.Models
{
    public class TimeOff
    {
        public int Id { get; set; }

        public DateTime From { get; set; }

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
