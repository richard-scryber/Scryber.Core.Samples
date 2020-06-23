using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Scryber.Core.Samples.Web.Models;
using Scryber.Components;
using Scryber.Components.Mvc;

namespace Scryber.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _env;
        
        public HomeController(IWebHostEnvironment environment)
        {
            _env = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
