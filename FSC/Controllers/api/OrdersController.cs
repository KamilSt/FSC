using AutoMapper;
using FSC.DataLayer;
using FSC.Moduls.FormFilters;
using FSC.Moduls.Printing;
using FSC.ViewModels.Api;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Script.Serialization;
using FSC.Providers.UserProvider.Interface;
using FSC.DataLayer.Repository.Interface;
using FSC.Business;
using FSC.Moduls.BusinessEngines.Interface;

namespace FSC.Controllers.api
{
    [Authorize]
    public class OrdersController : ApiController
    {
        private ApplicationDbContext applicationDB = null;
        private IOrderRepository orderRepository = null;
        private IBusinessEngineFactory _BusinessEngineFactory = null;
        private readonly string userId = null;
        public OrdersController(IOrderRepository orderRepo, IUserProvider user, IBusinessEngineFactory businessEngineFactory)
        {
            _BusinessEngineFactory = businessEngineFactory;
            applicationDB = new ApplicationDbContext();
            orderRepository = orderRepo;
            userId = user.GuidId;
        }

        [HttpPost]
        [Route("api/Orders/Get")]
        public OrderListVM Get([FromBody]string filters)
        {
            OrderListVM vm = new OrderListVM();
            IEnumerable<Order> orders = orderRepository.Get(filters);
            var ordersList = Mapper.Map<List<OrderListItemVM>>(orders);
            vm.Orders.AddRange(ordersList);
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
            orderRepository.Add(newOrder);
            orderRepository.AddOrderItems(newOrder.OrderId, value.OrderItems);
            value.Id = newOrder.OrderId;
            return Created("/Orders/{value.Id.ToString()}", value);
        }

        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]NewOrderVM value)
        {
            if (id == 0)
                return BadRequest();
            var order = orderRepository.Get(id);
            if (order == null)
                return NotFound();
            Mapper.Map<NewOrderVM, Order>(value, order);
            orderRepository.ChangeOrderItems(value.Id, value.OrderItems);
            orderRepository.Update(order);
            var newOrder = orderRepository.Get(value.Id);
            var newOrderVM = Mapper.Map<NewOrderVM>(newOrder);
            return Ok(newOrderVM);
        }

        [Route("api/Orders/Get/{id:int:max(10000)}")]
        public IHttpActionResult Get(int id)
        {
            if (id == 0)
                return BadRequest();
            var order = orderRepository.Get(id);
            if (order == null)
                return NotFound();
            var orderVM = Mapper.Map<NewOrderVM>(order);
            return Ok(orderVM);
        }

        [HttpGet]
        [Route("api/Orders/CreateInvoice/{id:int:max(10000)}")]
        public IHttpActionResult CreateInvoice(int id)
        {
            if (id == 0)
                return NotFound();
            var order = orderRepository.Get(id);
            if (order == null)
                return NotFound();

            ICreateInvoicesEngine createIinvoicesEngine = _BusinessEngineFactory.GetBusinessEngine<ICreateInvoicesEngine>();
            var createdInvoiceVM = createIinvoicesEngine.CreateIinvoice(id);
            return Ok(createdInvoiceVM);
        }
    }
}
