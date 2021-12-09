namespace HSBM.EntityModel.RoleMasterDetails
{
    using System;

    public class RoleMasterDetailsRequest
    {
        public long Id { get; set; }

        public long RoleMasterID { get; set; }

        public long RoleModuleID { get; set; }

        public bool CanAdd { get; set; }

        public bool CanUpdate { get; set; }

        public bool CanDelete { get; set; }

        public bool CanView { get; set; }

    }

}
