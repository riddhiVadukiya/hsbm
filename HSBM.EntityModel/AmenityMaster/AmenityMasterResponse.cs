namespace HSBM.EntityModel.AmenityMaster
{

    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using BLToolkit.Validation;
    using System.Collections.Generic;

    public class AmenityMasterResponse : AmenityMaster
    {
        public int RecordsTotal { get; set; }
    }

}