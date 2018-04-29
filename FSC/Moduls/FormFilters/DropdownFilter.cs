using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSC.Moduls.FormFilters
{
    public class DropdownFilter : FiltersBase<string>, IFilter
    {
        public List<FiltersBase<string>> options { get; set; }
        public DropdownFilter()
        {
            this.controlType = "dropdown";
            this.value = string.Empty; 
            this.options = new List<FiltersBase<string>>();
        }
    }
}