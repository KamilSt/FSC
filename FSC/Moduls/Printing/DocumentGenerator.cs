using FSC.DataLayer;
using System;

namespace FSC.Moduls.Printing
{
    public abstract class DocumentGenerator
    {
        public virtual void Generate()
        {
        }
        public DateTime DateOfInvoice;
        public string InvoiceNumber { get; set; }
        public InvoiceDocument InvoiceDocument { get; set; }
        public byte[] PDFFile;
    }
}