using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVTask.Models;

namespace VVTask.ViewModels
{
    public class AddVTaskViewModel
    {
        public int VTaskId { get; set; }
        public string Description { get; set; }
        public int Point { get; set; }
        public int KidId { get; set; }
    }
}
