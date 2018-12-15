using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FSC.DataLayer.Repository.Interface;
using AutoMapper;
using FSC.ViewModels.Api;
using FSC.Moduls.FormFilters;

namespace FSC.DataLayer.Repository
{
    public class OrderRepository : DataRepositoryBase<Order>, IOrderRepository
    {
        protected override Order AddEntity(ApplicationDbContext entityContext, Order entity)
        {
            return entityContext.Orders.Add(entity);
        }
        protected override Order UpdateEntity(ApplicationDbContext entityContext, Order entity)
        {
            return (from e in entityContext.Orders
                    where e.OrderId == entity.OrderId
                    select e).FirstOrDefault();
        }
        protected override IQueryable<Order> GetEntities(ApplicationDbContext entityContext)
        {
            return entityContext.Orders;
        }
        protected override Order GetEntity(ApplicationDbContext entityContext, int id)
        {
            return entityContext.Orders.Include("OrderItems").SingleOrDefault(x => x.OrderId == id);
        }

        public IEnumerable<Order> Get(string filters)
        {
            using (ApplicationDbContext entityContext = new ApplicationDbContext())
            {
                var orders = entityContext.Orders.Include("OrderItems").Include("Customer").Include("InvoiceDocuments").AsQueryable();
                if (!string.IsNullOrEmpty(filters))
                    orders = DataFilteringFactory.GetFor<Order>(DataFilteringFactory.FilterFor_OrderListFilter, orders, filters);
                return orders.OrderByDescending(x => x.OrderDateTime).ToList();
            }
        }
        public void AddOrderItems(int orderId, List<NewOrderItem> orderItems)
        {
            using (ApplicationDbContext entityContext = new ApplicationDbContext())
            {
                foreach (var item in orderItems)
                {
                    var order = new OrderItem() { OrderId = orderId };
                    entityContext.OrderItems.Add(Mapper.Map<NewOrderItem, OrderItem>(item, order));
                }
                entityContext.SaveChanges();
            }
        }
        public void AddInvoiceToOrder(InvoiceDocument invoiceDocument)
        {
            using (ApplicationDbContext entityContext = new ApplicationDbContext())
            {
                entityContext.InvoiceDocuments.Add(invoiceDocument);
                entityContext.SaveChanges();
            }
        }
        public void ChangeOrderItems(int id, List<NewOrderItem> orderItems)
        {
            using (ApplicationDbContext entityContext = new ApplicationDbContext())
            {
                foreach (var item in orderItems)
                {
                    if (item.Status == 0) // New,
                    {
                        var newItem = Mapper.Map<OrderItem>(item);
                        newItem.OrderId = id;
                        entityContext.Entry(newItem).State = EntityState.Added;
                    }
                    if (item.Status == 1) //Modyficate,
                    {
                        var orderItem = entityContext.OrderItems.FirstOrDefault(x => x.OrderItemId == item.OrderItemId);
                        if (orderItem != null)
                        {
                            Mapper.Map<NewOrderItem, OrderItem>(item, orderItem);
                            entityContext.Entry(orderItem).State = EntityState.Modified;
                        }
                    }
                    if (item.Status == 2) //Delete
                    {
                        var orderItem = entityContext.OrderItems.FirstOrDefault(x => x.OrderItemId == item.OrderItemId);
                        if (orderItem != null)
                        {
                            entityContext.Entry(orderItem).State = EntityState.Deleted;
                        }
                    }
                    //if (item.Status == 3)  //Orginal
                }
                entityContext.SaveChanges();
            }
        }
    }
}