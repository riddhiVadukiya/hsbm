
using HSBM.EntityModel.CityMaster;
using HSBM.EntityModel.CountryMaster;
using HSBM.EntityModel.RegionMaster;
using HSBM.Repository.Contracts;
using HSBM.Service.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;
using HSBM.EntityModel.Common;
using HSBM.Common.Utils;
using HSBM.Service.ServiceContext;
using System.IO;
using System;
using HSBM.Repository.Repositories;

namespace HSBM.Service.Services
{
    public class LocationsService
    {
        #region Init
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        CountryMasterRepository _CountryMasterRepository= new CountryMasterRepository();
        RegionMasterRepository _RegionMasterRepository=new RegionMasterRepository();
        CityMasterRepository _CityMasterRepository=new CityMasterRepository();
        CountryService _CountryService = new CountryService();

        //public readonly ICountryMasterRepository _CountryMasterRepository;
        //public readonly IRegionMasterRepository _RegionMasterRepository;
        //public readonly ICityMasterRepository _CityMasterRepository;

        //public LocationsService(
        //    //ICountryMasterRepository CountryMasterRepository,
        //    IRegionMasterRepository StateMasterRepository,
        //    ICityMasterRepository CityMasterRepository)
        //{
        //   // _CountryMasterRepository = CountryMasterRepository;
        //    _RegionMasterRepository = StateMasterRepository;
        //    _CityMasterRepository = CityMasterRepository;
        //}


        #endregion

        /// <summary>
        /// Get All Locations
        /// </summary>
        /// <param name="p_SearchRequest">CountryMasterRequest</param>
        /// <returns> List of CountryMaster </returns>
        public List<CountryMaster> GetAllLocations(CountryMasterRequest p_SearchRequest)
        {




            var _country = _CountryMasterRepository.GetTable().GroupJoin(_RegionMasterRepository.GetTable(),
                                 CM => CM.Id,
                                 RM => RM.CountryMasterId,
                                 (Country, Regions) => new
                                 {
                                     RegionMaster = Regions.GroupJoin(_CityMasterRepository.GetTable(),
                                     RMD => RMD.Id,
                                     CT => CT.RegionMasterId,
                                     (Region, City) => new
                                     {
                                         CityMaster = City,
                                         Id = Region.Id,
                                         CountryMasterId = Region.CountryMasterId,
                                         RegionName = Region.RegionName,
                                         Code = Region.Code,
                                         IsActive = Region.IsActive
                                     }).ToList(),
                                     Id = Country.Id,
                                     CountryName = Country.CountryName,
                                     Code = Country.Code,
                                     IsActive = Country.IsActive
                                 });


            var result = _country.ToList();

            return null;

        }

        #region Country

        //public GridDataResponse GetAllCountriesBySearchRequest(GridParams p_GridParams, CountryMasterRequest p_SearchRequest)
        //{
        //    GridDataResponse _GridDataResponse = new GridDataResponse();
        //    _CountryMasterRepository = new CountryMasterRepository();
        //    var _Country = _CountryMasterRepository.GetAllCountriesBySearchRequest(p_GridParams, p_SearchRequest);

        //    if (_Country != null && _Country.Count > 0)
        //    {
        //        _GridDataResponse.recordsTotal = _Country.FirstOrDefault().RecordsTotal;
        //        _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
        //        _GridDataResponse.data = _Country;
        //    }

        //    return _GridDataResponse;
        //}


        /// <summary>
        /// Get country grid
        /// </summary>
        /// <param name="TotalRecords">TotalRecords</param>
        /// <param name="GlobalSearch">GlobalSearch</param>
        /// <param name="LimitOffset">LimitOffset</param>
        /// <param name="LimitRowcount">LimitRowcount</param>
        /// <param name="SortColumn">SortColumn</param>
        /// <param name="SortDirection">SortDirection</param>
        /// <returns> List of CountryMaster </returns>
        public List<CountryMaster> GetAllCountryForGrid(out int TotalRecords, string GlobalSearch, int LimitOffset, int LimitRowcount, string SortColumn, string SortDirection)
        {
            TotalRecords = 100;
            var data = _CountryMasterRepository.GetTable().Skip(LimitOffset).Take(LimitRowcount);
            if (!string.IsNullOrEmpty(SortColumn))
            {
                data = data.OrderBy(SortColumn + " " + SortDirection);
            }

            return data.ToList();
        }

        //public List<CountryMaster> GetAllCountry()
        //{
        //    _CountryMasterRepository = new CountryMasterRepository();
        //    //return _CountryMasterRepository.GetTable().ToList();
        //    return _CountryMasterRepository.GetAllCountryMaster();
        //}

        //public CountryMaster GetCountryById(long Id)
        //{
        //    _CountryMasterRepository = new CountryMasterRepository();
        //    return _CountryMasterRepository.GetCountryById(Id);
        //    //return _CountryMasterRepository.SelectById(Id);
        //}

