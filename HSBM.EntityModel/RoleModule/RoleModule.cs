namespace HSBM.EntityModel.RoleModule
{
    using BLToolkit.DataAccess;
    using System;

    public class RoleModule : EntityBase
    {
        [PrimaryKey, NonUpdatable]
        public long Id { get; set; }

        public string ModuleName { get; set; }

        public bool IsActive { get; set; }

    }
}
