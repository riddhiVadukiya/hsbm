namespace HSBM.EntityModel.BannerMaster
{
    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class BannerMaster
    {
        public long Id { get; set; }

        public string ImageName { get; set; }

        public string ImageOrignalName { get; set; }

        public string Alt { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        [Display(Name = "Order Index")]
        public long OrderIndex { get; set; }

        public DateTime CreatedDate { get; set; }

        public long CreatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public long? UpdatedBy { get; set; }

        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }

    }

}