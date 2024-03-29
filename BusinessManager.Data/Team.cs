﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessManager.Data
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Project> Projects { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
