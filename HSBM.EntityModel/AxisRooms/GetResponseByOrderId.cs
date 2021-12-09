using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.AxisRooms
{
    public class GetResponseByOrderId
    {
        public int Id { get; set; }
        public int Farmstaysid { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int RoomId { get; set; }
    }
}
