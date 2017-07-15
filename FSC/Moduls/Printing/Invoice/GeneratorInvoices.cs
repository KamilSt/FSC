using FSC.DataLayer;
using System;
using System.Data.Entity;
using System.Linq;

namespace FSC.Moduls.Printing.Invoice
{
    public class GeneratorInvoices
    {
        private ApplicationDbContext applicationDB;
        private DateTime dateInvoice;
        private string documentType;
        private InvoiceCounter invoiceCounter;
        private string template;

        public GeneratorInvoices(DateTime dateInvoice, string documentType)
        {
            applicationDB = new ApplicationDbContext();
            this.dateInvoice = dateInvoice;
            this.documentType = documentType;
        }
        public string GenerateInvoiceNumber()
        {
            InvoiceTemplate itemplate = getTemplate();
            if (itemplate == null)
                throw new Exception("Nie znaleziono wzoru");
            template = itemplate.Template;
            replaceDate();
            replaceNumber();
            return template;
        }

        private void replaceDate()
        {
            template = template.Replace("yyyy", dateInvoice.Year.ToString());
            template = template.Replace("mm", dateInvoice.Month.ToString());
        }
        private void replaceNumber()
        {
            getInvoiceCounter();
            if (invoiceCounter == null)
            {
                makeInvoiceCounter();
                applicationDB.Entry(invoiceCounter).State = EntityState.Added;
            }
            else
                applicationDB.Entry(invoiceCounter).State = EntityState.Modified;

            template = template.Replace("nr", invoiceCounter.Counter.ToString());
            ++invoiceCounter.Counter;
            applicationDB.SaveChanges();
        }
        private void makeInvoiceCounter()
        {
            invoiceCounter = new InvoiceCounter
            {
                Counter = 1,
                Year = dateInvoice.Year.ToString(),
                Month = dateInvoice.Month.ToString(),
                DocumentType = documentType
            };
        }
        private void getInvoiceCounter()
        {
            invoiceCounter = applicationDB.InvoiceCounters
                .Where(x => x.DocumentType == documentType)
                .Where(x => x.Year == dateInvoice.Year.ToString())
                .Where(x => x.Month == dateInvoice.Month.ToString())
                .FirstOrDefault();
        }
        private InvoiceTemplate getTemplate()
        {
            return applicationDB.InvoiceTemplates
                  .FirstOrDefault(x => x.DocumentType == DocumentTypeEnum.Invoice.ToString());
        }
    }
}