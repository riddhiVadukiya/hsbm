using HSBM.Common.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.AxisRooms
{
    public class AxisBookingRequest
    {
        public List<AxisBookingResponse> AxisBookingList { get; set; }
        
    }
    public class AxisBookingResponse
    {
        public BookingDetails BookingDetails { get; set; }
        public CheckinDetails CheckinDetails { get; set; }
        public GuestDetails GuestDetails { get; set; }
        public Rates Rates { get; set; }
        public string accessKey { get; set; }
    }
    public class BookingDetails
    {
        public string bookedBy { get; set; }
        public string bookingDateTime { get; set; }
        public string bookingNo { get; set; }
        public string bookingSource { get; set; }
        public string bookingSourceRefId { get; set; }
        public string bookingStatus { get; set; }
        public string hotelId { get; set; }
        public string ota { get; set; }
        public string otaRefId { get; set; }
    }

    public class CheckinDetails
    {
        public string amountToBeCollected { get; set; }
        public string checkInDate { get; set; }
        public string checkOutDate { get; set; }
        public string children { get; set; }
        public bool isDayWisePrice { get; set; }
        public bool isGeniusBooker { get; set; }
        public string paid { get; set; }
        public List<object> specialRequest { get; set; }
        public string supplierAmount { get; set; }
        public string taxes { get; set; }
        public string totalAmount { get; set; }
        public string totalPax { get; set; }
    }

    public class GuestDetails
    {
        public string countryCode { get; set; }
        public string emailId { get; set; }
        public string guestName { get; set; }
        public string mobileNo { get; set; }
        public string title { get; set; }
    }

    public class DayWiseDetail
    {
        public string date { get; set; }
        public string deals { get; set; }
        public string rate { get; set; }
    }

    public class RoomType
    {
        public string cityTax { get; set; }
        public List<DayWiseDetail> dayWiseDetails { get; set; }
        public string id { get; set; }
        public string noOfRooms { get; set; }
        public string ratePlanId { get; set; }
        public string ratePlanName { get; set; }
        public string serviceCharge { get; set; }
        public string totalAdults { get; set; }
        public string vat { get; set; }
    }

    public class Rates
    {
        public List<RoomType> roomType { get; set; }
    }
}
