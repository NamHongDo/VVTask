using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VVTask.Models
{
    public class VTask
    {
        public int VTaskId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Point { get; set; }
        public bool Done { get; set; } = false;
        public VType VType { get; set; }
        public Kid Kid { get; set; }
        public int KidId { get; set; }
    }
}
