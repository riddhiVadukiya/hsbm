namespace HSBM.EntityModel.EmailTemplates
{

    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using BLToolkit.Validation;
    using System.Collections.Generic;

    public class EmailTemplatesRequest
    {
        public bool IncludeIsDeleted { get; set; }
    }

}