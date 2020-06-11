using System;
namespace Scryber.Online.Web.Utility
{
    public static class PDFDocument_ResponseExtensions
    {


        /// <summary>
        /// Loads a PDFDocument from the specified full or releative path.
        /// </summary>
        /// <remarks>If the method is called with a non rooted path then the full path is built based upon either the
        /// current web request, or the current working directory</remarks>
        /// <param name="path"></param>
        /// <returns></returns>
        public static PDFDocument ParseDocument(string path)
        {
            if (!System.IO.Path.IsPathRooted(path))
            {
                if (System.Web.HttpContext.Current != null)
                {
                    path = System.Web.HttpContext.Current.Server.MapPath(path);
                }
                else
                {
                    string root = System.Environment.CurrentDirectory;
                    path = System.IO.Path.Combine(root, path);
                    path = System.IO.Path.GetFullPath(path);
                }
            }

            IPDFComponent parsed = Parse(path);
            if (!(parsed is PDFDocument))
                throw new InvalidCastException(String.Format(Errors.CannotConvertObjectToType, parsed.GetType(), typeof(PDFDocument)));

            PDFDocument doc = parsed as PDFDocument;

            return doc;

        }

        #region public void ProcessDocument(System.Web.HttpResponse response, bool endresponse) + 3 overloads

        private const string ResponseContentType = "application/pdf";
        private const string PDFExtension = ".pdf";
        private const bool AsAttatchment = true;
        private static readonly Encoding ResponseEncoding = Encoding.ASCII;

        /// <summary>
        /// Performs complete initialization and load and render of this document to the http response.
        /// Rendering as an attachment with the name of the file this document was parsed from, 
        /// or defaulting to Document.pdf if there was no original source file.
        /// Once complete the response will be ended and all content returned to the client.
        /// </summary>
        /// <param name="response">The HttpResponse to render to</param>
        /// <remarks>This will be output as an attachment and all existing 
        /// content on the response stream will be cleared. Once rendering has been completed the 
        /// response will be ended and no further processing can take place.</remarks>
        public static void ProcessDocument(this Scryber.Components.PDFDocument doc, System.Web.HttpResponse response)
        {
            bool endresponse = true;
            doc.ProcessDocument(response, endresponse);
        }

        /// <summary>
        /// Performs complete initialization and load and render of this document to the http response.
        /// Rendering as an attachment with the name of the file this document was parsed from, or defaulting to Document.pdf if there was no original source file
        /// </summary>
        /// <param name="response">The HttpResponse to render to</param>
        /// <param name="endresponse">If true then the current web response thread will be ended and no more action can be taken</param>
        /// <remarks>This will be output as an attachment and all existing content on the response stream will be cleared</remarks>
        public void ProcessDocument(System.Web.HttpResponse response, bool endresponse)
        {
            string name = this.LoadedSource;

            if (string.IsNullOrEmpty(name))
                name = "Document";
            else
                name = System.IO.Path.GetFileNameWithoutExtension(name);


            name = name + PDFExtension;

            this.ProcessDocument(response, name, endresponse);
        }

        /// <summary>
        /// Performs a complete initialization and load and render of this document to the http response. 
        /// Rendering as an attachment with the specified name
        /// </summary>
        /// <param name="response">The HttpResponse to render to</param>
        /// <param name="documentname">The name of the document as it will appear as on the client</param>
        public void ProcessDocument(System.Web.HttpResponse response, string documentname, bool endresponse)
        {
            bool asAttachment = AsAttatchment;
            bool clearContent = true;
            string contentType = ResponseContentType;
            bool bind = this.AutoBind;

            this.ProcessDocument(response, documentname, asAttachment, clearContent, contentType, System.Text.Encoding.UTF8, bind, endresponse);
        }

        /// <summary>
        /// Performs a complete initialization and load and render of this document to the http response. 
        /// Rendering as an attachment with the specified name.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="documentname">The name of the document as it will appear as on the client</param>
        /// <param name="bind">Overrides the value of this documents AutoBind value to the explicit value set here</param>
        public void ProcessDocument(System.Web.HttpResponse response, string documentname, bool bind, bool endresponse)
        {
            bool asAttachment = AsAttatchment;
            bool clearContent = true;
            string contentType = ResponseContentType;
            Encoding encoding = ResponseEncoding;
            this.ProcessDocument(response, documentname, asAttachment, clearContent, contentType, encoding, bind, endresponse);
        }

        /// <summary>
        /// Main method for outputting this document to a http response.
        /// Performs a complete initialization, load and render of this document to the http response. 
        /// </summary>
        /// <param name="response">The HttpRespone to write to.</param>
        /// <param name="documentname">The name of the document it will appear as on the client</param>
        /// <param name="outputAsAttachement">If true this will be output as an attachment on the response stream</param>
        /// <param name="clearAnyContent">If true all existing content will be cleared</param>
        /// <param name="contentType">If not empty then the attachmemt will have the specified content type</param>
        /// <param name="encoding">If specified then the content encoding will be set to this value</param>
        /// <param name="bind">Overrides the value of this documents AutoBind value to the explicit value set here</param>
        /// <param name="endresponse">If true, once all the document has been written then the http response will be ended</param>
        public virtual void ProcessDocument(System.Web.HttpResponse response, string documentname, bool outputAsAttachement, bool clearAnyContent, string contentType, System.Text.Encoding encoding, bool bind, bool endresponse)
        {

            if (null == response)
                throw new ArgumentNullException("response");
            if (string.IsNullOrEmpty(documentname))
                throw new ArgumentNullException("documentname");

            if (!string.IsNullOrEmpty(documentname))
                this.FileName = documentname;

            if (clearAnyContent)
            {
                response.Clear();
                response.ClearHeaders();
                response.ClearContent();
            }

            this.ProcessDocument(response.OutputStream, bind);

            if (!string.IsNullOrEmpty(contentType))
                response.ContentType = contentType;

            if (null != encoding)
                response.ContentEncoding = encoding;

            if (outputAsAttachement)
                response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", this.FileName));

            if (endresponse)
                response.End();
        }

        #endregion

    }
}
