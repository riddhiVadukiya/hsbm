namespace HSBM.EntityModel.CityMaster
{
    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using BLToolkit.Validation;
    using System.Collections.Generic;

    public class CityMasterRequest
    {
        public long Id { get; set; }

        public long CountryMasterId { get; set; }

        public long? RegionMasterId { get; set; }

        public string CityName { get; set; }

        public string RegionName { get; set; }

        public string CountryName { get; set; }

        public string Code { get; set; }

        public bool IncludeIsDeleted { get; set; }

    }
}