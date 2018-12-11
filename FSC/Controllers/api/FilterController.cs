using FSC.DataLayer;
using FSC.Moduls.FormFilters;
using Newtonsoft.Json.Linq;
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
    public class FilterController : ApiController
    {
        private ApplicationDbContext applicationDB = null;
        private readonly string userId = null;
        public FilterController(IUserProvider user)
        {
            applicationDB = new ApplicationDbContext();
            userId = user.GuidId;
        }

        [HttpPost]
        [Route("api/Filter/Get/")]
        public IHttpActionResult GetFiltersFor([FromBody]string filterName)
        {
            if (string.IsNullOrEmpty(filterName))
                return NotFound();
            var filterFactory = FilterFactory.GetFilter(filterName);
            var filtersStatusSaved = applicationDB.FiltersStatus.SingleOrDefault(x => x.UserId.Equals(userId) && x.FilterName.Equals(filterName));
            if (filtersStatusSaved == null || filterFactory.Version > filtersStatusSaved.Version)
                return Ok(filterFactory);

            filterFactory.Filters = new List<IFilter>();
            dynamic filters = JArray.Parse(filtersStatusSaved.Filters) as JArray;

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

            return Ok(filterFactory);
        }

        [HttpPatch]
        [Route("api/Filter/SaveFilter/{filterName}/{version}")]
        public IHttpActionResult SaveFilter(string filterName, int version, [FromBody]string filters)
        {
            if (string.IsNullOrEmpty(filterName))
                return NotFound();
            var statusFilters = applicationDB.FiltersStatus.SingleOrDefault(x => x.UserId.Equals(userId) && x.FilterName.Equals(filterName));
            if (statusFilters == null)
            {
                statusFilters = new FiltersStatus();
                statusFilters.UserId = userId;
                statusFilters.FilterName = filterName;
            }
            statusFilters.Filters = filters;
            statusFilters.Version = version;
            if (statusFilters.Id == 0)
                applicationDB.FiltersStatus.Add(statusFilters);
            else
                applicationDB.Entry(statusFilters).State = EntityState.Modified;
            applicationDB.SaveChangesAsync();

            return Ok();
        }
    }
}
