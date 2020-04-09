using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Alumni_Student_Portal.Startup))]
namespace Alumni_Student_Portal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
