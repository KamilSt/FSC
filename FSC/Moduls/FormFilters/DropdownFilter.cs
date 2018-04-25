using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSC.Moduls.FormFilters
{
    public class DropdownFilter : FiltersBase<List<FiltersBase<string>>>, IFilter
    {
        public DropdownFilter()
        {
            this.controlType = "dropdown";
            this.value = new List<FiltersBase<string>>();
        }
    }
}