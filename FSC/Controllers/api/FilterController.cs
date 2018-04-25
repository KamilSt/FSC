using FSC.DataLayer;
using FSC.Moduls.FormFilters;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FSC.Controllers.api
{
    public class FilterController : ApiController
    {
        private ApplicationDbContext applicationDB = null;
        private readonly string userId = null;
        public FilterController()
        {
            applicationDB = new ApplicationDbContext();
            userId = User.Identity.GetUserId();
        }

        [HttpPost]
        [Route("api/Filter/Get/")]
        public IHttpActionResult GetFiltersFor([FromBody]string filterName)
        {
            if (string.IsNullOrEmpty(filterName))
                return NotFound();
            var filterFactory = FilterFactory.GetFilter(filterName);
            return Ok(filterFactory);
        }
    }
}
