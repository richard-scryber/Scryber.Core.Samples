using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Scryber.Components;
using Scryber.Components.Mvc;

namespace Scryber.Core.Samples.Web.Controllers
{
    public class PDFController : Controller
    {
        private IWebHostEnvironment _environment;

        public PDFController(IWebHostEnvironment env)
        {
            _environment = env;
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

        public IActionResult HelloTemplates()
        {
            string path = _environment.ContentRootPath;
            path = System.IO.Path.Combine(path, "Samples", "HelloWorld.pdfx");

            return this.PDF(path, false);
        }
    }

    
}