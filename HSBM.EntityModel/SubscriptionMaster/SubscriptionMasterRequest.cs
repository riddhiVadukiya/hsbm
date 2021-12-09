namespace HSBM.EntityModel.SubscriptionMaster
{

    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using BLToolkit.Validation;
    using System.Collections.Generic;

    public class SubscriptionMasterRequest
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedDateFrom { get; set; }

        public string CreatedDateTo { get; set; }

        public DateTime? UpdateDate { get; set; }

    }

}