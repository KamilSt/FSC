using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using FSC.DataLayer;
using FSC.ViewModels.Api;
using FSC.DataLayer.Repository.Interface;
using FSC.Providers.UserProvider.Interface;

namespace FSC.Controllers.api
{
    [Authorize]
    public class CheckListController : ApiController
    {
        private IChecklistRepository checklistRepository = null;
        private readonly string userId = null;
        public CheckListController(IChecklistRepository _checklistRepository, IUserProvider user)
        {
            checklistRepository = _checklistRepository;
            userId = user.GuidId;
        }

        [HttpGet]
        //[Route("api/CheckList/Get")]
        public IEnumerable<Checklist> Get()
        {
            return checklistRepository.GetChecklistForUser(userId).ToList();
        }

        [Route("api/CheckList/Get/{id:int:max(10000)}")]
        public IHttpActionResult Get(int id)
        {
            if (id == 0)
                return BadRequest();
            var checkList = checklistRepository.Get(id);
            if (checkList == null)
                return NotFound();
            var result = Mapper.Map<CheckListDisplayVM>(checkList);
            var checkListItems = checklistRepository.GetChecklistChild(checkList.Id);
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
            checklistRepository.Add(newChecklist);

            return Created(newChecklist.Id.ToString(), checklist);
        }

        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]Checklist value)
        {
            if (id == 0)
                return BadRequest();
            var checklist = checklistRepository.Get(id);
            if (checklist == null)
                return NotFound();
            checklist.IsCompleted = value.IsCompleted;
            checklist.Description = value.Description;
            checklistRepository.Update(checklist);

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            if (id == 0)
                return BadRequest();
            var checklist = checklistRepository.Get(id);
            if (checklist == null)
                return NotFound();

            var checklistItems = checklistRepository.GetChecklistChild(id);
            foreach (var item in checklistItems)
                checklistRepository.Remove(item);
            checklistRepository.Remove(checklist);

            return Ok();
        }
    }
}
