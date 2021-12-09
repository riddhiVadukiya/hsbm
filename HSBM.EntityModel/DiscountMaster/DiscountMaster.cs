namespace HSBM.EntityModel.DiscountMaster
{

    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;
    using HSBM.EntityModel.FarmStays;
    using System.Web.Mvc;
    using HSBM.Common.Utils;
    using System.Globalization;

    public class DiscountMaster : IValidatableObject
    {
        public long Id { get; set; }

        //[Required]
        [StringLength(20, ErrorMessage = "Maximum {1} characters are allowed.")]
        public string Name { get; set; }

        [Required]
        public bool IsPercentage { get; set; }

        [Required]
        public bool IsEBO { get; set; }

        public int DaysBeforeBooking { get; set; }

        //private string _BookbyDate;
        //public string BookbyDate { get { return string.IsNullOrEmpty(_BookbyDate) ? null : Convert.ToDateTime(_BookbyDate).ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture); } set { _BookbyDate = value; } }

        private string _StartDate;
        [Required]
        public string StartDate { get { return string.IsNullOrEmpty(_StartDate) ? "" : Convert.ToDateTime(_StartDate).ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture); } set { _StartDate = value; } }

        private string _EndDate;
        [Required]
        public string EndDate { get { return string.IsNullOrEmpty(_EndDate) ? "" : Convert.ToDateTime(_EndDate).ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture); } set { _EndDate = value; } }

        [Required]
        [Range(0.01, int.MaxValue, ErrorMessage = "Please enter valid Discount Value.")]
        public decimal? DiscountValue { get; set; }

        [MapIgnore]
        public List<DiscountHistoryResponse> FarmStays { get; set; }

        [MapIgnore]
        public List<SelectListItem> FarmStaysCategories { get; set; }

        [MapIgnore]
        public string SelectedFarmStays { get; set; }

        public IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsPercentage)
            {
                if (DiscountValue > 100)
                {
                    yield return new System.ComponentModel.DataAnnotations.ValidationResult("Please enter valid percentage!", new[] { "Discount Value" });
                }
            }
        }
    }

}