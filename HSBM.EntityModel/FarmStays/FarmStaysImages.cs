using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.FarmStays
{
    public class FarmStaysImages
    {
        public long Id { get; set; } //(bigint, not null)
        public long FarmStaysId { get; set; } //(bigint, not null)
        public string ImageName { get; set; } //(nvarchar(max), not null)
        public bool IsDeleted { get; set; }
    }
}
