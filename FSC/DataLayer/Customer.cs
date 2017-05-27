using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FSC.DataLayer
{
    public class Customer
    {
        public int CustomerId { get; set; }
        [Required]
        public string CompanyName { get; set; }
        public string NIP { get; set; }
        public string AccountNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public List<Order> Orders { get; set; }
    }
}