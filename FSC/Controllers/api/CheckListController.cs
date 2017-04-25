using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using FSC.DataLayer;
using FSC.ViewModels.Api;

namespace FSC.Controllers.api
{
    [Authorize]
    public class CheckListController : ApiController
    {
        private ApplicationDbContext applicationDB = null;
        private readonly string userId = null;
        public CheckListController()
        {
            applicationDB = new ApplicationDbContext();
            userId = User.Identity.GetUserId();
        }

        [HttpGet]
        //[Route("api/CheckList/Get")]
        public IEnumerable<Checklist> Get()
        {
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
            var checkList = applicationDB.CheckLists.Where(x => x.UserId == userId).FirstOrDefault(x => x.Id == id);
            if (checkList == null)
                return NotFound();
            var result = Mapper.Map<CheckListDisplayVM>(checkList);
            var checkListItems = applicationDB.CheckLists.Where(x => x.ParentId == checkList.Id);
            result.Items = Mapper.Map<List<CheckListItem>>(checkListItems);

            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody]CreateChecklistVM checklist)
        {
            if (string.IsNullOrWhiteSpace(checklist.Description))
                return BadRequest();
            var newChecklist = new Checklist()
            {
                Description = checklist.Description,
                ParentId = checklist.ParentId,
                UserId = userId,
            };
            applicationDB.CheckLists.Add(newChecklist);
            applicationDB.SaveChanges();
            checklist.Id = newChecklist.Id;

            return Created(checklist.Id.ToString(), checklist);
        }

        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]Checklist value)
        {
            if (id == 0)
                return BadRequest();
            var checklist = applicationDB.CheckLists.FirstOrDefault(x => x.Id == id);
            if (checklist == null)
                return NotFound();
            checklist.IsCompleted = value.IsCompleted;
            checklist.Description = value.Description;

            applicationDB.Entry(checklist).State = EntityState.Modified;
            applicationDB.SaveChanges();
            
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            if (id == 0)
                return BadRequest();
            var checklist = applicationDB.CheckLists.FirstOrDefault(x => x.Id == id);
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
