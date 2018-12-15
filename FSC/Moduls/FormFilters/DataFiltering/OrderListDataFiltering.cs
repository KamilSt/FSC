using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using FSC.DataLayer;

namespace FSC.Moduls.FormFilters.DataFiltering
{
    public class OrderListDataFiltering
    {
        public static IQueryable<Order> Filter<T>(IQueryable<Order> orders, string filters)
        {
            var ser = new JavaScriptSerializer();
            var reqFilters = ser.Deserialize<List<FilterRequest>>(filters);
            foreach (var filter in reqFilters)
            {
                switch (filter.key)
                {
                    case "NumberInvoice":
                        if (!string.IsNullOrWhiteSpace(filter.value))
                            orders = orders.Where(x => x.InvoiceDocuments.Any(y => y.InvoiceNmuber.Contains(filter.value)));
                        break;

                    case "CompanyName":
                        if (!string.IsNullOrWhiteSpace(filter.value))
                            orders = orders.Where(x => x.Customer.CompanyName.Contains(filter.value));
                        break;

                    case "DocumentType":
                        if (!string.IsNullOrEmpty(filter.value))
                        {
                            if (filter.value.Equals("Invoice"))
                                orders = orders.Where(x => x.Invoiced == true);
                            else if (filter.value.Equals("Correction"))
                                orders = orders.Where(x => x.Description.Contains("Corection Invoice"));
                        }
                        break;

                    default:
                        break;
                }
            }
            return orders;
        }
    }
}