namespace HSBM.EntityModel.Blogs
{
    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Blogs
    {
        public long Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Maximum {1} characters are allowed.")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Display(Name = "Meta Title")]
        public string MetaTitle { get; set; }

        [Display(Name = "Meta Keyword")]
        public string MetaKeyword { get; set; }

        [Display(Name = "Meta Description")]
        public string MetaDescription { get; set; }

        [Required]
        public string Categories { get; set; }
        
        [Required]
        public string Image { get; set; }

        [Display(Name = "Is Popular Post")]
        public bool IsPopulerPost { get; set; }

        public List<BlogCategory> CategoriesWithName { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public long CreatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public long? UpdatedBy { get; set; }

        public string Author { get; set; }

    }
}