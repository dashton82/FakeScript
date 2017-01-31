using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Fake.TestProject.Startup))]
namespace Fake.TestProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
