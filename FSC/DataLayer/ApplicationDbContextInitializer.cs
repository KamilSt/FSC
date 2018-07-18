using System;
using System.Collections.Generic;

namespace FSC.DataLayer
{
    public class ApplicationDbContextInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var invoiceTempl = new List<InvoiceTemplate>
            {
             new InvoiceTemplate { Id = 1, Template = "yyyy/mm/nr" , DocumentType="Invoice"}
            };
            invoiceTempl.ForEach(templ => context.InvoiceTemplates.Add(templ));
            context.SaveChanges();
        }
    }
}