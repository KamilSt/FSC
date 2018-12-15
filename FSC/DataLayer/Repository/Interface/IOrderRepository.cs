using FSC.ViewModels.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSC.DataLayer.Repository.Interface
{
    public interface IOrderRepository : IDataRepository<Order>
    {
        IEnumerable<Order> Get(string filters);
    }
}