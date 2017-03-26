using AutoMapper;
using FSC.DataLayer;
using FSC.ViewModels.Api;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace FSC.Controllers.api
{
    public class CheckListController : ApiController
    {
        private ApplicationDbContext applicationDB = null;

        public CheckListController()
        {
            applicationDB = new ApplicationDbContext();
        }

        [HttpGet]
        //[Route("api/CheckList/Get")]
        public IEnumerable<CheckList> Get()
        {
            var userId = User.Identity.GetUserId();
            var data = applicationDB.CheckLists
                .Where(x => x.ParentId == 0)
                .Where(x => x.UserId == userId)
                .ToList();

            return data;
        }

        [Route("api/CheckList/Get/{id:int:max(10000)}")]
        public IHttpActionResult Get(int id)
        {
            if (id == 0)
                return BadRequest();
            var userId = User.Identity.GetUserId();
            var checkList = applicationDB.CheckLists.Where(x => x.UserId == userId).First(x => x.Id == id);
            if (checkList == null)
                return NotFound();

            var result = Mapper.Map<CheckListDisplayVM>(checkList);
            var checkListItems = applicationDB.CheckLists.Where(x => x.ParentId == checkList.Id).ToList();
            result.Items = Mapper.Map<List<CheckListItem>>(checkListItems);

            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody]CreateChecklistVM body)
        {
            if (string.IsNullOrWhiteSpace(body.Description))
                return BadRequest();
            var newChecklist = new CheckList();
            newChecklist.Description = body.Description;
            newChecklist.UserId = User.Identity.GetUserId();
            applicationDB.CheckLists.Add(newChecklist);
            applicationDB.SaveChanges();
            body.Id = newChecklist.Id;

            return Ok(body);
        }

        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]CheckListDisplayVM value)
        {
            if (id == 0)
                return BadRequest();

            var checkList = Mapper.Map<CheckList>(value);
            var checkListItems = Mapper.Map<List<CheckList>>(value.Items);

            checkListItems.ForEach(x => x.ParentId = checkList.Id);
            checkListItems.Add(checkList);
            checkListItems.ForEach(x => x.UserId = User.Identity.GetUserId());
            checkListItems.ForEach(x =>
            {
                if (x.Id == 0)
                    applicationDB.Entry(x).State = EntityState.Added;
                else
                    applicationDB.Entry(x).State = EntityState.Modified;
            });
            applicationDB.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            if (id == 0)
                return BadRequest();
            var checklist = applicationDB.CheckLists.First(x => x.Id == id);
            if (checklist == null)
                return NotFound();

            applicationDB.CheckLists.Remove(checklist);
            var checklistItems = applicationDB.CheckLists.Where(x => x.ParentId == id);
            applicationDB.CheckLists.RemoveRange(checklistItems);
            applicationDB.SaveChanges();

            return Ok();
        }
    }
}
