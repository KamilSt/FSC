using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSC.ViewModels.Api
{
    public class OrderListVM
    {
        public OrderListVM()
        {
            this.Orders = new List<OrderListItemVM>();
        }

        public List<OrderListItemVM> Orders { get; internal set; }
        public int Count { get; set; }
        public int Page { get; set; }
    }
    public class OrderListItemVM
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public bool Invoiced { get; set; }
        public string InvoiceNumber { get; set; }
    }
}