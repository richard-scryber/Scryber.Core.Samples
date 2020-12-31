using System;
using Scryber.Components;

namespace Scryber.Core.Samples.Console
{
    class Program
    {
        

        static void Main(string[] args)
        {
            

            //Get the working and temp directory
            string workingDirectory = System.Environment.CurrentDirectory;
            string tempDirectory = System.IO.Path.GetTempPath();

            //The path to the document to create
            string docPath = "PDFs\\DocumentRefs.pdfx";

            
            //The path to the output file - could be a stream
            var outputPath = System.IO.Path.Combine(tempDirectory, "DocumentRefs.pdf");

            var repeat = true;

            while (repeat)
            {
              
                System.Console.WriteLine("Beginning PDF Creation");
                //Load the template and output to the directory

                try
                {
                    var path = System.IO.Path.Combine(workingDirectory, "PDFs", "DocumentRefs.pdfx");
                    using (var doc = PDFDocument.ParseDocument(path))
                        doc.ProcessDocument(outputPath, System.IO.FileMode.OpenOrCreate);

                    //Notify completion
                    System.Console.WriteLine("PDF File generated at " + outputPath);
                }
                catch(Exception ex)
                {
                    System.Console.WriteLine("ERROR: " + ex.Message);
                }

                System.Console.Write("Repeat (y/n):");
                var key = System.Console.ReadKey();
                if (key.Key != ConsoleKey.Y)
                    repeat = false;

                System.Console.WriteLine();

            }

        }

        private static PDFComponent StyledComponent()
        {
            var div = new PDFDiv()
            {
                BackgroundColor = new Scryber.Drawing.PDFColor(Drawing.ColorSpace.RGB, 255, 0, 0),
                Margins = new Drawing.PDFThickness(20),
                Padding = new Drawing.PDFThickness(4),
                FontFamily = "Arial",
                FontSize = 20,
                FillColor = Scryber.Drawing.PDFColors.White
            };

            div.Contents.Add(new PDFLabel()
            {
                Text = "Hello World from scryber"
            });

            return div;

        }

    }
}
