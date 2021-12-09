using HSBM.Web.Helpers;
using System;
using System.Linq;
using System.Web.Mvc;
using HSBM.Common.Enums;
using System.Collections.Generic;
using System.Web.Routing;
using System.Web;

namespace HSBM.Web.Helpers
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class CustomAuthorize : AuthorizeAttribute
    {
        public Module moduleId { get; set; }
        public CustomAuthorize(object module = null)
        {

            if (module != null && module.GetType().BaseType != typeof(Enum))
                throw new ArgumentException("Module");


            if (module != null)
            {
                moduleId = (Module)Enum.Parse(typeof(Module), Enum.GetName(module.GetType(), module));
            }

        }

        public override void OnAuthorization(AuthorizationContext p_filterContext)
        {
            if (p_filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            try
            {

                if (HttpContext.Current.Request.Url.AbsoluteUri.ToLower().Contains("/admin"))
                {
                    if (SessionProxy.CheckIsAdminLogin() || SessionProxy.CheckIsSubAdminLogin())
                    {
                        if (moduleId > 0)
                        {
                            if (!SessionProxy.CheckModuleRights(moduleId))
                            {
                                p_filterContext.Result = new RedirectResult("/Admin/Information/AccessDenied");
                            }
                        }
                    }
                    else
                    {
                        SessionProxy.UserDetails = null;
                        p_filterContext.Result = //new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" } });
                        new RedirectResult("/Admin/Account/Login");
                    }
                }                
                else if (HttpContext.Current.Request.Url.AbsoluteUri.ToLower().Contains("/"))
                {
                    if (!SessionProxy.CheckUserIsLogin())
                    {
                        SessionProxy.CustomerDetails = null;
                        p_filterContext.Result = //new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" } });
                        new RedirectResult("/Customer/Login");
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }




    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class CustomAuthorizeAction : AuthorizeAttribute
    {
        public Module moduleId { get; set; }
        public ModuleAccess moduleaccess { get; set; }

        public CustomAuthorizeAction(object module, object action)
        {
            if (module != null && module.GetType().BaseType != typeof(Enum))
                throw new ArgumentException("roles");

            if (action != null && action.GetType().BaseType != typeof(Enum))
                throw new ArgumentException("action");


            if (module != null && action != null)
            {
                moduleId = (Module)Enum.Parse(typeof(Module), Enum.GetName(module.GetType(), module));
                moduleaccess = (ModuleAccess)Enum.Parse(typeof(ModuleAccess), Enum.GetName(action.GetType(), action));
            }
        }

        public override void OnAuthorization(AuthorizationContext p_filterContext)
        {
            if (p_filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            try
            {
                if (HttpContext.Current.Request.Url.AbsoluteUri.ToLower().Contains("/admin"))
                {
                    if (SessionProxy.CheckIsAdminLogin() || SessionProxy.CheckIsSubAdminLogin())
                    {
                        if (moduleId > 0 && moduleaccess > 0)
                        {
                            if (!SessionProxy.CheckModuleRights(moduleId))
                            {
                                p_filterContext.Result = new RedirectResult("/Admin/Information/AccessDenied");
                            }
                            else
                            {
                                if (!SessionProxy.CheckModuleAccess(moduleId, moduleaccess))
                                {
                                    p_filterContext.Result = new RedirectResult("/Admin/Information/AccessDenied");
                                }
                            }
                        }
                    }
                    else
                    {
                        p_filterContext.Result = new RedirectResult("/Admin/Account/Login");
                    }
                }
                
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

        }
    }


}