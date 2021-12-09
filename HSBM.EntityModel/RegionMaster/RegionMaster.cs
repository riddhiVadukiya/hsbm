namespace HSBM.EntityModel.RegionMaster
{

    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using System.Collections.Generic;
    using HSBM.EntityModel.CityMaster;
    using HSBM.EntityModel.CountryMaster;
    using System.ComponentModel.DataAnnotations;

    public class RegionMaster : EntityBase
    {
        [PrimaryKey, NonUpdatable]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Country")]
        public long CountryMasterId { get; set; }

        [Required]
        [Display(Name = "Region Name")]
        public string RegionName { get; set; }

        public string Code { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [RegularExpression(@"^[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?$", ErrorMessage = "Invalid value")]
        public string Latitude { get; set; }

        [RegularExpression(@"^[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?$", ErrorMessage = "Invalid value")]
        public string Longitude { get; set; }

        [MapIgnore]
        public virtual CountryMaster CountryMaster { get; set; }

        [MapIgnore]
        public virtual List<CityMaster> CityMaster { get; set; }

    }

}