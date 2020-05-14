using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PayrollSystem.Startup))]
namespace PayrollSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
