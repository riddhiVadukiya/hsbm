using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.FarmStays
{
    public class RatePlan
    {
        public string RatePlanName { get; set; }

        public List<Plans> ListOfPlans { get; set; }
    }
}
