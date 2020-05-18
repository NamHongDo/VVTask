using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVTask.Models;

namespace VVTask.ViewModels
{
    public class KidDetailsViewModel
    {
        public Kid kid { get; set; }
        public IEnumerable<VTask> currentKidVTasks { get; set; }
    }
}
