using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVTask.Models;

namespace VVTask.ViewModels
{
    public class KidProfileViewModel
    {
        public IEnumerable<Kid> Profiles { get; set; }
        public string userName { get; set; }
    }
}
