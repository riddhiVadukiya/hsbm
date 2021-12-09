using HSBM.Common.Utils;
using HSBM.EntityModel.BannerMaster;
using HSBM.EntityModel.Common;
using HSBM.Repository.Repositories;
using HSBM.Service.ServiceContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Service.Services
{
    public class BannerService
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        BannerMasterRepository _BannerMasterRepository = new BannerMasterRepository();

        /// <summary>
        /// Active/Inactive Banner
        /// </summary>
        /// <param name="bannerMaster">BannerMaster</param>
        public void ActiveAndInactiveSwitchUpdateForBanner(RequestResponseServiceContext requestResponseServiceContext, EntityModel.BannerMaster.BannerMaster bannerMaster)
        {
            try
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                _BannerMasterRepository.ActiveAndInactiveSwitchUpdateForBanner(bannerMaster);
            }
            catch (Exception ex)
            {
                _ILogger.Error("AmenityMaster=>ActiveAndInactiveAmenityMaster: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }
        }

        /// <summary>
        /// Add / Update Banner
        /// </summary>
        /// <param name="bannerMaster">BannerMaster</param>
        /// <returns> integer </returns>
        public int AddorUpdateBanner(RequestResponseServiceContext requestResponseServiceContext, List<BannerMaster> bannerMaster)
        {
            int result = 0;
            try
            {
                result = _BannerMasterRepository.AddorUpdateBanner(bannerMaster);
                if (result > 0)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Banner has been added successfully." };
                    return result;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not Found." };
                    return 0;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("AmenityMaster=>AddOrUpdateAmenityMaster: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return 0;
            }
        }

        /// <summary>
        /// Delete Banner By Id
        /// </summary>
        /// <param name="Id">Banner Id</param>
        public void DeleteBannerById(RequestResponseServiceContext requestResponseServiceContext, long Id)
        {
            try
            {
                _BannerMasterRepository.DeleteBannerById(Id);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
            }
            catch (Exception ex)
            {
                _ILogger.Error("AmenityMaster=>AddOrUpdateAmenityMaster: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }
        }

        /// <summary>
        /// Grid of all Banners
        /// </summary>
        /// <param name="p_GridParams">GridParams</param>
        /// <param name="p_SearchRequest">BannerMasterRequest</param>
        /// <returns> Object of GridDataResponse </returns>
        public GridDataResponse GetAllBannerBySearchRequest(RequestResponseServiceContext requestResponseServiceContext, GridParams p_GridParams, BannerMasterRequest p_SearchRequest)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();

            try
            {
                var _Country = _BannerMasterRepository.GetAllBannerMasterBySearchRequest(p_GridParams, p_SearchRequest);

                if (_Country != null && _Country.Count > 0)
                {
                    _GridDataResponse.recordsTotal = _Country.FirstOrDefault().RecordsTotal;
                    _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
                    _GridDataResponse.data = _Country;

                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;

                    return _GridDataResponse;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not Found." };
                    return null;
                }

            }
            catch (Exception ex)
            {
                _ILogger.Error("AmenityMaster=>AddOrUpdateAmenityMaster: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

        /// <summary>
        /// Get All Banners
        /// </summary>
        /// <returns> List of BannerMasterResponse </returns>
        public List<BannerMasterResponse> GetAllBanners(RequestResponseServiceContext requestResponseServiceContext)
        {
            List<BannerMasterResponse> _Country = new List<BannerMasterResponse>();
            try
            {
                _Country = _BannerMasterRepository.GetAllBanners();
                if (_Country.Any())
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _Country;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not Found." };
                    return null;
                }

            }
            catch (Exception ex)
            {
                _ILogger.Error("AmenityMaster=>AddOrUpdateAmenityMaster: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

        /// <summary>
        /// Get Banner By Id
        /// </summary>
        /// <param name="Id">Banner Id</param>
        /// <returns> Object of BannerMaster </returns>
        public BannerMaster GetBannerById(RequestResponseServiceContext requestResponseServiceContext, long Id)
        {
            BannerMaster _BannerMasterModel = new BannerMaster();

            try
            {
                _BannerMasterModel = _BannerMasterRepository.GetBannerRecordById(Id);
                if (_BannerMasterModel != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _BannerMasterModel;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not Found." };
                    return null;
                }

            }
            catch (Exception ex)
            {
                _ILogger.Error("AmenityMaster=>AddOrUpdateAmenityMaster: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

    }
}
