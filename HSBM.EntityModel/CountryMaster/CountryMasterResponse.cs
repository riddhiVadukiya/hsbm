namespace HSBM.EntityModel.CountryMaster
{

    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using BLToolkit.Validation;
    using System.Collections.Generic;

    public class CountryMasterResponse : CountryMaster
    {
        public int RecordsTotal { get; set; }
    }

}