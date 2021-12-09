using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.HomeMaster
{
    public class NewCustomersResponse
    {
        public string Month { get; set; }

        public string Year { get; set; }

        public int TotalCount { get; set; }

        public string MonthYear { get; set; }        
        
    }
}
