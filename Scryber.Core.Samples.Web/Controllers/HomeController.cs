using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Scryber.Core.Samples.Web.Models;
using Scryber.Components;
using Scryber.Drawing;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;
using Scryber.Components.Mvc;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Scryber.Core.Samples.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private string _rootPath;
        
        public HomeController(IWebHostEnvironment environment, IConfiguration config)
        {
            _env = environment;
            _rootPath = environment.ContentRootPath;
        }

        public IActionResult Index()
        {
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> ViewHtml(bool pdf = false)
        {
            var data = new Models.DataContentList();
            for (var i = 0; i < 100; i++)
            {
                data.Add(new DataContent() { ID = i.ToString(), Name = "Item " + i.ToString(), Price = i * 100 });
            }
            
            if (pdf)
            {
                Document doc = await this.ParseDocumentView("HtmlContent", data);
                return this.PDF(doc);
            }
            else
                return View("HtmlContent", data);
        }


        

        [HttpGet]
        public IActionResult HelloWorld(string title = "Hello World From Scryber")
        {
            var path = _env.ContentRootPath;
            path = System.IO.Path.Combine(path, "Views", "PDF", "HelloWorld.pdfx");

            using (var doc = Document.ParseDocument(path))
            {
                doc.Params["Title"] = title;
                var page = doc.Pages[0] as Page;
                page.Contents.Add(new Label() { Text = "My Content" });
                return this.PDF(doc); // inline:false, outputFileName:"HelloWorld.pdf"
            }
        }

        [HttpGet]
        public IActionResult Create(string doc)
        {
            var path = _env.ContentRootPath;
            
            path = System.IO.Path.Combine(path, "Views", "PDF", doc + ".pdfx");
            path = System.IO.Path.GetFullPath(path);

            var pdf = Document.ParseDocument(path);
            pdf.Params["theme-bg"] = new Scryber.Drawing.PDFColor(0.0, 0.0, 0.0);
            pdf.Params["theme-bg2"] = new Scryber.Drawing.PDFColor(0.3, 0.3, 0.3);
            pdf.Params["theme-title-fill"] = new Scryber.Drawing.PDFColor(1, 1, 1);
            pdf.Params["theme-title-font"] = "Gill Sans";

            System.Drawing.Bitmap bmp = LoadImageBitmap();
            Scryber.Drawing.PDFImageData data = Scryber.Drawing.PDFImageData.LoadImageFromBitmap("Dynamic", bmp, false);
            pdf.Params["toroidBin"] = data;

            return this.PDF(pdf); //, inline:false, outputFileName:"HelloWorld.pdf");
        }

        [HttpGet]
        public IActionResult CreateHtml(string doc)
        {
            var path = _env.ContentRootPath;

            path = System.IO.Path.Combine(path, "Views", "HTML", doc + ".html");
            path = System.IO.Path.GetFullPath(path);

            var model = new { one = "First String" };
       
            var pdf = Document.ParseDocument(path);
            
            System.Drawing.Bitmap bmp = LoadImageBitmap();
            Scryber.Drawing.PDFImageData data = Scryber.Drawing.PDFImageData.LoadImageFromBitmap("Dynamic", bmp, false);
            pdf.Params["toroidBin"] = data;

            return this.PDF(pdf); //, inline:false, outputFileName:"HelloWorld.pdf");
        }

        public IActionResult ImageDocument()
        {
            var root = _env.ContentRootPath;
            var path = System.IO.Path.Combine(root, "Views", "PDF", "DrawingImages.pdfx");
            path = System.IO.Path.GetFullPath(path);

            using(var doc = Document.ParseDocument(path))
            {
                var images = System.IO.Path.Combine(root, "Content", "Images");
                
                //Set the source Directly
                (doc.FindAComponentById("tiff32") as Image).Source = System.IO.Path.Combine(images, "Toroid32.tiff");

                //Set the source parameter
                doc.Params["toroidPath"] = System.IO.Path.Combine(images, "Toroid24.jpg");

                //Set the Image Data from another bitmap or file
                var bmp = LoadImageBitmap();
                var data = PDFImageData.LoadImageFromBitmap("DynamicJpeg", bmp, false);
                doc.Params["toroidBin"] = data;

                return this.PDF(doc);
            }
        }

        private System.Drawing.Bitmap LoadImageBitmap()
        {
            //Example method that just return an image from a file
            var path = _env.ContentRootPath;
            path = System.IO.Path.Combine(path, "Content", "Images", "Toroid24.jpg");

            return System.Drawing.Bitmap.FromFile(path) as System.Drawing.Bitmap;
        }


        [HttpGet]
        public IActionResult PlaceholderDocument()
        {
            var path = _rootPath;
            path = System.IO.Path.Combine(path, "Views", "PDF", "DocumentPlaceholders.pdfx");
            var doc = Document.ParseDocument(path);

            //In this example, just create a random table
            //Replace with anything needed.

            var place = doc.FindAComponentById("DynamicContent") as PlaceHolder;
            var table = new TableGrid();

            table.Style.Size.FullWidth = true;
            table.Style.Padding.All = 5;

            place.Contents.Add(table);

            for(var r = 0; r < 5; r++)
            {
                var row = new TableRow();
                table.Rows.Add(row);

                for (var c = 1; c < 4; c++)
                {
                    var cell = new TableCell();
                    var literal = new TextLiteral("Cell " + ((r * 3) + c));
                    cell.Contents.Add(literal);
                    row.Cells.Add(cell);
                }
            }

            return this.PDF(doc);
        }

        [HttpGet]
        public IActionResult DocumentParameters()
        {
            var path = _rootPath;
            path = System.IO.Path.Combine(path,"Views", "PDF", "DocumentParameters.pdfx");
            var doc = Document.ParseDocument(path);

            doc.Params["MyTitle"] = "New Document Title";
            doc.Params["TitleBg"] = new Scryber.Drawing.PDFColor(1, 0, 0);

            return this.PDF(doc);
        }

        [HttpGet]
        public IActionResult DocumentStyleParameters()
        {
            var path = _rootPath;
            path = System.IO.Path.Combine(path, "Views", "PDF", "DocumentStyleParameters.pdfx");
            var doc = Document.ParseDocument(path);
           

            var model = new
            {
                Title = "This is the document title",
                List = new[] {
                    new { Name = "First", Id = "FirstID" },
                    new { Name = "Second", Id = "SecondID" }
                },
                Theme = new
                {
                    TitleBg = new PDFColor(1, 0, 0),
                    TitleColor = new PDFColor(1, 1, 1),
                    TitleFont = "Segoe UI Light",
                    BodyFont = "Segoe UI",
                    BodySize = (PDFUnit)12
                }
            };

            return this.PDF(doc, model);
        }

        [HttpGet]
        public IActionResult DocumentXmlParameters()
        {
            var path = _rootPath;
            path = System.IO.Path.Combine(path, "Views", "PDF", "DocumentXmlParameters.pdfx");
            var doc = Document.ParseDocument(path);

            doc.Params["MyTitle"] = "Xml Document Title";
            var ele = new XElement("Root",
                new XElement("Entry", new XAttribute("id", "Fourth"), new XText("Fourth Name")),
                new XElement("Entry", new XAttribute("id", "Fifth"), new XText("Fifth Name")),
                new XElement("Entry", new XAttribute("id", "Sixth"), new XText("Sixth Name"))
                );
            doc.Params["MyData"] = ele;
            return this.PDF(doc);
        }



        [HttpGet]
        public IActionResult DocumentTemplateParameters()
        {
            var path = _rootPath;
            path = System.IO.Path.Combine(path, "Views", "PDF", "DocumentTemplateParameters.pdfx");
            var doc = Document.ParseDocument(path);

            doc.Params["MyTitle"] = "Xml Document Title";
            var ele = new XElement("Root",
                new XElement("Entry", new XAttribute("id", "Fourth"), new XText("Fourth Name")),
                new XElement("Entry", new XAttribute("id", "Fifth"), new XText("Fifth Name")),
                new XElement("Entry", new XAttribute("id", "Sixth"), new XText("Sixth Name"))
                );
            doc.Params["MyData"] = ele;

            doc.Params["MyContent"] = "<doc:Li><doc:H1 text='{xpath:text()}' /></doc:Li>";

            return this.PDF(doc);
        }

        public IActionResult DocumentDataSource()
        {
            var path = _rootPath;
            path = System.IO.Path.Combine(path, "Views", "PDF", "DocumentDataSources.pdfx");

            //This method always stores the passed model as the Model parameter
            return this.PDF(path);
        }

        public IActionResult Xml()
        {
            var xml = new XDocument(
                new XElement("DataSources",
                    new XAttribute("title", "Testing Xml Datasources"),
                    new XElement("Entries",
                        new XElement("Entry", new XAttribute("Name", "First Xml"), new XAttribute("Id", "FirstID")),
                        new XElement("Entry", new XAttribute("Name", "Second Xml"), new XAttribute("Id", "SecondID")),
                        new XElement("Entry", new XAttribute("Name", "Third Xml"), new XAttribute("Id", "ThirdID")),
                        new XElement("Entry", new XAttribute("Name", "Fourth Xml"), new XAttribute("Id", "FourthID"))
                        )
                    )
                );
            return Content(xml.ToString(), "text/xml");
        }

        public IActionResult Html(string name = "Other")
        {
            var data = new Models.DataContentList();
            for(var i = 0; i < 12; i++)
            {
                data.Add(new DataContent() { ID = i.ToString(), Name = "Item " + i.ToString(), Price = i * 100 });
            }
            this.ViewBag.Name = name;
            return PartialView("HtmlContent", data);
        }

        public IActionResult Json()
        {
            var json = new
            {
                DataSources = new
                {
                    Title = "Testing Document Datasources",
                    Entries = new[]
                    {
                        new { Name = "First", Id = "FirstID"},
                        new { Name = "Second", Id = "SecondID"}
                    }
                }
            };

            return Json(json);
        }


        public IActionResult DocumentDynamic(string title = "New Document")
        {
            using (var pdfx = GetDocument(title))
            {
                var model = GetData(title);

                return this.PDF(pdfx, model);
            }
        }

        protected Document GetDocument(string title)
        {
            string content = @"<?xml version='1.0' encoding='utf-8' ?>
                        <doc:Document xmlns:doc = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                                    xmlns:styles = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
                                    xmlns:data = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd' >
                        <Params>
                            <doc:Object-Param id='Model' ></doc:Object-Param>
                        </Params>
                        <Pages>
                            <doc:Section>
                                <Content>
                                    <data:ForEach id='Foreach2' value='{@:Model.Entries}' >
                                        <Template>
                                            <doc:Label text='{@:.Name}' />
                                        </Template>
                                    </data:ForEach>
                                </Content>
                            </doc:Section>
                        </Pages>
                    </doc:Document>";

            //With a string reader, but could be any stream or source.
            using (var reader = new System.IO.StringReader(content))
            {
                return Document.ParseDocument(reader, ParseSourceType.DynamicContent);
            }
        }

        protected object GetData(string title)
        {
            var data = new
            {
                Title = title,
                Entries = new[]
                    {
                        new { Name = "First", Id = "FirstID"},
                        new { Name = "Second", Id = "SecondID"}
                    }
            };
            return data;
        }

        public IActionResult DocumentController()
        {
            var path = _rootPath;
            path = System.IO.Path.Combine(path, "Views", "PDF", "DocumentController.pdfx");

            //This method always stores the passed model as the Model parameter
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
