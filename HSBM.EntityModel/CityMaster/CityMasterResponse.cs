using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.CityMaster
{
    public class CityMasterResponse : CityMaster
    {
        public int RecordsTotal { get; set; }

        public string CountryName { get; set; }

        public string RegionName { get; set; }
    }
}
