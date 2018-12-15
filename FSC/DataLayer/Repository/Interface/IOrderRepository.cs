using FSC.ViewModels.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSC.DataLayer.Repository.Interface
{
    public interface IOrderRepository : IDataRepository<Order>
    {
        IEnumerable<Order> Get(string filters);
        void AddOrderItems(int orderId, List<ViewModels.Api.NewOrderItem> list);
        void AddInvoiceToOrder(InvoiceDocument invoiceDocument);
        void ChangeOrderItems(int id, List<NewOrderItem> orderItem);
    }
}