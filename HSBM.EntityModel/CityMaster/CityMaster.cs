namespace HSBM.EntityModel.CityMaster
{

    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using System.Collections.Generic;
    using HSBM.EntityModel.RegionMaster;
    using HSBM.EntityModel.CountryMaster;
    using System.ComponentModel.DataAnnotations;
    public class CityMaster : EntityBase
    {
        [PrimaryKey, NonUpdatable]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Region")]
        public long RegionMasterId { get; set; }

        [Required]
        [Display(Name = "Country")]
        public long CountryMasterId { get; set; }

        public long RoomXMLRegionId { get; set; }        

        [Required]
        [Display(Name = "City Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "City name shuould be alphabetic")]
        public string CityName { get; set; }

        public string ImageUrl { get; set; }

        public string ImageOrignalName { get; set; }

        public string Code { get; set; }

        //[RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Invalid value")]
        [RegularExpression(@"^[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?$", ErrorMessage = "Invalid value")]
        public string Latitude { get; set; }

        //[RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Invalid value")]
        [RegularExpression(@"^[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?$", ErrorMessage = "Invalid value")]
        public string Longitude { get; set; }

        public bool IsActive { get; set; }

        public bool IsTopDestination { get; set; }

        [MapIgnore]
        public virtual CountryMaster CountryMaster { get; set; }

        [MapIgnore]
        public virtual RegionMaster RegionMaster { get; set; }

        [MapIgnore]
        public long CountryId { get; set; }

        public string RegionName { get; set; }

        public string CountryName { get; set; }

        public string HotelBedsCode { get; set; }
    }

}