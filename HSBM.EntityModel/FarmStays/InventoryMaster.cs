using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.FarmStays
{
    public class InventoryMaster
    {
        public long Id { get; set; } //(bigint, not null)
        public long FarmstaysId { get; set; } //(bigint, not null)
        public DateTime BookingDate { get; set; } //(date, not null)
        public int People { get; set; } //(int, not null)
        public bool OnSite { get; set; } //(bit, not null)
        public long OrderId { get; set; } //(bigint, null)
        public Guid BookingGroupId { get; set; } //(uniqueidentifier, null)
    }
}
