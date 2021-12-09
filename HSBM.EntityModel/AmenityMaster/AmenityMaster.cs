namespace HSBM.EntityModel.AmenityMaster
{
    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class AmenityMaster
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Amenity Name is required.")]
        [DisplayName("Amenity Name")]
        [StringLength(20, ErrorMessage = "Maximum {1} characters are allowed.")]
        public string AmenityName { get; set; }

        [DisplayName("Image")]
        [Required(ErrorMessage = "Image is required.")]
        public string ImageUrl { get; set; }

        public HttpPostedFileBase amenityImage { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public long CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public long? UpdatedBy { get; set; }
        
    }
}
