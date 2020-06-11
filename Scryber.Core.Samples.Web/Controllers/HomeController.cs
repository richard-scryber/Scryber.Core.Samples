using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Scryber.Core.Samples.Web.Models;
using Scryber.Core.Samples.Web;
using Microsoft.Extensions;
using Scryber.Components;
using Scryber.Components.Mvc;

namespace Scryber.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        private readonly IWebHostEnvironment _env;
        private readonly string _absPath;

        private const string SamplesPath = "Samples";
        

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment)
        {
            
            _logger = logger;
            _env = environment;
            _absPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(environment.ContentRootPath, SamplesPath));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult HelloWorld()
        {
            using (var doc = new PDFDocument())
            {
                PDFPage pg = new PDFPage();
                doc.Pages.Add(pg);
                PDFLabel label = new PDFLabel();
                label.Text = "Hello World";

                pg.Contents.Add(label);

                return this.PDF(doc);

            }
            
        }

        public IActionResult HelloTemplate()
        {
            string path = _env.ContentRootPath;
            path = System.IO.Path.Combine(path, "Views", "PDF", "HelloWorld.pdfx");

            return this.PDF(path);
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
