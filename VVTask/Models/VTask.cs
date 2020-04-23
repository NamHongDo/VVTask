using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VVTask.Models
{
    public class VTask
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Point { get; set; }
        public bool Done { get; set; }
        public VType VType { get; set; }
    }
}
