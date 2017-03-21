using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BinarySurveyPOC.Startup))]
namespace BinarySurveyPOC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
