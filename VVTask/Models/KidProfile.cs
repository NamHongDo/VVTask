using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VVTask.Models
{
    public class KidProfile
    {
        [Key]
        public int KidId { get; set; }
        public string KidName { get; set; }
        public int AccumultedPoint { get; set; } = 0;
        public IList<VTask> VTasks { get; set; }
    }
}
