using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSC.Moduls.FormFilters
{
    public class TextBoxFilter : FiltersBase<string>, IFilter
    {
        public TextBoxFilter()
        {
            this.controlType = "textbox";
            this.value = string.Empty;
        }
    }
}