using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSC.ViewModels.Api
{
    public class NewOrderVM
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Description { get; set; }

        public List<NewOrderItem> OrderItems { get; set; }
    }
    public class NewOrderItem
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public int OrderItemId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal VAT { get; set; }
        public virtual decimal Brutto { get; set; }
        public string Servis { get; set; }
        public string ServiceItemName { get; set; }
    }
}