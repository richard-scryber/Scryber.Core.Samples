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

namespace Scryber.Logging
{
    /// <summary>
    /// A PDFTraceLog implementation that writes to the Trace context in the current System.Web.HttpContext
    /// </summary>
    public class PDFWebTraceLog : PDFTraceLog
    {

        /// <summary>
        /// Creates a new PDFWebTrace instance that records at the specified level.
        /// </summary>
        /// <param name="level"></param>
        public PDFWebTraceLog(TraceRecordLevel level, string name)
            : base(level, name)
        {
        }


        /// <summary>
        /// Records the log entry to the Trace context in the current System.Web.HttpContext
        /// </summary>
        /// <param name="inset"></param>
        /// <param name="level"></param>
        /// <param name="timestamp"></param>
        /// <param name="category"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        protected internal override void Record(string inset, TraceLevel level, TimeSpan timestamp, string category, string message, Exception ex)
        {
            System.Web.HttpContext context = AssertGetWebContext();
            System.Web.TraceContext trace = context.Trace;

            if (trace.IsEnabled)
            {
                string fullmessage = GetFullMessage(inset, level, timestamp, message, ex);

                if (level >= TraceLevel.Warning)
                    trace.Warn(category,  fullmessage, ex);
                else
                    trace.Write(category, fullmessage, ex);
            }
        }


        private string GetFullMessage(string inset, TraceLevel level, TimeSpan timestamp, string message, Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(inset);

            if (string.IsNullOrEmpty(message))
            {
                if (null == ex || string.IsNullOrEmpty(ex.Message))
                    sb.Append("EMPTY MESSAGE TO LOG");
                else
                    sb.Append(ex.Message);
            }
            else
                sb.Append(message);

            return sb.ToString();
        }


        private System.Web.HttpContext AssertGetWebContext()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            if (null == context)
                throw new System.InvalidOperationException(Errors.CannotRecordWebTraceWithoutWebContext);
            return context;
        }
    }


    /// <summary>
    /// Factory for the Web trace log
    /// </summary>
    public class PDFWebTraceLogFactory : IPDFTraceLogFactory
    {

        public PDFWebTraceLogFactory()
        {
        }

        public PDFTraceLog CreateLog(TraceRecordLevel level, string name)
        {
            return new PDFWebTraceLog(level, name);
        }
    }
}
