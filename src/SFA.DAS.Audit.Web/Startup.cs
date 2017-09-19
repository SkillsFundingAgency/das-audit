using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SFA.DAS.Audit.Web.Startup))]

namespace SFA.DAS.Audit.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}