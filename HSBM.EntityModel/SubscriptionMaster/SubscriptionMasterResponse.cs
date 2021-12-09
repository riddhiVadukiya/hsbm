namespace HSBM.EntityModel.SubscriptionMaster
{

    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using BLToolkit.Validation;
    using System.Collections.Generic;

    public class SubscriptionMasterResponse : SubscriptionMaster
    {
        public int RecordsTotal { get; set; }
    }

}