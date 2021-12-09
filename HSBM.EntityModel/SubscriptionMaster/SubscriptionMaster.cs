namespace HSBM.EntityModel.SubscriptionMaster
{
    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using HSBM.Common.Utils;

    public class SubscriptionMaster
    {
        public long Id { get; set; }

        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,50}|[0-9]{1,50})(\]?)$", ErrorMessage = "Please enter valid email")]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        [MapIgnore]
        public string strCreatedDate { get { try { return CreatedDate.ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture); } catch (Exception) { return ""; } } }

        public DateTime? UpdateDate { get; set; }

        public string CreatedDateFrom { get; set; }

        public string CreatedDateTo { get; set; }

        [MapIgnore]
        public bool IsChecked { get; set; }

    }

}