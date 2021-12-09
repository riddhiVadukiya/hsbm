using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.HomeMaster
{
    public class FarmStaySummary
    {       
        public string Year { get; set; }
        public string Month { get; set; }
        public string MonthYear{get;set;}
        public decimal TotalEarning { get; set; }
        public decimal TotalComplete { get; set; }
        public decimal TotalCancel { get; set; }
        public int Cancel { get; set; }
    }
}
