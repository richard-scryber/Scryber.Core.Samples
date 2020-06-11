using System;
namespace Scryber.Core.Samples.Web.Models
{
    public class DocumentReference
    {

        public string FullPath { get; set; }

        public string Name { get; set; }

        public DocumentReference(string path, string name)
        {
            this.FullPath = path;
            this.Name = name;
        }
    }
}
