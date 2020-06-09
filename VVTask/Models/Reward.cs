using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VVTask.Models
{
    public class Reward
    {
        public int RewardId { get; set; }
        [Required]
        public string RewardName { get; set; }
        public string Description { get; set; }
        [Required]
        public int Point { get; set; }
        public bool Acquired { get; set; } = false;
        public Kid Kid { get; set; }
        public int KidId { get; set; }
    }
}