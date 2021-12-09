
namespace HSBM.EntityModel.RoleMaster
{
    using System;

    public class RoleMasterRequest
    {
        public long Id { get; set; }

        public string RoleName { get; set; }

        public bool Isdefault { get; set; }

        public bool IncludeIsDeleted { get; set; }

        public bool IsAdmin { get; set; }
    }
}
