namespace HSBM.EntityModel.RegionMaster
{

    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using BLToolkit.Validation;
    using System.Collections.Generic;

    public class RegionMasterRequest
    {
        public long Id { get; set; }

        public long CountryMasterId { get; set; }

        public string RegionName { get; set; }

        public string CountryName { get; set; }

        public string Code { get; set; }

        public bool IncludeIsDeleted { get; set; }

    }

}