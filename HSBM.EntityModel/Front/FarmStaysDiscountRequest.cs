using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.Front
{
    public class FarmStaysDiscountRequest
    {
        public Int64 FarmStaysId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal Price { get; set; }
        public decimal Flag { get; set; }
        public int NoOfGuest { get; set; }
        public int IsSolo { get; set; }  
    }
}
