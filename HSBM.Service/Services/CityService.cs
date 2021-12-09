using HSBM.Common.Utils;
using HSBM.EntityModel.CityMaster;
using HSBM.EntityModel.Common;
using HSBM.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HSBM.Service.Services
{
    public class CityService
    {
        #region Service for ActiveInactive City By CityId
        /// <summary>
        /// ActiveInactive City By CityId
        /// </summary>
        /// <param name="cityMaster">Object of CityMaster</param>
        /// <returns> true/false </returns>
        public bool ActiveInactiveCityById(CityMaster cityMaster)
        {
            try
            {
                CityMasterRepository _CityMasterRepository = new CityMasterRepository();
                return _CityMasterRepository.ActiveAndInactiveSwitchForCityUpdate(cityMaster);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        #endregion

        #region Service for Top Destination Switch City By CityId
        /// <summary>
        /// Top Destination Switch City By CityId
        /// </summary>
        /// <param name="cityMaster">Object of CityMaster</param>
        /// <returns> true/false </returns>
        public bool TopDestinationSwitchCityById(CityMaster cityMaster)
        {
            try
            {
                CityMasterRepository _CityMasterRepository = new CityMasterRepository();
                return _CityMasterRepository.TopDestinationSwitchForCityUpdate(cityMaster);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        #endregion

        #region Service for AddUpdate CityMaster
        /// <summary>
        /// AddUpdate CityMaster
        /// </summary>
        /// <param name="cityMaster">Object of CityMaster</param>
        /// <returns> true/false </returns>
        public bool AddUpdateCity(CityMaster cityMaster)
        {
            try
            {
                CityMasterRepository _CityMasterRepository = new CityMasterRepository();
                return _CityMasterRepository.AddUpdateCityMaster(cityMaster);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        #endregion

        #region Delete City by Id
        /// <summary>
        /// Delete City by Id
        /// </summary>
        /// <param name="Id">City Id</param>
        /// <returns> true/false </returns>
        public bool DeleteCity(long Id)
        {
            try
            {
                CityMasterRepository _CityMasterRepository = new CityMasterRepository();
                return _CityMasterRepository.DeleteCityById(Id);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        #endregion

        #region Get City By SearchRequest
        /// <summary>
        /// Get City By SearchRequest
        /// </summary>
        /// <param name="p_GridParams">Objcet of GridParams</param>
        /// <param name="p_SearchRequest">Objcet of CityMasterRequest</param>
        /// <returns> Object of GridDataResponse </returns>
        public GridDataResponse GetCityBySearchRequest(GridParams p_GridParams, CityMasterRequest p_SearchRequest)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            try
            {
                CityMasterRepository _CityMasterRepository = new CityMasterRepository();
                var _Country = _CityMasterRepository.GetAllCityBySearchRequest(p_GridParams, p_SearchRequest);

                if (_Country != null && _Country.Count > 0)
                {
                    _GridDataResponse.recordsTotal = _Country.FirstOrDefault().RecordsTotal;
                    _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
                    _GridDataResponse.data = _Country;
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _GridDataResponse;
        }
        #endregion

        #region Get All CityList
        /// <summary>
        /// Get All CityList
        /// </summary>
        /// <returns> List of CityMaster </returns>
        public List<CityMaster> GetAllCity()
        {
            List<CityMaster> _ListOfCityMaster = new List<CityMaster>();
            CityMasterRepository _CityMasterRepository = new CityMasterRepository();

            try
            {
                _ListOfCityMaster = _CityMasterRepository.GetAllCity();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfCityMaster;
        }
        #endregion

        #region Service for Get City By CityId
        /// <summary>
        /// Get City By CityId
        /// </summary>
        /// <param name="Id"> City Id </param>
        /// <returns> Object of CityMaster </returns>
        public CityMaster GetCityById(long Id)
        {
            CityMaster cityMaster = new EntityModel.CityMaster.CityMaster();
            try
            {
                CityMasterRepository _CityMasterRepository = new CityMasterRepository();
                cityMaster = _CityMasterRepository.GetCityById(Id);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return cityMaster;
        }
        #endregion

        #region City DropDownList
        /// <summary>
        /// City DropDownList
        /// </summary>
        /// <param name="RegionId">RegionId</param>
        /// <returns> List of SelectListItem </returns>
        public List<SelectListItem> CityDropDown(long RegionId)
        {
            List<SelectListItem> _ListOfSelectListItem = new List<SelectListItem>();
            List<CityMaster> _ListOfCityMaster = new List<CityMaster>();
            CityMasterRepository _CityMasterRepository = new CityMasterRepository();

            try
            {
                _ListOfCityMaster = _CityMasterRepository.GetAllCityByRegionMasterId(RegionId);
                if (_ListOfCityMaster != null)
                {
                    _ListOfSelectListItem.Add(new SelectListItem() { Value = "0", Text = "Select" });
                    _ListOfSelectListItem.AddRange(_ListOfCityMaster
                        .Select(t => new SelectListItem() { Text = t.CityName, Value = t.Id.ToString() }).ToList());
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfSelectListItem;
        }

        /// <summary>
        /// Get All City By Country
        /// </summary>
        /// <param name="p_CountryId">CountryId</param>
        /// <returns> List of SelectListItem </returns>
        public List<SelectListItem> GetAllCityByCountryId(long p_CountryId)
        {
            List<SelectListItem> _ListOfSelectListItem = new List<SelectListItem>();
            List<CityMaster> _ListOfCityMaster = new List<CityMaster>();
            CityMasterRepository _CityMasterRepository = new CityMasterRepository();

            try
            {
                _ListOfCityMaster = _CityMasterRepository.GetAllCityByCountryId(p_CountryId);
                if (_ListOfCityMaster != null)
                {
                    _ListOfSelectListItem.Add(new SelectListItem() { Value = "0", Text = "Select" });
                    _ListOfSelectListItem.AddRange(_ListOfCityMaster
                        .Select(t => new SelectListItem() { Text = t.CityName, Value = t.Id.ToString() }).ToList());
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfSelectListItem;
        }
        #endregion
    }
}
