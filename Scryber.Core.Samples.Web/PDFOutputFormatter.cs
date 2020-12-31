using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scryber.Core.Samples.Web
{
    public class PDFOutputFormatter : Microsoft.AspNetCore.Mvc.Formatters.OutputFormatter
    {

        public PDFOutputFormatter()
        {
            this.SupportedMediaTypes.Add(Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/pdf"));
           
        }

        protected override bool CanWriteType(Type type)
        {
            return base.CanWriteType(type);
        }

        public override void WriteResponseHeaders(OutputFormatterWriteContext context)
        {
            base.WriteResponseHeaders(context);
        }

        public override Task WriteAsync(OutputFormatterWriteContext context)
        {
            return base.WriteAsync(context);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var obj = context.Object;
            return InputFormatterResult.SuccessAsync(obj);
        }
        
        private void GeneratePDF()
        {

        }

    }
}
