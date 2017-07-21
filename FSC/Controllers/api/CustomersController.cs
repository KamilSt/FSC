using FSC.DataLayer;
using FSC.ViewModels.Api;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FSC.Controllers.api
{
    [Authorize]
    public class CustomersController : ApiController
    {
        private ApplicationDbContext applicationDB = null;
        private readonly string userId = null;
        public CustomersController()
        {
            applicationDB = new ApplicationDbContext();
            userId = User.Identity.GetUserId();
        }

        public IEnumerable<CustomersVM> Get()
        {
            var listItem = applicationDB.Customers.Select(cutomer => new CustomersVM
            {
                CustomerId = cutomer.CustomerId,
                CompanyName = cutomer.CompanyName,
                NIP = cutomer.NIP,
                AccountNumber = cutomer.AccountNumber,
                Address = cutomer.Address,
                City = cutomer.City,
                Phone = cutomer.Phone,

            });

            return listItem;
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody]CustomersVM customerVM)
        {
            if (customerVM == null)
                return BadRequest();
            var customer = new Customer()
            {
                CompanyName = customerVM.CompanyName,
                Address = customerVM.Address,
                City = customerVM.City,
                AccountNumber = customerVM.AccountNumber,
                NIP = customerVM.NIP,
                Phone = customerVM.Phone
            };
            applicationDB.Customers.Add(customer);
            applicationDB.SaveChanges();
            return Created(customer.CustomerId.ToString(), customer);
        }

        [HttpPut]
        public IHttpActionResult Put(int Id, [FromBody]CustomersVM customerVM)
        {
            if (Id == 0)
                return BadRequest();
            var customer = applicationDB.Customers.FirstOrDefault(x => x.CustomerId == Id);
            if (customer == null)
                return NotFound();
            customer.CompanyName = customerVM.CompanyName;
            customer.Address = customerVM.Address;
            customer.City = customerVM.City;
            customer.AccountNumber = customerVM.AccountNumber;
            customer.NIP = customerVM.NIP;
            customer.Phone = customerVM.Phone;

            applicationDB.Entry(customer).State = EntityState.Modified;
            applicationDB.SaveChanges();
            return Ok();
        }
    }   
}
