using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VVTask.ViewModels
{
    public class KidEditViewModel: KidCreateViewModel
    {
        public int KidId { get; set; }
        public int TotalPoint { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}
