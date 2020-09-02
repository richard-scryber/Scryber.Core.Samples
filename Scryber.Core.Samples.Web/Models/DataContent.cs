using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scryber.Core.Samples.Web.Models
{
    public class DataContent
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

    }

    public class DataContentList : List<DataContent>
    {

    }
}
