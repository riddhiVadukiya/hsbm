using Microsoft.Owin;
using Owin;
using System.Net;

[assembly: OwinStartup(typeof(HSBM.Web.Startup))]
namespace HSBM.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
    }
}
