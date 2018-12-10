using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FSC.DataLayer.Repository.Interface;

namespace FSC.DataLayer.Repository
{
    public class ChecklistRepository : DataRepositoryBase<Checklist>, IChecklistRepository
    {
        protected override Checklist AddEntity(ApplicationDbContext entityContext, Checklist entity)
        {
            return entityContext.CheckLists.Add(entity);
        }
        protected override Checklist UpdateEntity(ApplicationDbContext entityContext, Checklist entity)
        {
            return entityContext.CheckLists.FirstOrDefault(e => e.Id == entity.Id);
        }
        protected override IQueryable<Checklist> GetEntities(ApplicationDbContext entityContext)
        {
            return entityContext.CheckLists;
        }
        protected override Checklist GetEntity(ApplicationDbContext entityContext, int id)
        {
            return entityContext.CheckLists.Where(x => x.Id == id).SingleOrDefault();
        }

        public IEnumerable<Checklist> GetChecklistChild(int id)
        {
            using (ApplicationDbContext entityContext = new ApplicationDbContext())
            {
                return entityContext.CheckLists.Where(x => x.ParentId == id).ToList();
            }
        }
        public IEnumerable<Checklist> GetChecklistForUser(string userId)
        {
            using (ApplicationDbContext entityContext = new ApplicationDbContext())
            {
                return entityContext.CheckLists.Where(x => x.ParentId == 0).Where(x => x.User.Id == userId).ToList();
            }
        }
        public Checklist GetForUser(int id, string userId)
        {
            using (ApplicationDbContext entityContext = new ApplicationDbContext())
            {
                return entityContext.CheckLists.Where(y => y.Id == id).Where(u => u.User.Id == userId).FirstOrDefault();
            }
        }
    }
}