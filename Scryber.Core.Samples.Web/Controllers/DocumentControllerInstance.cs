using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber;

namespace Scryber.Core.Samples.Web.Controllers
{
    public class DocumentControllerInstance
    {
        /// <summary>
        /// The Heading will be set on a controller instance from the parser
        /// </summary>
        [PDFOutlet(Required = true)]
        public PDFHead1 Title
        {
            get;set;
        }

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DocumentControllerInstance()
        {
        }


        [PDFAction()]
        public void LoadDocument(object sender, PDFLoadEventArgs args)
        {
            //We know this property is set as we have flagged it
            //as Required. Otherwise, check it's set.
            this.Title.Text = "Test Controller Title";
        }

        string[] data = new[] { "First", "Second", "Third" };

        /// <summary>
        /// Happens just before the ForEach is DataBound, so that we can assign the data value, and that will be used.
        /// </summary>
        [PDFAction()]
        public void BindingForEach(object sender, PDFDataBindEventArgs args)
        {
            //Dynamically set the data on the ForEach component - so it will loop through
            var forEach = (Data.PDFForEach)sender;
            forEach.Value = data;
        }

        /// <summary>
        /// Happens 3 times for each of the list items created in the template from the data source.
        /// </summary>
        [PDFAction()]
        public void BoundListItem(object sender, PDFDataBindEventArgs args)
        {
            var listItem = (PDFListItem)sender;
            var index = args.Context.CurrentIndex;
            var text = data[index];
            //Create a new text literal and add it to the listitem
            PDFTextLiteral literal = new PDFTextLiteral(text);
            listItem.Contents.Add(literal);
        }
    }
}