        //public long AddorUpdateCountry(CountryMaster countryMaster)
        //public bool AddorUpdateCountry(CountryMaster countryMaster)
        //{
        //    //if (countryMaster.Id > 0)
        //    //{
        //    //    return _CountryMasterRepository.Update(countryMaster);
        //    //}
        //    //else
        //    //{
        //    //    return _CountryMasterRepository.Insert(countryMaster);
        //    //}

        //    try
        //    {
        //        _CountryMasterRepository = new CountryMasterRepository();
        //        return _CountryMasterRepository.AddUpdateCountry(countryMaster);
        //    }
        //    catch (Exception _Exception)
        //    {
        //        throw _Exception;
        //    }
        //}

        //public void DeleteCountry(long Id)
        //{
        //    _CountryMasterRepository = new CountryMasterRepository();
        //    //_CountryMasterRepository.DeleteById(Id);
        //    _CountryMasterRepository.DeleteCountryById(Id);
        //}

        #endregion

        #region Region


        //public GridDataResponse GetAllRegionBySearchRequest(GridParams p_GridParams, RegionMasterRequest p_SearchRequest)
        //{
        //    GridDataResponse _GridDataResponse = new GridDataResponse();
        //    _RegionMasterRepository = new RegionMasterRepository();
        //    var _Country = _RegionMasterRepository.GetAllRegionBySearchRequest(p_GridParams, p_SearchRequest);

        //    if (_Country != null && _Country.Count > 0)
        //    {
        //        _GridDataResponse.recordsTotal = _Country.FirstOrDefault().RecordsTotal;
        //        _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
        //        _GridDataResponse.data = _Country;
        //    }

        //    return _GridDataResponse;
        //}

        //public List<RegionMaster> GetAllRegion()
        //{
        //    _RegionMasterRepository = new RegionMasterRepository();
        //    return _RegionMasterRepository.GetAllRegion();

        //    //var _Region = _RegionMasterRepository.GetTable().Join(_CountryMasterRepository.GetTable(),
        //    //    Region => Region.CountryMasterId,
        //    //    Country => Country.Id,
        //    //(Region, Country) => new RegionMaster()
        //    //{
        //    //    Id = Region.Id,
        //    //    CountryMasterId = Region.CountryMasterId,
        //    //    RegionName = Region.RegionName,
        //    //    Code = Region.Code,
        //    //    IsActive = Region.IsActive,
        //    //    CountryMaster = Country
        //    //});

        //    //return _Region.ToList();
        //}

        //public RegionMaster GetRegionById(long Id)
        //{
        //    //return _RegionMasterRepository.SelectById(Id);
        //    _RegionMasterRepository = new RegionMasterRepository();
        //    return _RegionMasterRepository.GetRegionById(Id);
        //}


        //public bool AddUpdateRegion(RegionMaster regionMaster)
        //{
        //    _RegionMasterRepository = new RegionMasterRepository();
        //    return _RegionMasterRepository.AddUpdateRegion(regionMaster);

        //    //if (regionMaster.Id > 0)
        //    //{
        //    //    return _RegionMasterRepository.Update(regionMaster);
        //    //}
        //    //else
        //    //{
        //    //    return _RegionMasterRepository.Insert(regionMaster);
        //    //}
        //}

        //public void DeleteRegion(long Id)
        //{
        //    //_RegionMasterRepository.DeleteById(Id);
        //    _RegionMasterRepository = new RegionMasterRepository();
        //    _RegionMasterRepository.DeleteRegionById(Id);
        //}

        #endregion

        #region City

        //public GridDataResponse GetAllCityBySearchRequest(GridParams p_GridParams, CityMasterRequest p_SearchRequest)
        //{
        //    GridDataResponse _GridDataResponse = new GridDataResponse();
        //    _CityMasterRepository = new CityMasterRepository();
        //    var _Country = _CityMasterRepository.GetAllCityBySearchRequest(p_GridParams, p_SearchRequest);

        //    if (_Country != null && _Country.Count > 0)
        //    {
        //        _GridDataResponse.recordsTotal = _Country.FirstOrDefault().RecordsTotal;
        //        _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
        //        _GridDataResponse.data = _Country;
        //    }

        //    return _GridDataResponse;
        //}

        //public List<CityMaster> GetAllCity()
        //{
        //    List<CityMaster> listOfCityMaster = new List<CityMaster>();
        //    _CityMasterRepository = new CityMasterRepository();

        //    try
        //    {
        //        listOfCityMaster = _CityMasterRepository.GetAllCity();
        //    }
        //    catch (Exception _Exception)
        //    {
        //        throw _Exception;
        //    }

        //    return listOfCityMaster;

