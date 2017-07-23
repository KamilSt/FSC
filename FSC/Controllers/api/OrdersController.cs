using AutoMapper;
using FSC.DataLayer;
using FSC.Moduls.Printing;
using FSC.ViewModels.Api;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

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

        [HttpGet]
        //[Route("api/Orders/Get")]
        public OrderListVM Get()
        {
            OrderListVM vm = new OrderListVM();
            var listItem = applicationDB.Orders.Select(order => new OrderListItemVM
            {
                Id = order.OrderId,
                CompanyName = order.Customer.CompanyName,
                Date = order.OrderDateTime,
                Total = order.OrderItems.Sum(s => s.Quantity * s.Rate * (1 + (s.VAT / 100))),
                Invoiced = order.Invoiced,
                InvoiceNumber = order.InvoiceDocuments.FirstOrDefault().InvoiceNmuber
            });//Filters
            vm.Orders.AddRange(listItem);
            vm.Count = vm.Orders.Count();
            return vm;
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
                applicationDB.OrderItems.Add(new OrderItem()
                {
                    OrderId = newOrder.OrderId,
                    Rate = item.Rate,
                    Quantity = item.Quantity,
                    VAT = item.VAT,
                    ServiceItemCode = item.Servis,
                    ServiceItemName = item.Servis,
                });
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
            order.Description = value.Description;
            order.CustomerId = value.CustomerId;
            foreach (var item in value.OrderItems)
            {
                if (item.Status == 0) // New,
                {
                    var newItem = new OrderItem()
                    {
                        OrderId = value.Id,
                        Quantity = item.Quantity,
                        Rate = item.Rate,
                        ServiceItemName = item.Servis,
                        ServiceItemCode = item.Servis,
                        VAT = item.VAT,
                    };
                    applicationDB.Entry(newItem).State = EntityState.Added;
                }
                if (item.Status == 1) //Modyficate,
                {
                    var orderItem = applicationDB.OrderItems.FirstOrDefault(x => x.OrderItemId == item.Id);
                    if (orderItem != null)
                    {
                        orderItem.VAT = item.VAT;
                        orderItem.Quantity = item.Quantity;
                        orderItem.Rate = item.Rate;
                        orderItem.ServiceItemCode = item.Servis;
                        orderItem.ServiceItemName = item.Servis;
                        applicationDB.Entry(orderItem).State = EntityState.Modified;
                    }
                }
                if (item.Status == 2) //Delete
                {
                    var orderItem = applicationDB.OrderItems.FirstOrDefault(x => x.OrderItemId == item.Id);
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
            newOrderVM.Id = order.OrderId;
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
            newOrderVM.Id = order.OrderId;
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
            invoice.Generate();
            invoice.InvoiceDocument.OrderId = id;
            invoice.InvoiceDocument.CustomerId = order.CustomerId;
            applicationDB.InvoiceDocuments.Add(invoice.InvoiceDocument);
            applicationDB.SaveChanges();
            return Ok(new { Id = invoice.InvoiceDocument.Id, InvoiceNmuber = invoice.InvoiceDocument.InvoiceNmuber });
        }
    }
}
