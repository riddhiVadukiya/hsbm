using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.FrontReview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.Front
{
    public class FarmStaysDetail
    {
        public string Name { get; set; }        
        public string Description { get; set; } 
        public int Bedrooms { get; set; }       
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }       
        public string Address { get; set; }
        public string HouseRules { get; set; }
        public string StayPolicy { get; set; }     
        public decimal MarkupPercentage { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public List<ImageDto> FarmStaysImages { get; set; }
       // public List<DiscountDto> FarmStaysDiscount { get; set; }
        public List<string> FarmStaysAmenities { get; set; }
        public string FarmStayDiscount { get; set; }

        public string CheckIn { get; set; }

        public string CheckOut { get; set; }

        public int Guests { get; set; }

        public int Child { get; set; }

        public bool IsSolo { get; set; }

        public long FarmStayId { get; set; }
        public int Bathrooms { get; set; }
        public string USPTags { get; set; }
        public string Persuasions { get; set; }
        public string LanguagesSpoken { get; set; }
        public string TypeofFood { get; set; }
        public string Location { get; set; }

        public List<FarmStaysRatingsAndReviewResponse> RatingAndReview { get; set; }
    }

    public class RoomDetails
    {
        public List<RoomDetail> ListofRoom { get; set; }
        public string FarmStayDiscount { get; set; }
    }
    public class RoomDetail
    {
        public int RoomId { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int MaxPerson { get; set; }
        public int NoOfAdults { get; set; }
        public int NoOfChild { get; set; }
        public string TypeName
        {
            get
            {
                return Helper.GetEnumDescription((RoomType)Type);
            }
        }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public string AvailableFor { get; set; }
        public bool IsShared { get; set; }
        public int Days { get; set; }
        public string BookingURL { get; set; }
        public List<RatePlan> ListOfRatePlan { get; set; }

        public int RatePlanId { get; set; }
        public string RatePlanName
        {
            get
            {
                return RatePlanId>0 ? Helper.GetEnumDescription((RatePlanEnum)RatePlanId) : string.Empty;
            }
        }
    }
    public class RatePlan
    {
        public int RatePlanId { get; set; }
        public string RatePlanName
        {
            get
            {
                return Helper.GetEnumDescription((RatePlanEnum)RatePlanId);
            }
        }
    }
}
