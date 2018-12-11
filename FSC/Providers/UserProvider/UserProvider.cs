using System;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNet.Identity;
using FSC.Providers.UserProvider.Interface;

namespace FSC.Providers.UserProvider
{
    public class UserProvider : IUserProvider
    {
        private readonly ClaimsIdentity identity;
        public string GuidId { get; set; }
        public string UserName { get; set; }
        public UserProvider(IPrincipleProvider provider)
        {
            identity = (ClaimsIdentity)provider.User.Identity;
            GuidId = provider.User.Identity.GetUserId();
            UserName = provider.User.Identity.GetUserName();
        }
    }
}