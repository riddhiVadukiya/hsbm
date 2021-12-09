using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.FarmStays
{
    public class FarmStaysAmenities
    {
        public long Id { get; set; } //(bigint, not null)
        public long FarmStaysId { get; set; } //(bigint, not null)
        public long AmenityId { get; set; }
    }
}
