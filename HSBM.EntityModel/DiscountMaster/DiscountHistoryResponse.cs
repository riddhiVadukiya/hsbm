namespace HSBM.EntityModel.DiscountMaster
{
    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using BLToolkit.Validation;
    using System.Collections.Generic;

    public class DiscountHistoryResponse
    {
        public string FarmsName { get; set; }
        public string FarmId { get; set; }
        public bool IsApplied { get; set; }

        public string CategoryId { get; set; }
    }
}