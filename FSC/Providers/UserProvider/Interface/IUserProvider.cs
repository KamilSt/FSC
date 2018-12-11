using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSC.Providers.UserProvider.Interface
{
    public interface IUserProvider
    {
        string GuidId { get; set; }
        string UserName { get; set; }
    }
}