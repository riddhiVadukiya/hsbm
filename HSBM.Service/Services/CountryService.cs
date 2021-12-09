using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.CountryMaster;
using HSBM.Repository.Repositories;
using HSBM.Service.ServiceContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HSBM.Service.Services
{
    public class CountryService
    {
        /// <summary>
        /// Active Inactive Country ById
        /// </summary>
        /// <param name="countryMaster">Object of CountryMaster</param>
        /// <returns> true/false </returns>
        public bool ActiveInactiveCountryById(CountryMaster countryMaster)
        {
            try
            {
                CountryMasterRepository _CountryMasterRepository = new CountryMasterRepository();
                return _CountryMasterRepository.ActiveAndInactiveSwitchForCountryUpdate(countryMaster);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Add or Update Country
        /// </summary>
        /// <param name="countryMaster">CountryMaster</param>
        /// <returns> true/false </returns>
        public bool AddorUpdateCountry(CountryMaster countryMaster)
        {
            try
            {
                CountryMasterRepository _CountryMasterRepository = new CountryMasterRepository();
                return _CountryMasterRepository.AddUpdateCountry(countryMaster);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Delete Country
        /// </summary>
        /// <param name="p_Id">Id</param>
        /// <returns> true/false </returns>
        public bool DeleteCountry(long p_Id)
        {
            try
            {
                CountryMasterRepository _CountryMasterRepository = new CountryMasterRepository();
                return _CountryMasterRepository.DeleteCountryById(p_Id);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Grid of Countries
        /// </summary>
        /// <param name="p_GridParams">GridParams</param>
        /// <param name="p_SearchRequest">CountryMasterRequest</param>
        /// <returns> Object of GridDataResponse </returns>
        public GridDataResponse GetAllCountriesBySearchRequest(GridParams p_GridParams, CountryMasterRequest p_SearchRequest)
        {
            try
            {
                GridDataResponse _GridDataResponse = new GridDataResponse();
                CountryMasterRepository _CountryMasterRepository = new CountryMasterRepository();
                var _Country = _CountryMasterRepository.GetAllCountriesBySearchRequest(p_GridParams, p_SearchRequest);

                if (_Country != null && _Country.Count > 0)
                {
                    _GridDataResponse.recordsTotal = _Country.FirstOrDefault().RecordsTotal;
                    _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
                    _GridDataResponse.data = _Country;
                }

                return _GridDataResponse;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Get All Country Master
        /// </summary>
        /// <returns> List of SelectListItem </returns>
        public List<SelectListItem> GetAllCountry()
        {
            List<SelectListItem> _ListOfSelectListItem = new List<SelectListItem>();
            List<CountryMaster> _ListOfCountryMaster = new List<CountryMaster>();
            try
            {
                CountryMasterRepository countryMasterRepository = new CountryMasterRepository();
                _ListOfCountryMaster = countryMasterRepository.GetAllCountryMaster();

                if (_ListOfCountryMaster != null)
                {
                    _ListOfSelectListItem.Add(new SelectListItem() { Value = "0", Text = "Select" });
                    _ListOfSelectListItem.AddRange(_ListOfCountryMaster
                        .Where(t => t.IsActive).OrderBy(t=>t.CountryName)
                        .Select(t => new SelectListItem() { Text = t.CountryName, Value = t.Id.ToString() }).ToList());
                }

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfSelectListItem;
        }

        /// <summary>
        /// Get Country By Id
        /// </summary>
        /// <param name="p_Id">Country Id</param>
        /// <returns>CountryMaster</returns>
        public CountryMaster GetCountryByCountryId(long Id)
        {
            try
            {
                CountryMasterRepository _CountryMasterRepository = new CountryMasterRepository();
                return _CountryMasterRepository.GetCountryById(Id);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        public long GetCountryByCountryCode(RequestResponseServiceContext requestResponseServiceContext, string p_Code)
        {
            long _CountryId = 0;
            try
            {
                CountryMasterRepository _CountryMasterRepository = new CountryMasterRepository();
                CountryMaster _CountryMaster = _CountryMasterRepository.GetCountryByCountryCode(p_Code);
                _CountryId = _CountryMaster.Id;
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
            }
            catch (Exception _Exception)
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { _Exception.Message };
            }
            return _CountryId;
        }
    }
}