        //    //var _City = _CityMasterRepository.GetTable()
        //    //                .Join(_RegionMasterRepository.GetTable(),
        //    //                c => c.RegionMasterId,
        //    //                r => r.Id, (c, r) => new { City = c, Region = r })
        //    //                    .Join(_CountryMasterRepository.GetTable(),
        //    //               c => c.Region.CountryMasterId,
        //    //               cm => cm.Id, (c, cm) => new { City = c.City, Region = c.Region, Country = cm })
        //    //               .Select(t => new CityMaster()
        //    //               {
        //    //                   Id = t.City.Id,
        //    //                   RegionMasterId = t.City.RegionMasterId,
        //    //                   CityName = t.City.CityName,
        //    //                   Code = t.City.Code,
        //    //                   IsActive = t.City.IsActive,
        //    //                   CountryMaster = t.Country,
        //    //                   RegionMaster = t.Region
        //    //               });
        //    //return _City.ToList();
        //}

        //public CityMaster GetCityById(long Id)
        //{
        //    _CityMasterRepository = new CityMasterRepository();
        //    CityMaster cityMaster = new EntityModel.CityMaster.CityMaster();
        //    return cityMaster = _CityMasterRepository.GetCityById(Id);

        //    //var _City = _CityMasterRepository.GetTable().Where(t => t.Id == Id)
        //    //                .Join(_RegionMasterRepository.GetTable(),
        //    //                c => c.RegionMasterId,
        //    //                r => r.Id, (c, r) => new { City = c, Region = r })
        //    //               .Select(t => new CityMaster()
        //    //               {
        //    //                   Id = t.City.Id,
        //    //                   RegionMasterId = t.City.RegionMasterId,
        //    //                   CityName = t.City.CityName,
        //    //                   Code = t.City.Code,
        //    //                   CountryMasterId = t.Region.CountryMasterId,
        //    //                   IsActive = t.City.IsActive,
        //    //                   Latitude = t.City.Latitude,
        //    //                   Longitude = t.City.Longitude

        //    //               });

        //    //return _City.FirstOrDefault();
        //}

        //public bool AddUpdateCity(CityMaster cityMaster)
        //{
        //    _CityMasterRepository = new CityMasterRepository();
        //    return _CityMasterRepository.AddUpdateCityMaster(cityMaster);
        //    //if (cityMaster.Id > 0)
        //    //{
        //    //    return _CityMasterRepository.Update(cityMaster);
        //    //}
        //    //else
        //    //{
        //    //    return _CityMasterRepository.Insert(cityMaster);
        //    //}
        //}

        //public void DeleteCity(long Id)
        //{
        //    //_CityMasterRepository.DeleteById(Id);
        //    _CityMasterRepository = new CityMasterRepository();
        //    _CityMasterRepository.DeleteCityById(Id);
        //}

        #endregion

        /// <summary>
        /// Get All Top Destination City
        /// </summary>
        /// <returns> List of CityMaster </returns>
        public List<CityMaster> GetAllTopDestinationCity()
        {
            List<CityMaster> listOfCityMaster = new List<CityMaster>();
            _CityMasterRepository = new CityMasterRepository();

            try
            {
                listOfCityMaster = _CityMasterRepository.GetAllTopDestinationCity();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return listOfCityMaster;
            
        }


        #region For DropDown

        //public List<SelectListItem> CountryDropDown()
        //{
        //    var lst = new List<SelectListItem>();            
        //    GetAllCountryForDropDown _GetAllCountryForDropDown = new GetAllCountryForDropDown();
        //    lst.Add(new SelectListItem() { Value = "0", Text = "Select" });
        //    lst.AddRange(_GetAllCountryForDropDown.GetAllCountry()
        //        .Where(t => t.IsActive)
        //        .Select(t => new SelectListItem() { Text = t.CountryName, Value = t.Id.ToString() }).ToList());

        //    return lst;
        //}

        //public List<SelectListItem> RegionDropDown(long CountryId)
        //{
            
        //    var lst = new List<SelectListItem>();
        //    lst.Add(new SelectListItem() { Value = "0", Text = "Select" });
        //    lst.AddRange(.GetRegionByCountryId(CountryId)
        //        .Where(t => t.IsActive && t.CountryMasterId == CountryId)
        //        .Select(t => new SelectListItem() { Text = t.RegionName, Value = t.Id.ToString() }).ToList());

        //    return lst;
        //}

        //public List<SelectListItem> CityDropDown(long RegionId)
        //{
        //    var lst = new List<SelectListItem>();
        //    lst.Add(new SelectListItem() { Value = "0", Text = "Select" });
        //    lst.AddRange(_CityMasterRepository.GetTable()
        //        .Where(t => t.IsActive && t.RegionMasterId == RegionId)
        //        .Select(t => new SelectListItem() { Text = t.CityName, Value = t.Id.ToString() }).ToList());

        //    return lst;

        //}

        #endregion



        public List<KeyValueDto> GetCityNamesForDropDown(string search)
        {
            List<KeyValueDto> _List = new List<KeyValueDto>();
            _List = _CityMasterRepository.GetCityNamesForDropDown(search);
            return _List;
        }

    }
}