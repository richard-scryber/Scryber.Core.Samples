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
    internal class BindingPostValueExpressionFactory : IPDFBindingExpressionFactory
    {
        public string BindingKey { get { return "post"; } }

        public DocumentGenerationStage BindingStage
        {
            get { return DocumentGenerationStage.Initialized; }
        }

        public PDFInitializedEventHandler GetInitBindingExpression(string expressionvalue, Type classType, System.Reflection.PropertyInfo forProperty)
        {
            Scryber.Generation.PDFValueConverter conv;
            if(!ParserDefintionFactory.IsSimpleObjectType(forProperty.PropertyType, out conv) &&
                        !ParserDefintionFactory.IsCustomParsableObjectType(forProperty.PropertyType, out conv))
                throw new PDFParserException(string.Format(Errors.ParserAttributeMustBeSimpleOrCustomParsableType, forProperty.Name,forProperty.PropertyType));

            BindingPostValueExpression expr = BindingPostValueExpression.Create(expressionvalue, conv, forProperty);
            return new PDFInitializedEventHandler(expr.InitComponent);
        }

        public PDFLoadedEventHandler GetLoadBindingExpression(string expressionvalue, Type classType, System.Reflection.PropertyInfo forProperty)
        {
            throw new NotSupportedException("Query string Binding is not supported on any other document lifecycle stage than Init");
        }

        public PDFDataBindEventHandler GetDataBindingExpression(string expressionvalue, Type classType, System.Reflection.PropertyInfo forProperty)
        {
            throw new NotSupportedException("Query string Binding is not supported on any other document lifecycle stage than Init");
        }
    }
}
