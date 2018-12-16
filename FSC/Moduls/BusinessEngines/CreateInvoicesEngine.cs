using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FSC.DataLayer.Repository.Interface;
using FSC.Moduls.Printing;
using FSC.ViewModels.Api;
using FSC.Moduls.BusinessEngines.Interface;

namespace FSC.Moduls.BusinessEngines
{
    public class CreateInvoicesEngine : ICreateInvoicesEngine
    {
        private IOrderRepository orderRepository = null;
        public CreateInvoicesEngine(IOrderRepository orderRepo)
        {
            orderRepository = orderRepo;
        }

        public CreatedInvoiceVM CreateIinvoice(int id)
        {
            var order = orderRepository.Get(id);
            var invoice = DocumentGeneratorFactory.GetGenerator(DocumentTypeEnum.Invoice);
            invoice.Generate(id);
            invoice.InvoiceDocument.OrderId = id;
            invoice.InvoiceDocument.CustomerId = order.CustomerId;
            order.Invoiced = true;
            orderRepository.Update(order);
            orderRepository.AddInvoiceToOrder(invoice.InvoiceDocument);
            return new CreatedInvoiceVM() { Id = invoice.InvoiceDocument.Id, InvoiceNmuber = invoice.InvoiceDocument.InvoiceNmuber };
        }
    }
}