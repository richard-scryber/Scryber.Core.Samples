/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 * 
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;

namespace Scryber.Web
{
    /// <summary>
    /// Handles the requests for Scryber pdf documents
    /// </summary>
    public class ScryberPDFHandlerFactory : System.Web.IHttpHandlerFactory
    {
        /// <summary>
        /// Inner class that implemements the IHttpHander
        /// </summary>
        private class ScryberPDFHandler : System.Web.IHttpHandler
        {
            public bool IsReusable
            {
                get { return false; }
            }

            public void ProcessRequest(System.Web.HttpContext context)
            {
                string path = context.Request.PhysicalPath;
                if (!System.IO.File.Exists(path))
                {
                    context.Response.StatusCode = 404;
                    context.Response.End();
                }
                using (PDFDocument doc = PDFDocument.ParseDocument(path))
                {
                    doc.ProcessDocument(context.Response);
                }
            }
        }

        public System.Web.IHttpHandler GetHandler(System.Web.HttpContext context, string requestType, string url, string pathTranslated)
        {
            return new ScryberPDFHandler();
        }

        public void ReleaseHandler(System.Web.IHttpHandler handler)
        {
            //Do nothing
        }
    }
}
