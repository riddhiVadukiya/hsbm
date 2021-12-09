using BLToolkit.Mapping;
using HSBM.Common.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.HomeMaster
{
    public class TotalBooking
    {
        public string IsDay { get; set; }
        public string IsWeek { get; set; }
        public string IsMonth { get; set; }
        public string IsYear { get; set; }
        public string IsCustom { get; set; }        
        public string Month { get; set; }
        public string Time { get; set; }
        public string Week { get; set; }
        public string Year { get; set; }        
        public DateTime date { get; set; }
        public int Cancelled { get; set; }
        public int Booked { get; set; }
        [MapIgnore]
        public string strDate
        {
            get
            {
                try
                {
                    if (IsDay == "1")
                    {
                        return Time;
                    }
                    else if (IsWeek == "1")
                    {
                        return Week;
                    }
                    else if (IsMonth == "1")
                    {
                        return Month;
                        //return date.ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);
                    }
                    else if (IsYear == "1")
                    {
                        return Year;
                    }
                    else if (IsCustom == "1")
                    {
                        return Month;
                    }
                    else
                    {
                        return date.ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);

                    }
                }
                catch (Exception)
                { return ""; }
            }
        }
    }
}
