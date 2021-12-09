namespace HSBM.EntityModel.BannerMaster
{

    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using BLToolkit.Validation;
    using System.Collections.Generic;

    public class BannerMasterRequest
    {
        public long Id { get; set; }

        public bool IncludeIsDeleted { get; set; }
    }
}