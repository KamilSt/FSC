using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSC.ViewModels.Api
{
    public class CustomersVM
    {
        public int CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string NIP { get; set; }
        public string AccountNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
    }
}