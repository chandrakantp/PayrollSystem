using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(edueTree.Startup))]
namespace edueTree
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
