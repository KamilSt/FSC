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
    }
}