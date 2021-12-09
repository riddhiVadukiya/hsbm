using HSBM.Common.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.AxisRooms
{
    public class AxisRoom
    {
        public string accessKey { get; set; }

        public string channelId { get; set; }

        public List<Hotel> hotels { get; set; }
    }

    public class Hotel
    {
        public string hotelId { get; set; }

        public List<Room> rooms { get; set; }
    }

    public class Room
    {
        public string roomId{get;set;}
        public List<AxisRatePlan> rateplans { get; set; }
    }

    public class AxisRatePlan
    {
        public string rateplanId { get; set; }
        public List<PriceDetails> priceDetails { get; set; }
    }

    public class PriceDetails
    {
        public string date { get; set; }
        public string endDate { get; set; }
        public string startDate { get; set; }
        public Price price { get; set; }
    }

    public class Price
    {
        public decimal Single { get; set; }
        public decimal Double { get; set; }
        public decimal Triple { get; set; }
        [JsonProperty(PropertyName = "Extra Bed")]
        
        public decimal ExtraBed { get; set; }
        [JsonProperty("Extra Child")]
        public decimal ExtraChild { get; set; }
        [JsonProperty(PropertyName = "Extra Adult")]
        public decimal ExtraAdult { get; set; }
    }
}
