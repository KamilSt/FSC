using FSC.DataLayer;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
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
        //[Route("api/CheckList/Get")]
        public OrderListVM Get()
        {
            OrderListVM vm = new OrderListVM();
            var list = applicationDB.Orders.ToList();

            foreach (var item in list) //autoMapper
            {
                vm.Orders.Add(new OrderListItemVM()
                {
                    CompanyName = item.Customer.CompanyName,
                    Date = item.OrderDateTime.ToShortDateString(),
                    Total = 5,
                    Invoiced = item.Invoiced
                });
            }
            vm.Count = vm.Orders.Count();

            return vm;
        }
    }
    public class OrderListVM
    {
        public OrderListVM()
        {
            this.Orders = new List<OrderListItemVM>();
        }

        public List<OrderListItemVM> Orders { get; internal set; }
        public int Count { get; set; }
        public int Page { get; set; }
    }
    public class OrderListItemVM
    {
        public string CompanyName { get; set; }
        public string Date { get; set; }
        public decimal Total { get; set; }
        public bool Invoiced { get; set; }
    }


}
