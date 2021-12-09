using BLToolkit.Mapping;
using HSBM.Common.Enums;
using HSBM.Common.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.HomeMaster
{
    public class RecentOrder
    {
        public int id { get; set; }

        public string OrderNo { get; set; }

        public DateTime OrderDate { get; set; }

        [MapIgnore]
        public string strOrderDate
        {
            get
            {
                try
                { return OrderDate.ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture); }
                catch (Exception)
                { return ""; }
            }
        }
        public string Customer { get; set; }

        public string FarmStaysName { get; set; }

        public decimal Amount { get; set; }

        public int Status { get; set; }

        public string GuestMobile { get; set; }

        public String StatusName
        {
            get
            {
                try
                {
                    return Helper.GetEnumDescription((BookingStatus)Status);
                }
                catch (Exception)
                { return ""; }
            }
        }
    }
}
