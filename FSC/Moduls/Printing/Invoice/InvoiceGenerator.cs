using FSC.DataLayer;
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
            PDFFile = generatePDF();
            InvoiceDocument = createInvoiceDbDocument();
        }
        private byte[] generatePDF()
        {
            return new byte[1];
        }
        private InvoiceDocument createInvoiceDbDocument()
        {
            var doc = new InvoiceDocument();
            doc.DateOfInvoice = DateOfInvoice;
            doc.InvoiceNmuber = "Faktura " + InvoiceNumber;
            doc.FileName = "Faktura " + InvoiceNumber + ".pdf";
            doc.File = PDFFile;
            return doc;
        }
    }
}