using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.RegionMaster
{
    public class RegionMasterResponse : RegionMaster
    {
        public int RecordsTotal { get; set; }
                public string CountryName { get; set; }
    }
}
