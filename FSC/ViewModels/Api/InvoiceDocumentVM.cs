using System;
using System.Collections.Generic;

namespace FSC.ViewModels.Api
{
    public class InvoiceDocumentVM
    {
        public decimal Sum { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }
        public IEnumerable<InfoInfoice> InvoiceInfo { get; set; }

        public InvoiceDocumentVM()
        {
            InvoiceInfo = new List<InfoInfoice>();
        }
    }

    public class InfoInfoice
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string InvoiNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal Sum { get; set; }
    }
}