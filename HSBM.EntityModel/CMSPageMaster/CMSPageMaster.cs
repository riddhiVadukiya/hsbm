
namespace HSBM.EntityModel.CMSPageMaster
{
    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CMSPageMaster
    {
        public long Id { get; set; }

        public int CMSPageId { get; set; }

        [Required(ErrorMessage = "Page Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Page Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Page Description is required.")]
        public string Description { get; set; }

        [Display(Name = "Meta Title")]
        public string MetaTitle { get; set; }

        [Display(Name = "Meta Keywords")]
        public string MetaKeywords { get; set; }

        [Display(Name = "Meta Description")]
        public string MetaDescription { get; set; }

        public DateTime CreatedDate { get; set; }

        public long CreatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public long? UpdatedBy { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

    }

    public class CMSPagesForLink
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Link { get; set; }
    }

}