using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Scryber.Models;
using Microsoft.Extensions;

namespace Scryber.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            using (var ms = new System.IO.MemoryStream())
            {
                var writer = new OpenType.BigEndianWriter(ms);
                UInt16 _uint = (UInt16)20;
                
                writer.WriteUInt16(20);

                ms.Position = 0;

                var reader = new OpenType.BigEndianReader(ms);
                UInt16 end = reader.ReadUInt16();

                if (_uint != end)
                    throw new InvalidOperationException("Not the same");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [ResponseCache(NoStore = true)]
        public IActionResult Generate()
        {
            var path = AppContext.BaseDirectory;
            path = System.IO.Path.Combine(path, "../../../Samples/Document.pdfx");
            path = System.IO.Path.GetFullPath(path);
            if (System.IO.File.Exists(path))
            {
                var doc = Scryber.Components.PDFDocument.ParseDocument(path);

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                doc.ProcessDocument(ms);
                ms.Flush();
                ms.Position = 0;
                return new FileStreamResult(ms, "application/pdf");
            }
            else
                return NotFound(path);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
