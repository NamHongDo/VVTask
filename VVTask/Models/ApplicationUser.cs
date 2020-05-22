using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VVTask.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Kid> Kids { get; set; }
    }
}
