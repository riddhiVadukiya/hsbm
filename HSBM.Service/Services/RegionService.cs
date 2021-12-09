using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.RegionMaster;
using HSBM.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HSBM.Service.Services
{
    public class RegionService
    {
        #region Service for ActiveInactive Region ByRegionId
        /// <summary>
        /// ActiveInactive Region ByRegionId
        /// </summary>
        /// <param name="countryMaster">Objcet of CountryMaster</param>
        /// <returns></returns>
        public bool ActiveInactiveRegionById(RegionMaster p_RegionMaster)
        {
            try
            {
                RegionMasterRepository _RegionMasterRepository = new RegionMasterRepository();
                return _RegionMasterRepository.ActiveAndInactiveSwitchForRegionUpdate(p_RegionMaster);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        #endregion

        #region Service for Add Update RegionMaster
        /// <summary>
        /// Add Update RegionMaster
        /// </summary>
        /// <param name="regionMaster">Objcet of RegionMaster</param>
        /// <returns></returns>
        public bool AddUpdateRegion(RegionMaster regionMaster)
        {
            try
            {
                RegionMasterRepository _RegionMasterRepository = new RegionMasterRepository();
                return _RegionMasterRepository.AddUpdateRegion(regionMaster);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        #endregion

        #region Service for Delete Region
        /// <summary>
        /// Delete Region by region Id
        /// </summary>
        /// <param name="Id">RegionId</param>
        public void DeleteRegionById(long Id)
        {
            try
            {
                RegionMasterRepository _RegionMasterRepository = new RegionMasterRepository();
                _RegionMasterRepository.DeleteRegionById(Id);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        #endregion

        #region Service for Get Region for DropDown
        /// <summary>
        /// Get Region for DropDown
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetAllRegionById(long p_Id)
        {
            List<SelectListItem> _ListOfSelectListItem = new List<SelectListItem>();
            List<RegionMaster> _ListOfRegionMaster = new List<RegionMaster>();
            try
            {
                RegionMasterRepository _RegionMasterRepository = new RegionMasterRepository();
                _ListOfRegionMaster = _RegionMasterRepository.GetRegionByCountryId(p_Id);

                if (_ListOfRegionMaster != null)
                {
                    _ListOfSelectListItem.Add(new SelectListItem() { Value = "0", Text = "Select" });
                    _ListOfSelectListItem.AddRange(_ListOfRegionMaster
                        .Where(t => t.IsActive && t.CountryMasterId == p_Id)
                        .Select(t => new SelectListItem() { Text = t.RegionName, Value = t.Id.ToString() }).ToList());
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfSelectListItem;
        }
        #endregion

        #region Service for Get All Region By SearchRequest
        /// <summary>
        /// Get All Region By SearchRequest
        /// </summary>
        /// <param name="p_GridParams"></param>
        /// <param name="p_SearchRequest"></param>
        /// <returns></returns>
        public GridDataResponse GetRegionBySearchRequest(GridParams p_GridParams, RegionMasterRequest p_SearchRequest)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            try
            {
                RegionMasterRepository _RegionMasterRepository = new RegionMasterRepository();
                var Regions = _RegionMasterRepository.GetAllRegionBySearchRequest(p_GridParams, p_SearchRequest);

                if (Regions != null && Regions.Count > 0)
                {
                    _GridDataResponse.recordsTotal = Regions.FirstOrDefault().RecordsTotal;
                    _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
                    _GridDataResponse.data = Regions;
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _GridDataResponse;
        }
        #endregion

        #region Get All Region List
        /// <summary>
        /// Get All Region List
        /// </summary>
        /// <returns></returns>
        public List<RegionMaster> GetAllRegion()
        {
            try
            {
                RegionMasterRepository _RegionMasterRepository = new RegionMasterRepository();
                return _RegionMasterRepository.GetAllRegion();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        #endregion

        #region Get Region By RegionId
        /// <summary>
        /// Get Region By RegionId
        /// </summary>
        /// <param name="Id">Region Id</param>
        /// <returns></returns>
        public RegionMaster GetRegionById(long Id)
        {
            try
            {
                RegionMasterRepository _RegionMasterRepository = new RegionMasterRepository();
                return _RegionMasterRepository.GetRegionById(Id);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        #endregion
    }
}
