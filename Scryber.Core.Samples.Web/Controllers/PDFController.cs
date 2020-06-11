using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Scryber.Core.Samples.Web.Controllers
{
    public class PDFController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}