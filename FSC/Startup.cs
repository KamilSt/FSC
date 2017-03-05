using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FSC.Startup))]
namespace FSC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
