using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.CurrencyMaster;
using HSBM.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HSBM.Service.Services
{
    public class CurrencyService
    {
        /// <summary>
        /// Active Inactive CurrencyMaster
        /// </summary>
        /// <param name="currencyMaster">CurrencyMaster</param>
        public void ActiveAndInactiveSwitchUpdateForCur(CurrencyMaster currencyMaster)
        {
            CurrencyMasterRepository _CurrencyMasterRepository = new CurrencyMasterRepository();

            _CurrencyMasterRepository.ActiveAndInactiveSwitchUpdateForCur(currencyMaster);
        }

        /// <summary>
        /// Delete Currency By Id
        /// </summary>
        /// <param name="Id">Id</param>
        public void DeleteCurrencyById(long Id)
        {
            CurrencyMasterRepository _CurrencyMasterRepository = new CurrencyMasterRepository();

            _CurrencyMasterRepository.DeleteCurrencyById(Id);
        }

        /// <summary>
        /// Add or Update Currency
        /// </summary>
        /// <param name="currencyMaster">CurrencyMaster</param>
        /// <returns> integer </returns>
        public int AddorUpdateCurrency(CurrencyMaster currencyMaster)
        {
            CurrencyMasterRepository _CurrencyMasterRepository = new CurrencyMasterRepository();

            if (currencyMaster.Id > 0)
            {
                return _CurrencyMasterRepository.AddorUpdateCurrency(currencyMaster);
            }
            else
            {
                return _CurrencyMasterRepository.AddorUpdateCurrency(currencyMaster);
            }
        }

        /// <summary>
        /// Grid of Currency
        /// </summary>
        /// <param name="p_GridParams">GridParams</param>
        /// <param name="p_SearchRequest">CurrencyMasterRequest</param>
        /// <returns>Object of GridDataResponse</returns>
        public GridDataResponse GetAllCurrencyBySearchRequest(GridParams p_GridParams, CurrencyMasterRequest p_SearchRequest)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();

            CurrencyMasterRepository _CurrencyMasterRepository = new CurrencyMasterRepository();

            var _Country = _CurrencyMasterRepository.GetAllCurrencyMasterBySearchRequest(p_GridParams, p_SearchRequest);

            if (_Country != null && _Country.Count > 0)
            {
                _GridDataResponse.recordsTotal = _Country.FirstOrDefault().RecordsTotal;
                _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
                _GridDataResponse.data = _Country;
            }

            return _GridDataResponse;
        }

        /// <summary>
        /// Get Currency DropDown
        /// </summary>
        /// <returns> List of SelectListItem </returns>
        public List<SelectListItem> GetAllCurrencyForDropDown()
        {
            List<SelectListItem> _Response = new List<SelectListItem>();

            CurrencyMasterRepository _CurrencyMasterRepository = new CurrencyMasterRepository();

            _Response = _CurrencyMasterRepository.GetAllCurrencyForDropDown();

            return _Response;
        }

        /// <summary>
        /// Get Currency By Id
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns> Object of CurrencyMaster </returns>
        public CurrencyMaster GetCurrencyById(long Id)
        {
            CurrencyMasterRepository _CurrencyMasterRepository = new CurrencyMasterRepository();

            CurrencyMaster _CurrencyMaster = new CurrencyMaster();

            _CurrencyMaster = _CurrencyMasterRepository.GetCurrencyRecordById(Id);

            return _CurrencyMaster;
        }

        /// <summary>
        /// Get All CurrencyMaster
        /// </summary>
        /// <returns> Object of CurrencyMasterResponse </returns>
        public List<CurrencyMasterResponse> GetAllCurrencyMaster()
        {
            CurrencyMasterRepository _CurrencyMasterRepository = new CurrencyMasterRepository();

            List<CurrencyMasterResponse> _CurrencyMaster = new List<CurrencyMasterResponse>();
            _CurrencyMaster = _CurrencyMasterRepository.GetAllCurrencyMaster();

            return _CurrencyMaster;
        }

        public int UpdateCurrencyRates(Dictionary<string, decimal> UpdatedRates, long UpdatedBy)
        {
            CurrencyMasterRepository _CurrencyMasterRepository = new CurrencyMasterRepository();

            return _CurrencyMasterRepository.UpdateCurrencyRates(UpdatedRates, UpdatedBy);

        }

    }
}
