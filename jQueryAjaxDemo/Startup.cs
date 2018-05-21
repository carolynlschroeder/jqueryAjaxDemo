using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(jQueryAjaxDemo.Startup))]
namespace jQueryAjaxDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
