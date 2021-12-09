using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.FrontEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HSBM.EntityModel.Front
{
    public class FarmStaysBookingDetail : SearchFarmStaysRequest
    {

        public string FarmStayName { get; set; }
        public string Address { get; set; } 
        public string Name { get; set; }
        public int Type { get; set; }
        public int MaxPerson { get; set; }
        public string TypeName  {  get { return Helper.GetEnumDescription((RoomType)Type);}  }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public bool? IsMale { get; set; }
        public bool CancellationPolicyIsNonRefundable { get; set; } //(bit, not null)
        public decimal RefundablePercentage { get; set; } //(decimal(3,2), null)
        public int RefundableBeforDays { get; set; } //(int, null)
        public decimal MarkupPercentage { get; set; }
        public decimal EBDAmount{ get; set; }
        public string EBDName{ get; set; }
        public decimal DiscountAmount{ get; set; }
        public string DiscountName { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public string FarmStayDiscount { get; set; }
        public string TermsAndConditions { get; set; }
        public bool IsApplyCancellationPolicy { get; set; }
        public string RatePlanName { get; set; }
        //public int RatePlanId { get; set; }
    }

    public class BookingResponse : FarmStaysBookingDetail
    {
        public LeadTraveler LeadTraveler { get; set; }
        public bool ExtraBed { get; set; }
        public string RatePlanName { get; set; }

        public string PaymentKey { get { return Helper.PaymentKey(); } }
        public string PaymentSalt { get { return Helper.PaymentSalt(); } }
    }
    public class LeadTraveler
    {
        public string GuestFirstName { get; set; }
        public string GuestLastName { get; set; }
        public string GuestEmail { get; set; }
        public bool? IsMale { get; set; }
        public int GuestCountryId { get; set; }
        public string GuestCity { get; set; }
        public string GuestMobile { get; set; }
        public string GuestAddress { get; set; }
        public long? CustomerId { get; set; }
    }
    public class HashKeyResponse
    {
        public string key { get; set; }
        public string txnid { get; set; }
        public string amount { get; set; }
        public string pinfo { get; set; }
        public string fname { get; set; }
        public string email { get; set; }
        public string udf5 { get; set; }
        public string salt { get; set; }
    }

    public class BookingPaymentResponse : FarmStaysBookingDetail
    {
        public string GuestFirstName { get; set; }
        public string GuestEmail { get; set; }
        public string Status { get; set; }
        public int OrderID { get; set; }
        public string PayuMoneyId { get; set; }
        public string PaymentResponse { get; set; }
    }
}
