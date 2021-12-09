using HSBM.Common.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.FarmStays
{
    public class SeasonListResponse
    {
        public int RoomId { get; set; }

        public DateTime SeasonStartDate { get; set; }

        public DateTime SeasonEndDate { get; set; }

        public List<RatePlan> ListOfRatePlan { get; set; }
    }
}
