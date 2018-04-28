using AutoMapper;
using FSC.DataLayer;
using FSC.Moduls.FormFilters;
using FSC.Moduls.Printing;
using FSC.ViewModels.Api;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace FSC.Controllers.api
{
    [Authorize]
    public class OrdersController : ApiController
    {
        private ApplicationDbContext applicationDB = null;
        private readonly string userId = null;
        public OrdersController()
        {
            applicationDB = new ApplicationDbContext();
            userId = User.Identity.GetUserId();
        }

        [HttpPost]
        [Route("api/Orders/Get")]
        public OrderListVM Get([FromBody]string filters)
        {
            OrderListVM vm = new OrderListVM();
            IQueryable<Order> orders = applicationDB.Orders;
            if (!string.IsNullOrEmpty(filters))
                orders = filterMethod(orders, filters);

            var orderList = orders.Select(order => new OrderListItemVM
            {
                Id = order.OrderId,
                CompanyName = order.Customer.CompanyName,
                Date = order.OrderDateTime,
                Total = order.Total,
                Invoiced = order.Invoiced,
                InvoiceNumber = order.InvoiceDocuments.FirstOrDefault().InvoiceNmuber,
                InvoiceId = order.InvoiceDocuments.FirstOrDefault() !=null ? order.InvoiceDocuments.FirstOrDefault().Id : 0
            });
            vm.Orders.AddRange(orderList);
            vm.Count = vm.Orders.Count();
            return vm;
        }
        private IQueryable<Order> filterMethod(IQueryable<Order> orders, string filters)
        {
            var ser = new JavaScriptSerializer();
            var reqFilters = ser.Deserialize<List<FilterRequest>>(filters);
            foreach (var filter in reqFilters)
            {
                switch (filter.key)
                {
                    case "NumberInvoice":
                        if (!string.IsNullOrWhiteSpace(filter.value))
                            orders = orders.Where(x => x.InvoiceDocuments.Any(y => y.InvoiceNmuber.Contains(filter.value)));
                        break;

                    case "CompanyName":
                        if (!string.IsNullOrWhiteSpace(filter.value))
                            orders = orders.Where(x => x.Customer.CompanyName.Contains(filter.value));
                        break;

                    case "Invoiced":
                        if (!string.IsNullOrEmpty(filter.value))
                        {
                            if (filter.value.Equals("Invoiced"))
                                orders = orders.Where(x => x.Invoiced == true);
                            else if (filter.value.Equals("TakenCorrection"))
                                orders = orders.Where(x => x.Description.Contains("Corection Invoice"));
                        }
                        break;

                    default:
                        break;
                }
            }
            return orders;
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody]NewOrderVM value)
        {
            var newOrder = new Order()
            {
                CustomerId = value.CustomerId,
                Description = value.Description,
                OrderDateTime = DateTime.UtcNow,
                OrderItems = new List<OrderItem>(),
                UserId = userId,
            };
            applicationDB.Orders.Add(newOrder);
            applicationDB.SaveChanges();
            foreach (var item in value.OrderItems)
            {
                var order = new OrderItem() { OrderId = newOrder.OrderId };
                applicationDB.OrderItems.Add(Mapper.Map<NewOrderItem, OrderItem>(item, order));
            }
            applicationDB.SaveChanges();
            value.Id = newOrder.OrderId;
            return Created($"/Orders/{value.Id.ToString()}", value);
        }

        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]NewOrderVM value)
        {
            if (id == 0)
                return BadRequest();
            var order = applicationDB.Orders.FirstOrDefault(x => x.OrderId == id);
            if (order == null)
                return NotFound();
            Mapper.Map<NewOrderVM, Order>(value, order);
            foreach (var item in value.OrderItems)
            {
                if (item.Status == 0) // New,
                {
                    var newItem = Mapper.Map<OrderItem>(item);
                    newItem.OrderId = value.Id;
                    applicationDB.Entry(newItem).State = EntityState.Added;
                }
                if (item.Status == 1) //Modyficate,
                {
                    var orderItem = applicationDB.OrderItems.FirstOrDefault(x => x.OrderItemId == item.OrderItemId);
                    if (orderItem != null)
                    {
                        Mapper.Map<NewOrderItem, OrderItem>(item, orderItem);
                        applicationDB.Entry(orderItem).State = EntityState.Modified;
                    }
                }
                if (item.Status == 2) //Delete
                {
                    var orderItem = applicationDB.OrderItems.FirstOrDefault(x => x.OrderItemId == item.OrderItemId);
                    if (orderItem != null)
                    {
                        applicationDB.Entry(orderItem).State = EntityState.Deleted;
                    }
                }
                //if (item.Status == 3)  //Orginal
            }

            applicationDB.Entry(order).State = EntityState.Modified;
            applicationDB.SaveChanges();


            var newOrder = applicationDB.Orders.FirstOrDefault(x => x.OrderId == value.Id);

            var newOrderVM = Mapper.Map<NewOrderVM>(newOrder);
            return Ok(newOrderVM);
        }

        [Route("api/Orders/Get/{id:int:max(10000)}")]
        public IHttpActionResult Get(int id)
        {
            if (id == 0)
                return BadRequest();
            var order = applicationDB.Orders.FirstOrDefault(x => x.OrderId == id);
            if (order == null)
                return NotFound();

            var newOrderVM = Mapper.Map<NewOrderVM>(order);
            return Ok(newOrderVM);
        }

        [HttpGet]
        [Route("api/Orders/CreateInvoice/{id:int:max(10000)}")]
        public IHttpActionResult CreateInvoice(int id)
        {
            if (id == 0)
                return NotFound();
            var order = applicationDB.Orders.FirstOrDefault(x => x.OrderId == id);
            if (order == null)
                return NotFound();

            order.Invoiced = true;
            var invoice = DocumentGeneratorFactory.GetGenerator(DocumentTypeEnum.Invoice);
            invoice.Generate(id);
            invoice.InvoiceDocument.OrderId = id;
            invoice.InvoiceDocument.CustomerId = order.CustomerId;
            applicationDB.InvoiceDocuments.Add(invoice.InvoiceDocument);
            applicationDB.SaveChanges();
            return Ok(new { Id = invoice.InvoiceDocument.Id, InvoiceNmuber = invoice.InvoiceDocument.InvoiceNmuber });
        }
    }
}
