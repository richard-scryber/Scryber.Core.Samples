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
using Scryber.Generation;
namespace Scryber
{
    internal class BindingPostValueExpression
    {
        private string _paramname;
        private PDFValueConverter _converter;
        private System.Reflection.PropertyInfo _property;

        private BindingPostValueExpression() { }

        internal void InitComponent(object sender, PDFInitEventArgs args)
        {
            Microsoft.AspNetCore.Http.HttpContext context = Scryber.Online.Utility.HttpContextAccess.Current;
            string value = context.Request.Query[this._paramname];
            if (string.IsNullOrEmpty(value))
            {
                if (args.Context.ShouldLogMessage)
                {
                    args.Context.TraceLog.Add(TraceLevel.Message, "Bindng Post Expression", "The parameter value for '" + this._paramname + "' was empty");
                }
            }
            else if (args.Context.ShouldLogVerbose)
            {
                args.Context.TraceLog.Add(TraceLevel.Verbose, "Bindng Post Expression", "The parameter value for '" + this._paramname + "' was '" + value + "'");
            }

            object converted = _converter(value, this._property.PropertyType, System.Globalization.CultureInfo.CurrentCulture);
            this._property.SetValue(sender, converted, null);
        }

        internal static BindingPostValueExpression Create(string expr, PDFValueConverter convertor, System.Reflection.PropertyInfo property)
        {
            BindingPostValueExpression binding = new BindingPostValueExpression();
            binding._paramname = expr;
            binding._converter = convertor;
            binding._property = property;

            return binding;
        }
    }
}
