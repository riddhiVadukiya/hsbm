namespace HSBM.EntityModel.AccountSummary
{
    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using BLToolkit.Validation;
    using System.Collections.Generic;

    public class AccountSummaryRequest
    {
        public bool IsAdminLogin { get; set; }

        public int SystemUserId { get; set; }



    }

}