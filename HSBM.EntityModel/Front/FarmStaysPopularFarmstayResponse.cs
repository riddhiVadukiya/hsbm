using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.Front
{
    public class FarmStaysPopularFarmstayResponse
    {
        public Int64 RoomId { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }
        
        public int MaxPerson { get; set; }
        
        //public int Booked { get; set; }

        public decimal Price { get; set; }

        public decimal DiscountPrice { get; set; }   

        public string AvailableFor { get; set; }

        public string ImageName { get; set; }
        
        public string FarmStayName { get; set; }

        public string CheckInDate { get; set; }

        public string CheckOutDate { get; set; }

        public int NoOfGuest { get; set; }

        public int IsSolo { get; set; }

        public int Id { get; set; }
        
    }
}
