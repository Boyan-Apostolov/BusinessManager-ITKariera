using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocationManager.Data
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public ProjectStatusType Status { get; set; }
        public ProjectPriority Priority { get; set; }
    }
}
