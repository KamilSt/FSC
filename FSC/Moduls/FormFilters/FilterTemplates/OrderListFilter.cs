using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSC.Moduls.FormFilters.FilterTemplates
{
    public class OrderListFilter
    {
        public static FilterResponse GetFilter()
        {
            var filterResponse = new FilterResponse();
            filterResponse.Version = 1;

            filterResponse.Filters.Add(new TextBoxFilter()
            {
                key = "NumberInvoice",
                label = "Numer faktury",
                order = 1,
                visible = true
            });

            filterResponse.Filters.Add(new TextBoxFilter()
            {
                key = "CompanyName",
                label = "Firma",
                order = 2,
                visible = true
            });

            var dropdownMenu = new List<FiltersBase<string>>();
            dropdownMenu.Add(new TextBoxFilter() { key = "Invoiced", label = "Zafakturowane", order = 1 });
            dropdownMenu.Add(new TextBoxFilter() { key = "TakenCorrection", label = "Z korektą", order = 2 });
            filterResponse.Filters.Add(new DropdownFilter()
            {
                key = "Invoiced",
                label = "Zafakturowa: ",
                order = 25,
                visible = true,
                value = dropdownMenu
            });

            return filterResponse;
        }
    }
}