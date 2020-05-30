using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VVTask.ViewModels
{
    public class KidCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        public string ApplicationUserId { get; set; }
        public IFormFile Photo { get; set; }
    }
}
