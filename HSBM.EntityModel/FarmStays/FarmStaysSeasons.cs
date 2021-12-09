using HSBM.Common.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.FarmStays
{
    public class FarmStaysSeasons
    {
        public int Id { get; set; }
        public long RoomId { get; set; }
        public DateTime BookingDate { get; set; }
        public int RatePlanId { get; set; }
        public decimal Price { get; set; }
        public decimal Single { get; set; }
        public decimal Double { get; set; }
        public decimal Triple { get; set; }
        public decimal ExtraBed { get; set; }
        public decimal ExtraChild { get; set; }
        public decimal ExtraAdult { get; set; }
        public Guid GroupId { get; set; }
        public bool IsDeleted { get; set; }


        //public string StartDate
        //{
        //    get
        //    {
        //        return   Convert.ToDateTime(StartDate).ToShortTimeString();
        //    }
        //    set
        //    {   }
        //}
        //public string EndDate { get; set; }
        private string _StartDate;
        public string StartDate { get { return string.IsNullOrEmpty(_StartDate) ? "" : Convert.ToDateTime(_StartDate).ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture); } set { _StartDate = value; } }

        private string _EndDate;
        public string EndDate { get { return string.IsNullOrEmpty(_EndDate) ? "" : Convert.ToDateTime(_EndDate).ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture); } set { _EndDate = value; } }

    }
}
