using System;
using System.ComponentModel;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HSBM.Common.Utils;
using System.Globalization;
namespace HSBM.EntityModel.InventoryMaster
{  
    public class InventoryMaster
    {
        public long Id { get; set; }

        [Required]
        public long FarmstaysId { get; set; }

        [Required]
        public long RoomId { get; set; }
        
        private string _BookingDate;
        public string BookingDate { get { return string.IsNullOrEmpty(_BookingDate) ? "" : Convert.ToDateTime(_BookingDate).ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture); } set { _BookingDate = value; } }

        private string _StartDate;
        [Required]
        public string StartDate { get { return string.IsNullOrEmpty(_StartDate) ? "" : Convert.ToDateTime(_StartDate).ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture); } set { _StartDate = value; } }

        private string _EndDate;
        [Required]
        public string EndDate { get { return string.IsNullOrEmpty(_EndDate) ? "" : Convert.ToDateTime(_EndDate).ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture); } set { _EndDate = value; } }

        [Required]
        [Range(1, Double.MaxValue, ErrorMessage = "The field Number Of Property must be greater than zero.")]
        public int NumberOfProperty { get; set; }
        
        public bool OnSite { get; set; }

        public long? OrderId { get; set; }

        public string OrderNo { get; set; }

        public Guid? BookingGroupId { get; set; }

        public string Name { get; set; }

    }
}