using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HSBM.Web
{
    public class Log4NetServiceUserPatternConverter //: PatternLayoutConverter
    {
        //protected override void Convert(System.IO.TextWriter writer, LoggingEvent loggingEvent)
        protected void Convert(System.IO.TextWriter writer, LoggingEvent loggingEvent)
        {
            try
            {
                string name = GetUserName();

                writer.Write(name);
            }
            catch
            {
                writer.Write("");
            }
        }

        public static string GetUserName()
        {
            try
            {
                string name = "";

               
                return name;
            }
            catch
            {
                return "";
            }
        }
    }
}