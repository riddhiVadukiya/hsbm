namespace HSBM.EntityModel.SystemUsers
{
    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using BLToolkit.Validation;
    using System.Collections.Generic;
    using HSBM.EntityModel.RoleMasterDetails;
    using HSBM.EntityModel.RoleMaster;
    using System.ComponentModel.DataAnnotations;

    public class LoginUser : EntityBase
    {
        [Display(Name = "User Name")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage="Please enter email.")]
        public string UserName { get; set; }

        [DisplayName("Password")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Please enter password.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage="Password length should be 6 to 20 characters")]
        public string Password { get; set; }
                
        [DisplayName("RememberMe")]
        public bool RememberMe { get; set; }

        [MapIgnore]
        public int UserType { get; set; }

        [DisplayName("IsSocialMedia")]
        public bool IsSocialMedia { get; set; }

    }
}
