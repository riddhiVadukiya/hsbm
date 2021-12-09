namespace HSBM.EntityModel.FrontEnd
{
    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using HSBM.Common.Utils;
    using System.Globalization;

    public class SearchFarmStaysResult
    {
        public List<SearchFarmStays> SearchFarmStays { get; set; }
        public List<PriceFilter> PriceFilter { get; set; }
        public List<AmenityFilter> AmenityFilter { get; set; }
        public List<AmenityFilter> CategoryFilter { get; set; }
        public List<RatingsFilter> RatingsFilter { get; set; }
    }

    public class SearchFarmStays
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Bedrooms { get; set; }

        public string Image { get; set; }

        public bool IsNonRefundable { get; set; }

        public decimal Price { get; set; }

        public decimal NetPrice { get; set; }

        public string AvailableFor { get; set; }

        public decimal EBODiscountValueInPercentage { get; set; }

        //date
        private string _EBOBookbyDate;
        public string EBOBookbyDate { get { return string.IsNullOrEmpty(_EBOBookbyDate) ? null : Convert.ToDateTime(_EBOBookbyDate).ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture); } set { _EBOBookbyDate = value; } }

        public string DiscountName { get; set; }

        public bool DiscountIsPercentage { get; set; }

        public decimal DiscountValue { get; set; }


        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public int Ratings { get; set; }

        public string DealApplied
        {
            get
            {
                string strDiscount = string.Empty;
                if (EBODiscountValueInPercentage > 0)
                {
                    strDiscount += "Early bird discount " + EBODiscountValueInPercentage + "% off";
                }
                if (DiscountValue > 0)
                {
                    strDiscount += (strDiscount.Length > 0 ? " And " : "");
                    strDiscount += DiscountName + " " + (DiscountIsPercentage ? DiscountValue + "% " : Helper.GetCurrentCurrencySymbol() +" "+ DiscountValue + " FLAT ") + "off";
                }

                return strDiscount;
            }
        }

        public int? OrderIndex { get; set; }
        public int Bathrooms { get; set; }
        public string USPTags { get; set; } 
        public string Persuasions { get; set; } 
        public string LanguagesSpoken { get; set; }
        public string TypeofFood { get; set; }
        public string Location { get; set; }
        public string Neighborhood { get; set; }
        
    }


    public class PriceFilter
    {
        public string Name { get; set; }
        public int StartPrice { get; set; }
        public int EndPrice { get; set; }
        public int Count { get; set; }
    }
    public class AmenityFilter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public string FamrstaysId { get; set; }
    }
    public class RatingsFilter
    {
        public string Name { get; set; }
        public int Rating { get; set; }        
        public int Count { get; set; }
        public string FullName { get; set; }
    }
    public class SearchFarmStaysRequest
    {
        public string CheckIn { get; set; }

        public string CheckOut { get; set; }

        public int Guests { get; set; }

        public int Child { get; set; }

        public bool IsSolo { get; set; }

        public long FarmStayId { get; set; }

        public string CurrencyCode { get; set; }
        
        public long RoomId { get; set; }

        public bool IsPackage { get; set; }

        public int RatePlanId { get; set; }

    }
}
