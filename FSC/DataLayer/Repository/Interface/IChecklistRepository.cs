using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSC.DataLayer.Repository.Interface
{
    public interface IChecklistRepository : IDataRepository<Checklist>
    {
        IEnumerable<Checklist> GetChecklistChild(int id);
        IEnumerable<Checklist> GetChecklistForUser(string userId);
        Checklist GetForUser(int id, string userId);
    }
}