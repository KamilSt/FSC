using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace FSC.Providers.UserProvider.Interface
{
    public interface IPrincipleProvider
    {
        IPrincipal User { get; }
    }
}