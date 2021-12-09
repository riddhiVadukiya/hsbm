namespace HSBM.EntityModel.CategoryMaster
{
    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class CategoryMaster
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Name is required.")]
        [DisplayName("Category Name")]
        [StringLength(20, ErrorMessage = "Maximum {1} characters are allowed.")]
        public string CategoryName { get; set; }

        public string ImageUrl { get; set; }
        public HttpPostedFileBase CategoryImage { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public long CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public long? UpdatedBy { get; set; }
        
    }
}
