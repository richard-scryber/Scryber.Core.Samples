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
            string docsDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string outputDirectory = System.IO.Path.Combine(docsDirectory, "Scryber Test Output");
            if (System.IO.Directory.Exists(outputDirectory) == false)
                System.IO.Directory.CreateDirectory(outputDirectory);

            //The path to the document to create
            string docPath = "DocumentRefs.pdfx";

            
            //The path to the output file - could be a stream
            var outputPath = System.IO.Path.Combine(outputDirectory, "DocumentRefs.pdf");

            var repeat = true;

            while (repeat)
            {
              
                System.Console.WriteLine("Beginning PDF Creation");
                //Load the template and output to the directory

                try
                {
                    var path = System.IO.Path.Combine(workingDirectory, "PDFs", docPath);
                    using (var doc = Document.ParseDocument(path))
                        doc.SaveAsPDF(outputPath, System.IO.FileMode.OpenOrCreate);

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

        private static Component StyledComponent()
        {
            var div = new Div()
            {
                BackgroundColor = new Scryber.Drawing.PDFColor(Drawing.ColorSpace.RGB, 255, 0, 0),
                Margins = new Drawing.PDFThickness(20),
                Padding = new Drawing.PDFThickness(4),
                FontFamily = (Drawing.PDFFontSelector)"Arial",
                FontSize = 20,
                FillColor = Scryber.Drawing.PDFColors.White
            };

            div.Contents.Add(new Label()
            {
                Text = "Hello World from scryber"
            });

            return div;

        }

    }
}
