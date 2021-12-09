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

    public class SystemUsers : EntityBase
    {
        [PrimaryKey, NonUpdatable]
        public long Id { get; set; }

        public long ParentId { get; set; }

        public long UserType { get; set; }

        [Display(Name = "Role")]
        [System.ComponentModel.DataAnnotations.Required]
        public long RoleMasterID { get; set; }
        
        [Display(Name = "Username")]
        //[System.ComponentModel.DataAnnotations.Required]
        public string UserName { get; set; }
        
        [System.ComponentModel.DataAnnotations.Required]
        //[System.ComponentModel.DataAnnotations.MaxLength(20, ErrorMessage="Maximum length is 20 characters")]
        //[System.ComponentModel.DataAnnotations.MinLength(6, ErrorMessage = "Minimum length is 6 characters")]
        [StringLength(20, MinimumLength = 6, ErrorMessage="Password length should be 6 to 20 characters")]
        public string Password { get; set; }
        
        [Display(Name = "First Name")]
        [System.ComponentModel.DataAnnotations.Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [StringLength(50, ErrorMessage = "Maximum {1} characters are allowed.")]
        public string FirstName { get; set; }
        
        [Display(Name = "Last Name")]
        [System.ComponentModel.DataAnnotations.Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [StringLength(50, ErrorMessage = "Maximum {1} characters are allowed.")]
        public string LastName { get; set; }
        
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        
        [Display(Name = "Email")]
        [System.ComponentModel.DataAnnotations.Required]
        //[RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,50}|[0-9]{1,50})(\]?)$", ErrorMessage = "Please enter valid email")]
        [EmailAddress(ErrorMessage="Please enter valid email")]
        public string Email { get; set; }
        
        //[RegularExpression(@"^[0-9]+$", ErrorMessage = "Please enter valid Telephone number")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]

        [StringLength(15, MinimumLength = 10, ErrorMessage = "Telephone number must be between {2} to {1} characters.")]        
        //[RegularExpression(@"^[0-9]+$", ErrorMessage = "Please enter valid Telephone number")]
        [RegularExpression(@"^[\+\d]?(?:[\d-.\s()]*)$", ErrorMessage = "Please enter valid Telephone number")]        
        public string Telephone { get; set; }

        [StringLength(15, MinimumLength = 10, ErrorMessage = "Mobile number must be between {2} to {1} characters.")]
        //[RegularExpression(@"^[0-9]+$", ErrorMessage = "Please enter valid Mobile number")]
        [RegularExpression(@"^[\+\d]?(?:[\d-.\s()]*)$", ErrorMessage = "Please enter valid Mobile number")]            
        //[RegularExpression(@"^[0-9@\!#\$\^%&*()+=\-[]\\\';,\.\/\{\}\|\:<>\? ]+$/;", ErrorMessage = "Please enter valid Mobile number")]                        
        //[System.ComponentModel.DataAnnotations.MinLength(6, ErrorMessage = "Minimum length is 10 characters")]
        public string Mobile { get; set; }

        public string Gender { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public Nullable<long> CountryMasterID { get; set; }
        public Nullable<long> RegionMasterID { get; set; }
        public Nullable<long> CityMasterID { get; set; }
        public Nullable<DateTime> LastLoginDate { get; set; }
        public Nullable<long> PhotoAlbumID { get; set; }
        public string ImageUrl { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        
        [Display(Name = "Base Currency")]
        public long BaseCurrency { get; set; }

        [MapIgnore]
        public string ConnectionId { get; set; }

        [MapIgnore]
        public string CountryName { get; set; }

        [MapIgnore]
        public string CityName { get; set; }

        [MapIgnore]
        public string RegionName { get; set; }


        [MapIgnore]
        [Display(Name = "Full Name")]
        public string FullName { get { return FirstName + " " + LastName; } set { value = FirstName + " " + LastName; } }

        [MapIgnore]
        [Display(Name = "Full Address")]
        //public string FullAddress { get { return Address + ", " + Address2; } set { value = Address + ", " + Address2; } }
        public string FullAddress { get { return Address ; } set { value = Address; } }

        [MapIgnore]
        public virtual List<RoleMasterDetails> RoleMasterDetails { get; set; }

        [MapIgnore]
        public long SupplierId { get; set; }

        [MapIgnore]
        public int SupplierType { get; set; }

        public bool IsVerify { get; set; }

        public bool IsSocialMedia { get; set; }
        
    }
}
