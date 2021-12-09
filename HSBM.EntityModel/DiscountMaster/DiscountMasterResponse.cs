namespace HSBM.EntityModel.DiscountMaster
{
    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using BLToolkit.Validation;
    using System.Collections.Generic;

    public class DiscountMasterResponse : DiscountMaster
    {
        public int RecordsTotal { get; set; }
    }

}