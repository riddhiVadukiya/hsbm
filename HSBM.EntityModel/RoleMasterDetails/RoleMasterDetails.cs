namespace HSBM.EntityModel.RoleMasterDetails
{
    using BLToolkit.DataAccess;
    using System;
    using HSBM.EntityModel.RoleModule;
    using BLToolkit.Mapping;
    public class RoleMasterDetails : EntityBase
    {
        [PrimaryKey, NonUpdatable]
        public long Id { get; set; }

        public long RoleMasterID { get; set; }

        public long RoleModuleID { get; set; }

        public bool CanAdd { get; set; }

        public bool CanUpdate { get; set; }

        public bool CanDelete { get; set; }

        public bool CanView { get; set; }

        [MapIgnore]
        public virtual RoleModule RoleModule { get; set; }
    }

}
