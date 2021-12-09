using BLToolkit.Mapping;
using HSBM.Common.Enums;
using HSBM.Common.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSBM.EntityModel.InventoryMaster;

namespace HSBM.EntityModel.OrdersMaster
{
    public class OrdersMaster
    {
        public long Id { get; set; }
        [DisplayName("Order Id")]
        public string OrderNo { get; set; }
        [DisplayName("Order Date")]
        public DateTime OrderDate { get; set; }
        //private string _OrderDate;
        //[DisplayName("Order Date")]
        //public string OrderDate { get { return string.IsNullOrEmpty(_OrderDate) ? "" : Convert.ToDateTime(_OrderDate).ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture); } set { _OrderDate = value; } }

        [MapIgnore]
        public string strCreatedDate
        {
            get
            {
                try
                { return OrderDate.ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture); }
                catch (Exception)
                { return ""; }
            }
        }
        [DisplayName("Farm stays id")]
        public long Farmstaysid { get; set; }

        [DisplayName("Customer Id")]
        public long CustomerId { get; set; }

        [DisplayName("CheckIn Date")]
        public DateTime CheckInDate { get; set; }

        [MapIgnore]
        public string strCheckInDate
        {
            get
            {
                try
                { return CheckInDate.ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture); }
                catch (Exception)
                { return ""; }
            }
        }

        [DisplayName("CheckOut Date")]
        public DateTime CheckOutDate { get; set; }

        [MapIgnore]
        public string strCheckOutDate
        {
            get
            {
                try
                { return CheckOutDate.ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture); }
                catch (Exception)
                { return ""; }
            }
        }

        [DisplayName("Number of people")]
        public long NoOfPeople { get; set; }

        [DisplayName("Amount")]
        public decimal Amount { get; set; }

        [DisplayName("Early Bird Discount")]
        public decimal EBDAmount { get; set; }
        [DisplayName("Discount Amount")]
        public decimal DiscountAmount { get; set; }

        [DisplayName("Net Amount")]
        public decimal NetAmount { get; set; }

        [DisplayName("Farm /Home Stays Name")]
        public string FarmStaysName { get; set; }

        [DisplayName("Check In Time")]
        public DateTime CheckInTime { get; set; }

        [DisplayName("Check Out Time")]
        public DateTime CheckOutTime { get; set; }

        [DisplayName("Is cancellation Policy")]
        public bool CancellationPolicyIsNonRefundable { get; set; }

        [DisplayName("Refundable Percentage")]
        public decimal RefundablePercentage { get; set; }

        [DisplayName("Refundable Befor Days")]
        public long RefundableBeforDays { get; set; }

        [DisplayName("Markup Percentage")]
        public decimal MarkupPercentage { get; set; }

        [DisplayName("First Name")]
        public string GuestFirstName { get; set; }

        [DisplayName("Last Name")]
        public string GuestLastName { get; set; }

        [DisplayName("Is Male")]
        public bool IsMale { get; set; }

        [DisplayName("Address")]
        public string GuestAddress { get; set; }

        [DisplayName("City")]
        public string GuestCity { get; set; }

        [DisplayName("Country Id")]
        public long GuestCountryId { get; set; }

        [DisplayName("Phone Number")]
        public string GuestPhone { get; set; }

        [DisplayName("Mobile Number")]
        public string GuestMobile { get; set; }

        [DisplayName("Email")]
        public string GuestEmail { get; set; }

        [DisplayName("Status")]
        public long Status { get; set; }

        [DisplayName("Status")]
        public String StatusName {
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

        [DisplayName("Type")]
        public int Type { get; set; }

        public String TypeName
        {
            get
            {
                try
                {
                    return Helper.GetEnumDescription((RoomType)Type);
                }
                catch (Exception)
                { return ""; }
            }
        }
        [DisplayName("Room Name")]
        public string Name { get; set; }

        public bool IsSoloBooking { get; set; }

         [DisplayName("Exclusive/Solo")]
        public string ExclusiveOrSolo
        {
            get
            {
                return IsSoloBooking == true ? "Solo" : "Exclusive";
            }
        }


        public DateTime? UpdateDate { get; set; }

        public string CreatedDateFrom { get; set; }

        public string CreatedDateTo { get; set; }

        [MapIgnore]
        public bool IsChecked { get; set; }

        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [MapIgnore]
        public decimal RefundAmount { get; set; }

        [MapIgnore]
        public bool IsApplyCancellationPolicy { get; set; }

        [DisplayName("Discount ")]
        public string DiscountName { get; set; }

        [DisplayName("Early Bird Discount")]
        public string EBDName { get; set; }

         [DisplayName("Cancellation Reason")]
        public string CancellationReason { get; set; }

         //private string _OTA;
         //public string OTA { get { return !string.IsNullOrEmpty(_OTA) ? _OTA : "Himalayan"; } set { _OTA = value; } }

         [DisplayName("OTA")]
         public string OTA { get; set; }


        public List<InventoryMaster.InventoryMaster> inventoryList { get; set; }
    }
}
