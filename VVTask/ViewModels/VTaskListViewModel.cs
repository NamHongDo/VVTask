using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVTask.Models;

namespace VVTask.ViewModels
{
    public class VTaskListViewModel
    {
        public IEnumerable<VTask> VTasks { get; set; }
        public int KidId { get; set; }
    }
}
