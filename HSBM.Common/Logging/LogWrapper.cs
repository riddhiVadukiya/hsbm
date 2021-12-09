using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
namespace HSBM.Common.Logging
{
    public static class LogWrapper
    {
        public static void Configure(string filePath)
        {

            if (ConfigurationManager.AppSettings.Get("LogSystemType") == "log4net")
            {
                log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(filePath));
            }

        }

        public static ILogger GetLogger(Type type)
        {

            if (ConfigurationManager.AppSettings.Get("LogSystemType") == "log4net")
            {
                return new Log4NetWrapper(type);
            }
            return null;

        }
        public static ILogger GetLogger(string loggerName)
        {

            if (ConfigurationManager.AppSettings.Get("LogSystemType") == "log4net")
            {
                return new Log4NetWrapper(loggerName);
            }

            return null;
        }
    }
}
