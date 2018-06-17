using FSC.DataLayer;
using FSC.Moduls.FormFilters;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
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
            if (filterName.Equals("OrderListFilter"))
            {
                filterFactory.Filters = new List<IFilter>();
                var userOrderListFilter = applicationDB.Users.FirstOrDefault(x => x.Id.Equals(userId));
                dynamic filters = JArray.Parse(userOrderListFilter.PhoneNumber) as JArray;

                for (int i = 0; i < filters.Count; i++)
                {
                    switch ((string)filters[i].controlType)
                    {
                        case "textbox":
                            TextBoxFilter filterTextBox = filters[i].ToObject<TextBoxFilter>();
                            filterFactory.Filters.Add(filterTextBox);
                            break;
                        case "dropdown":
                            DropdownFilter filterDropdown = filters[i].ToObject<DropdownFilter>();
                            filterFactory.Filters.Add(filterDropdown);
                            break;
                        default:
                            break;
                    }
                }
            }
            return Ok(filterFactory);
        }

        [HttpPatch]
        [Route("api/Filter/SaveFilter/{filterName}")]
        public IHttpActionResult SaveFilter(string filterName, [FromBody]string body)
        {
            if (string.IsNullOrEmpty(filterName))
                return NotFound();
            var userFilter = applicationDB.Users.FirstOrDefault(x => x.Id.Equals(userId));
            userFilter.PhoneNumber = body;
            applicationDB.SaveChanges();
            return Ok();
        }
    }
}
