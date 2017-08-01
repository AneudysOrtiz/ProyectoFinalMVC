using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SistemaRH3.Startup))]
namespace SistemaRH3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
