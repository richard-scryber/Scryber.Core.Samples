using System;

namespace Scryber.Online.Web.Services
{
    public class PDFPathMappingService : Scryber.IPDFPathMappingService
    {
        public PDFPathMappingService()
        {
        }

        public string MapPath(ParserLoadType loadtype, string reference, string parent, out bool isFile)
        {
            isFile = true;
            if (string.IsNullOrEmpty(parent))
                return reference;
            else
                return System.IO.Path.Combine(parent, reference);
        }
    }
}
