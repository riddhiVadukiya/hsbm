namespace HSBM.EntityModel.CategoryMaster
{

    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using BLToolkit.Validation;
    using System.Collections.Generic;

    public class CategoryMasterResponse : CategoryMaster
    {
        public int RecordsTotal { get; set; }
    }

}