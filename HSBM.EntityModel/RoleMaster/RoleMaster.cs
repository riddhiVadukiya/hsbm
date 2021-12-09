
namespace HSBM.EntityModel.RoleMaster
{
    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using System.Collections.Generic;
    using HSBM.EntityModel.RoleMasterDetails;
    using System.ComponentModel.DataAnnotations;

    public class RoleMaster
    {
        public long Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Maximum {1} characters are allowed.")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public bool Isdefault { get; set; }

        public bool IsAdmin { get; set; }

        [MapIgnore]
        public virtual List<RoleMasterDetails> RoleMasterDetails { get; set; }
    }
}
