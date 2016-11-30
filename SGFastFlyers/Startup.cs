using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SGFastFlyers.Startup))]
namespace SGFastFlyers
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
