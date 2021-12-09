
using HSBM.Common.Utils;
using HSBM.EntityModel.CityMaster;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.CountryMaster;
using HSBM.EntityModel.RegionMaster;
using HSBM.Service.ServiceContext;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
namespace HSBM.Service.Contracts
{
    public interface ILocationsService
    {
        //GridDataResponse GetAllCountriesBySearchRequest(GridParams p_GridParams, CountryMasterRequest p_SearchRequest);

        List<CountryMaster> GetAllCountryForGrid(out int TotalRecords, string GlobalSearch, int LimitOffset, int LimitRowcount, string SortColumn, string SortDirection);

        List<CountryMaster> GetAllLocations(CountryMasterRequest p_SearchRequest);

        List<CountryMaster> GetAllCountry();

        CountryMaster GetCountryById(long Id);

        long AddorUpdateCountry(CountryMaster countryMaster);

        void DeleteCountry(long Id);

        GridDataResponse GetAllRegionBySearchRequest(GridParams p_GridParams, RegionMasterRequest p_SearchRequest);

        List<RegionMaster> GetAllRegion();

        RegionMaster GetRegionById(long Id);

        long AddUpdateRegion(RegionMaster regionMaster);

        void DeleteRegion(long Id);

        GridDataResponse GetAllCityBySearchRequest(GridParams p_GridParams, CityMasterRequest p_SearchRequest);

        List<CityMaster> GetAllCity();

        CityMaster GetCityById(long Id);

        long AddUpdateCity(CityMaster cityMaster);

        void DeleteCity(long Id);

        List<SelectListItem> CountryDropDown();

        List<SelectListItem> RegionDropDown(long CountryId);

        List<SelectListItem> CityDropDown(long RegionId);

        long ActiveAndInactiveSwitchForCountryUpdate(CountryMaster countryMaster);

        long ActiveAndInactiveSwitchForRegionUpdate(RegionMaster regionMaster);

        long ActiveAndInactiveSwitchForCityUpdate(CityMaster cityMaster);

        bool ImportCountryDocument(RequestResponseServiceContext WebRequestResponseServiceContext, Stream stream);

        bool ImportRegionDocument(RequestResponseServiceContext WebRequestResponseServiceContext, Stream stream);

        List<SelectListItem> CityByCountryDropDown(long CountryId);
    }
}