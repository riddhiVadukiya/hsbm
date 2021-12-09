using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.Front
{
    public class DiscountDto
    {
        public string Name { get; set; }
        public bool IsPercentage { get; set; }
        public bool IsEBO { get; set; }
        //public DateTime BookbyDate { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal DiscountAmount { get; set; }
        public int DaysBeforeBooking { get; set; }
    }
}
