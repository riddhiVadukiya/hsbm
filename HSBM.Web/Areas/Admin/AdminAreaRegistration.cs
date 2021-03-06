using System.Web.Mvc;

namespace HSBM.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                 name: "Admindefault",
                 url: "Admin/{controller}/{action}/{id}",
                 defaults: new { area = "Admin", controller = "Home", action = "Index", id = UrlParameter.Optional },
                 namespaces: new string[] { "HSBM.Web.Areas.Admin.Controllers" }
            );
        }
    }
}