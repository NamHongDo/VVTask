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
        public string Name { get; set; }
        public int TotalPoint { get; set; } = 0;
        public List<VTask> VTasks { get; set; }
        public List<Reward> Rewards { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
