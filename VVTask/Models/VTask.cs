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
        [Key]
        public int VTaskId { get; set; }
        public string Description { get; set; }
        public int Point { get; set; }
        public bool Done { get; set; }
        public VType VType { get; set; }
        public int KidId { get; set; }
        public KidProfile KidProfile { get; set; }
    }
}
