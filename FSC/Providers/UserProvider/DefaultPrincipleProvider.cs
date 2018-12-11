using System;
using System.Web;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using FSC.Providers.UserProvider.Interface;

namespace FSC.Providers.UserProvider
{
    public class DefaultPrincipleProvider : IPrincipleProvider
    {
        public IPrincipal User { get { return HttpContext.Current.User; } }
        public DefaultPrincipleProvider()
        { }
    }
}