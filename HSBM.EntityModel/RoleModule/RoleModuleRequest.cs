namespace HSBM.EntityModel.RoleModule
{
    using System;

    public class RoleModuleRequest
    {
        public long Id { get; set; }

        public string ModuleName { get; set; }

        public bool IncludeIsDeleted { get; set; }

    }
}
