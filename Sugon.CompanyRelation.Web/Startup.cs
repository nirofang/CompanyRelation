using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sugon.CompanyRelation.Web.Startup))]
namespace Sugon.CompanyRelation.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
