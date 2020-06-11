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

namespace Scryber.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPDFDocumentService _docService;
        private readonly IWebHostEnvironment _env;
        private readonly string _absPath;

        private const string SamplesPath = "../Samples/";
        

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment, IPDFDocumentService documentService)
        {
            _docService = documentService;
            _logger = logger;
            _env = environment;
            _absPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(environment.WebRootPath, SamplesPath));
        }

        public IActionResult Index()
        {
            var allDocs = _docService.GetDocumentReferences(_absPath);
            return View(allDocs);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [ResponseCache(NoStore = true)]
        public IActionResult Generate(string name)
        {
            name = Uri.UnescapeDataString(name);
            var reference = _docService.GetAReference(_absPath, name);
            if (null == reference)
                return NotFound(name);
            else
            {
                var doc = _docService.GetDocument(reference);

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                doc.ProcessDocument(ms);
                ms.Flush();
                ms.Position = 0;
                
                return new FileStreamResult(ms, "application/pdf");
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
