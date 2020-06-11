using System;
namespace Scryber.Core.Samples.Web
{
    public interface IPDFDocumentService
    {
        Models.DocumentReference[] GetDocumentReferences(string path);

        Models.DocumentReference GetAReference(string path, string name);

        Scryber.Components.PDFDocument GetDocument(Models.DocumentReference reference);
    }
}
