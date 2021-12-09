using HSBM.Common.Logging;
using HSBM.Common.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HSBM.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            CultureInfo cInfo = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();//new CultureInfo("en-IN");
            cInfo.DateTimeFormat.ShortDatePattern = Helper.DefaultDateFormat();
            cInfo.DateTimeFormat.LongDatePattern = Helper.DefaultDateFormat() + " HH:mm:ss,fff";
            cInfo.DateTimeFormat.DateSeparator = "-";

            CultureInfo.DefaultThreadCurrentCulture = cInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cInfo;

            Thread.CurrentThread.CurrentCulture = cInfo;
            Thread.CurrentThread.CurrentUICulture = cInfo;

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);



            #region log4net
            /// log4net.Config.XmlConfigurator.Configure();

            if (ConfigurationManager.AppSettings.Get("LogSystemType") == "log4net")
            {
                string l4net = Server.MapPath("~/log4net.config");
                LogWrapper.Configure(l4net);
            }
            _ILogger.Debug("Registering Routes...");
            #endregion
        }

        void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 60;
        }

        public static string FarmStayImagePath
        {
            get
            {
                return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["FarmStayImagePath"]);
            }
        }
        public static string BannerImagePath
        {
            get
            {
                return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["BannerImagePath"]);
            }
        }
        public static string BlogImagePath
        {
            get
            {
                return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["BlogImagePath"]);
            }
        }
        public static string AmenityImagePath
        {
            get
            {
                return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AmenityImagePath"]);
            }
        }
        public static string DefaultImageLocation
        {
            get
            {
                return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DefaultImageLocation"]);
            }
        }

    }
}
