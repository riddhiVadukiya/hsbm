using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.Front
{
    public class FarmStaysDiscountResponse
    {
        public string Name { get; set; }
        public int IsPercentage { get; set; }
        public int IsEBO { get; set; }
        //public DateTime BookbyDate { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal DiscountAmount { get; set; }

        public string ImageName { get; set; }
        public string FarmStayName { get; set; }

        public decimal OrigionalPrice { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal DiscountedPrice { get; set; }

        public string CheckInDate { get; set; }
        public string CheckOutDate { get; set; }
        public int NoOfGuest { get; set; }
        public int IsSolo { get; set; }
        public int Id { get; set; }


        public int DaysBeforeBooking { get; set; }
    }
}
