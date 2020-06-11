using System;
using System.Collections.Generic;
using Scryber.Core.Samples.Web.Models;

namespace Scryber.Core.Samples.Web.Services
{
    public class PDFDocumentService : IPDFDocumentService
    {
        private const string DocumentExtension = ".pdfx";

        public PDFDocumentService()
        {
        }

        public DocumentReference GetAReference(string path, string name)
        {
            var one = System.IO.Directory.GetFiles(path, name + DocumentExtension);
            if (null == one || one.Length < 1)
                return null;
            else
                return new DocumentReference(one[0], name);
        }

        public DocumentReference[] GetDocumentReferences(string path)
        {
            List<DocumentReference> all = new List<DocumentReference>();

            var files = System.IO.Directory.GetFiles(path, "*" + DocumentExtension);
            foreach (var f in files)
            {
                var name = System.IO.Path.GetFileNameWithoutExtension(f);

                DocumentReference dref = new DocumentReference(f, name);
                all.Add(dref);
            }

            return all.ToArray();
        }

        public Scryber.Components.PDFDocument GetDocument(DocumentReference reference)
        {
            if (reference.FullPath.EndsWith(DocumentExtension) && System.IO.File.Exists(reference.FullPath))
            {
                using (var ms = new System.IO.FileStream(reference.FullPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    return Scryber.Components.PDFDocument.ParseDocument(ms, ParseSourceType.LocalFile);
                }
            }
            else
                throw new System.IO.FileNotFoundException("No document file was found with the name " + reference.Name);
        }
    }
}
