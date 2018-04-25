using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSC.Moduls.FormFilters
{
    public class FilterResponse
    {
        public FilterResponse()
        {
            Filters = new List<IFilter>();
        }
        public List<IFilter> Filters { get; set; }
        public int Version { get; set; }
    }
}