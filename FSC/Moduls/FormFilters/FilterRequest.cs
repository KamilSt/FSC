using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSC.Moduls.FormFilters
{
    public class FilterRequest
    {
        public string key { get; set; }
        public string value { get; set; }
        public string controlType { get; set; }
    }
}