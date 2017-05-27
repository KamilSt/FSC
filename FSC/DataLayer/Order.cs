using FSC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FSC.DataLayer
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        [Display(Name = "Data zamówienia")]
        public DateTime OrderDateTime { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
        public bool Invoiced { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}