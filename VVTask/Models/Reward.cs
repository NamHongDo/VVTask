using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VVTask.Models
{
    public class Reward
    {
        public int RewardId { get; set; }
        public string Description { get; set; }
        public int Point { get; set; }
        public bool Acquired { get; set; } = false;
        public Kid Kid { get; set; }
        public int KidId { get; set; }
    }
}
