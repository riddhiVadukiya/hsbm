using HSBM.Common.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.AxisRooms
{
    public class AxisRoomInventoryRequest
    {
        public string accessKey { get; set; }

        public string channelId { get; set; }

        public List<InventoryHotel> hotels { get; set; }
    }
    public class InventoryHotel
    {
        public string hotelId { get; set; }

        public List<InventoryRoom> rooms { get; set; }
    }

    public class InventoryRoom
    {
        public string roomId { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public List<AvailabilityArray> availability { get; set; }
    }

    public class AvailabilityArray
    {
        public string date { get; set; }
        public int free { get; set; }
    }
}
