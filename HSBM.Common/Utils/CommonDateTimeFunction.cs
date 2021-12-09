using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Common.Utils
{
    public static class CommonDateTimeFunction
    {
        public static DateTime ConvertCentralStandardTime(DateTime p_DateTime, String p_Source_Time_Zone)
        {
            if (p_Source_Time_Zone == "")
            {
                return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(p_DateTime, TimeZoneInfo.Local.Id, "Central Standard Time");
            }
            else
            {
                return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(p_DateTime, p_Source_Time_Zone, "Central Standard Time");
            }
        }

        public static DateTime ConvertDateTimeToTimeZone(DateTime p_DateTime, string p_TimeZone)
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(p_DateTime, TimeZoneInfo.Local.Id, p_TimeZone);
        }

        public static DateTime GetCurrentCstDateTime()
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, TimeZoneInfo.Local.Id, "Central Standard Time");
        }

        public static DateTime GetCurrentCstDate()
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.Date, TimeZoneInfo.Local.Id, "Central Standard Time");
        }
        public static DateTime GetCurrentDateTime()
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, TimeZoneInfo.Local.Id, TimeZoneInfo.Local.Id);
        }

        public static DateTime ConvertDateTimeToDestinationTimeZone(DateTime p_DateTime, string p_TimeZone)
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(p_DateTime, "Central Standard Time", p_TimeZone);
        }
    }
}
