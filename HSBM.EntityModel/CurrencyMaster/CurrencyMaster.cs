namespace HSBM.EntityModel.CurrencyMaster
{
    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CurrencyMaster
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Currency Name")]
        public string CurrencyName { get; set; }

        [Display(Name = "Currency Code")]
        public string CurrencyCode { get; set; }

        public bool IsBaseCurrency { get; set; }

        public bool IsSelected { get; set; }

        [Required]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid value")]
        public decimal Value { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public long CreatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public long? UpdatedBy { get; set; }

    }

}