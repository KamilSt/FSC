using AutoMapper;
using FSC.DataLayer;
using FSC.ViewModels.Api;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FSC.Providers.UserProvider.Interface;

namespace FSC.Controllers.api
{
    [Authorize]
    public class CustomersController : ApiController
    {
        private ApplicationDbContext applicationDB = null;
        private readonly string userId = null;
        public CustomersController(IUserProvider user)
        {
            applicationDB = new ApplicationDbContext();
            userId = user.GuidId;
        }

        public IEnumerable<CustomersVM> Get()
        {
            var customers = Mapper.Map<IEnumerable<CustomersVM>>(applicationDB.Customers);
            return customers;
        }

        [Route("api/Customers/Get/{id:int:max(10000)}")]
        public CustomersVM Get(int id)
        {
            var customer = applicationDB.Customers.FirstOrDefault(x => x.CustomerId == id);
            var result = Mapper.Map<CustomersVM>(customer);
            return result;
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody]CustomersVM customerVM)
        {
            if (customerVM == null)
                return BadRequest();
            var customer = Mapper.Map<CustomersVM, Customer>(customerVM);
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
            Mapper.Map<CustomersVM, Customer>(customerVM, customer);
            applicationDB.Entry(customer).State = EntityState.Modified;
            applicationDB.SaveChanges();
            return Ok();
        }
    }
}
