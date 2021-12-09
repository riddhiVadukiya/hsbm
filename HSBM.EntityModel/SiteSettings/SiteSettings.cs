namespace HSBM.EntityModel.SiteSettings
{

    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class SiteSettings
    {
        public long Id { get; set; }

        [MapIgnore]
        public string Name { get; set; }

        public int SiteSettingId { get; set; }

        [Required]
        public string Value { get; set; }

        public DateTime CreatedDate { get; set; }

        public long CreatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public long? UpdatedBy { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [MapIgnore]
        public IEnumerable<SelectListItem> CurrencyList { get; set; }

    }

}