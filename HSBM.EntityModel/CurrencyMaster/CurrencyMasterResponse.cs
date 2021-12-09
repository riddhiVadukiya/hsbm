namespace HSBM.EntityModel.CurrencyMaster
{

    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using BLToolkit.Validation;
    using System.Collections.Generic;

    public class CurrencyMasterResponse : CurrencyMaster
    {        
        public int RecordsTotal { get; set; }
    }

}