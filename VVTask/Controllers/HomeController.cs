﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VVTask.Controllers
{ 
    public class HomeController : Controller
    {
        public HomeController()
        {
           
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}