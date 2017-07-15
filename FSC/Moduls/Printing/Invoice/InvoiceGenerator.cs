using System;

namespace FSC.Moduls.Printing.Invoice
{
    public class InvoiceGenerator : DocumentGenerator
    {
        private GeneratorInvoices generatorInvoices;
        public InvoiceGenerator()
        {
            DateOfInvoice = DateTime.Now;
            generatorInvoices = new GeneratorInvoices(DateOfInvoice, DocumentTypeEnum.Invoice.ToString());
        }
        public override void Generate()
        {
            InvoiceNumber = generatorInvoices.GenerateInvoiceNumber();
            PDFFile = GeneratePDF();
        }
        private byte[] GeneratePDF()
        {
            return new byte[1];
        }
    }
}