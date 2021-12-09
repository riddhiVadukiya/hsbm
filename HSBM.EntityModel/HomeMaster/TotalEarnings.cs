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
    public class TotalEarnings
    {
        public string IsDay { get; set; }
        public string IsWeek { get; set; }
        public string IsMonth { get; set; }
        public string IsYear { get; set; }
        public string IsCustom { get; set; }        
        public string Time { get; set; }
        public string Week { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalBooking { get; set; }
        public decimal TotalEarning { get; set; }
        [MapIgnore]
        private string _strDate;
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
                        //return Date.ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);
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
                        return Date.ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);

                    }
                }
                catch (Exception)
                { return ""; }
            }
            set {
                _strDate = value;
            }
        }
    }
}
