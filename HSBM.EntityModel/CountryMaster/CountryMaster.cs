namespace HSBM.EntityModel.CountryMaster
{
    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using System.Collections.Generic;
    using HSBM.EntityModel.CityMaster;
    using HSBM.EntityModel.RegionMaster;
    using System.ComponentModel.DataAnnotations;

    public class CountryMaster : EntityBase
    {
        [PrimaryKey, NonUpdatable]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Country Name")]
        public string CountryName { get; set; }

        public string Code { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [RegularExpression(@"^[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?$", ErrorMessage = "Invalid value")]
        public string Latitude { get; set; }

        [RegularExpression(@"^[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?$", ErrorMessage = "Invalid value")]
        public string Longitude { get; set; }

        [MapIgnore]
        public virtual List<RegionMaster> RegionMaster { get; set; }

        [MapIgnore]
        public virtual List<CityMaster> CityMaster { get; set; }
    }

}