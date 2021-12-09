using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Web.SessionState;
using HSBM.EntityModel.SystemUsers;
using Newtonsoft.Json;
using HSBM.Common.Utils;
using HSBM.Common.Enums;


namespace HSBM.Web.Helpers
{
    public class SessionProxy
    {
        #region Constants

        //Admin
        private const string USERDETAILS = "usersdata";
        private const string CUSTOMERDETAILS = "customerdetails";
        
        private const string ROOMXMLBOOKING = "RoomXMLBooking";
        private const string BASECURRENCY = "BaseCurrency";
        private const string CULTURELANGUAGE = "CultureLanguage";

        #endregion


        public static SessionProxy Current
        {
            get
            {
                SessionProxy sessionProxy = (SessionProxy)HttpContext.Current.Session["__ApplicationSession__"];
                if (sessionProxy == null)
                {
                    sessionProxy = new SessionProxy();
                    HttpContext.Current.Session["__ApplicationSession__"] = sessionProxy;
                }
                return sessionProxy;
            }
            set { SessionProxy mysession = (SessionProxy)value; }
        }



        public static SystemUsers UserDetails
        {
            get
            {
                if (HttpContext.Current.Session[USERDETAILS] != null)
                {
                    try
                    {
                        return (SystemUsers)HttpContext.Current.Session[USERDETAILS];
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }


                //return StoreintoCookie();
            }
            set
            {
                HttpContext.Current.Session[USERDETAILS] = value;

                //if (value == null)
                //{
                //    RemoveCookie();
                //}
                //else
                //{
                //    HttpContext.Current.Session[USERDETAILS] = StoreintoCookie(value);
                //}
            }
        }


        public static SystemUsers CustomerDetails
        {
            get
            {
                if (HttpContext.Current.Session[CUSTOMERDETAILS] != null)
                {
                    try
                    {
                        return (SystemUsers)HttpContext.Current.Session[CUSTOMERDETAILS];
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }


                //return StoreintoCookie();
            }
            set
            {
                HttpContext.Current.Session[CUSTOMERDETAILS] = value;

                //if (value == null)
                //{
                //    RemoveCookie();
                //}
                //else
                //{
                //    HttpContext.Current.Session[USERDETAILS] = StoreintoCookie(value);
                //}
            }
        }



        private static void RemoveCookie(SystemUsers obj = null)
        {
            HttpCookie _cookie = HttpContext.Current.Request.Cookies[USERDETAILS];
            if (_cookie != null)
            {
                _cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(_cookie);
            }
        }

        private static SystemUsers StoreintoCookie(SystemUsers obj = null)
        {
            HttpCookie _cookie = HttpContext.Current.Request.Cookies[USERDETAILS];
            if (obj == null)
            {
                if (_cookie == null || string.IsNullOrEmpty(_cookie.Value))
                {
                    return null;
                }
                else
                {
                    return JsonConvert.DeserializeObject<SystemUsers>(Helper.Decrypt(_cookie.Value));
                }
            }
            else
            {
                _cookie = new HttpCookie(USERDETAILS);
                _cookie.Value = Helper.Encrypt(JsonConvert.SerializeObject(obj));
            }

            _cookie.Expires = DateTime.UtcNow.AddDays(30);
            HttpContext.Current.Response.Cookies.Add(_cookie);

            return obj;
        }


        public static bool CheckModuleAccess(Module module, ModuleAccess access)
        {
            int moduleid = (int)module;
            var _CuurentUserDetails = UserDetails;
            if (_CuurentUserDetails.RoleMasterDetails.Where(t => t.RoleModuleID == moduleid).Any())
            {
                switch (access)
                {
                    case ModuleAccess.CanView:
                        return _CuurentUserDetails.RoleMasterDetails.Where(t => t.RoleModuleID == moduleid && t.CanView).Any();
                    case ModuleAccess.CanAdd:
                        return _CuurentUserDetails.RoleMasterDetails.Where(t => t.RoleModuleID == moduleid && t.CanAdd).Any();
                    case ModuleAccess.CanUpdate:
                        return _CuurentUserDetails.RoleMasterDetails.Where(t => t.RoleModuleID == moduleid && t.CanUpdate).Any();
                    case ModuleAccess.CanDelete:
                        return _CuurentUserDetails.RoleMasterDetails.Where(t => t.RoleModuleID == moduleid && t.CanDelete).Any();
                    default:
                        break;
                }
            }
            return false;

        }

        public static bool CheckModuleRights(Module module)
        {
            return UserDetails.RoleMasterDetails.Where(t => t.RoleModuleID == (long)module).Any();
        }

        public static bool CheckIsAdminLogin()
        {
            if (UserDetails != null)
            {
                if (UserDetails.UserType == Convert.ToInt32(UserTypes.Admin))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool CheckIsSubAdminLogin()
        {
            if (UserDetails != null)
            {
                if (UserDetails.UserType == Convert.ToInt32(UserTypes.SubAdmin))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool CheckUserIsLogin()
        {
            if (CustomerDetails != null)
            {
                if (CustomerDetails.UserType == Convert.ToInt32(UserTypes.Customer))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        public static bool CheckAssingedModule(HSBM.Common.Enums.Module enumModule)
        {
            var ud = UserDetails.RoleMasterDetails.Where(t => t.RoleModuleID == (int)enumModule && t.CanView).Any();
            return ud;
        }

        public static bool CanAdd(HSBM.Common.Enums.Module enumModule)
        {
            return UserDetails.RoleMasterDetails.Where(t => t.RoleModuleID == (int)enumModule && t.CanAdd).Any();
        }
        public static bool CanUpdate(HSBM.Common.Enums.Module enumModule)
        {
            return UserDetails.RoleMasterDetails.Where(t => t.RoleModuleID == (int)enumModule && t.CanUpdate).Any();
        }
        public static bool CanDelete(HSBM.Common.Enums.Module enumModule)
        {
            return UserDetails.RoleMasterDetails.Where(t => t.RoleModuleID == (int)enumModule && t.CanDelete).Any();
        }
        public static bool CanView(HSBM.Common.Enums.Module enumModule)
        {
            return UserDetails.RoleMasterDetails.Where(t => t.RoleModuleID == (int)enumModule && t.CanView).Any();
        }

        public static string BaseCurrency
        {
            get
            {
                if (HttpContext.Current.Session[BASECURRENCY] != null)
                {
                    return HttpContext.Current.Session[BASECURRENCY].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                HttpContext.Current.Session[BASECURRENCY] = value;
            }
        }

        public static string CultureLanguage
        {
            get
            {
                if (HttpContext.Current.Session[CULTURELANGUAGE] != null)
                {
                    return HttpContext.Current.Session[CULTURELANGUAGE].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                HttpContext.Current.Session[CULTURELANGUAGE] = value;
            }
        }

    }

}