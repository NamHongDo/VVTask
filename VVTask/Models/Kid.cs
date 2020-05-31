using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VVTask.Models
{
    public class Kid
    {
        public int KidId { get; set; }
        [Required , MaxLength(40)]
        public string Name { get; set; }
        [Required]
        public int TotalPoint { get; set; } = 0;
        public List<VTask> VTasks { get; set; }
        public List<Reward> Rewards { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string PhotoPath { get; set; }
    }
}
