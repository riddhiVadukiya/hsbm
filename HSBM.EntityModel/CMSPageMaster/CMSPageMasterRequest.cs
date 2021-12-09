namespace HSBM.EntityModel.CMSPageMaster
{

    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using BLToolkit.Validation;
    using System.Collections.Generic;

    public class CMSPageMasterRequest
    {
        public long Id { get; set; }

        public int CMSPageId { get; set; }

        public bool IncludeIsDeleted { get; set; }
    }

}