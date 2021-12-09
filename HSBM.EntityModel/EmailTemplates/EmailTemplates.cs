namespace HSBM.EntityModel.EmailTemplates
{
    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class EmailTemplates
    {
        public long Id { get; set; }

        [Display(Name = "Template Type")]
        public long TemplateType { get; set; }

        //[Required]
        public string Name { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Templates Html")]
        public string TemplatesHtml { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public long CreatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public long? UpdatedBy { get; set; }

    }

}