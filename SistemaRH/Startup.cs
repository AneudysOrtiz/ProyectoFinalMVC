using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SistemaRH.Startup))]
namespace SistemaRH
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
