using HSBM.Common.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.FarmStays
{
    public class PlanValue
    {
        public int RoomId { get; set; }
        public int RatePlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public decimal Double { get; set; }
        public decimal Triple { get; set; }
        public decimal ExtraBed { get; set; }
        public decimal ExtraChild { get; set; }
        public decimal ExtraAdult { get; set; }
    }
}
