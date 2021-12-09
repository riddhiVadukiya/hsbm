using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.SiteSettings;
using HSBM.Repository.Repositories;
using HSBM.Service.ServiceContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Service.Services
{
    public class SiteSettingService
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Active/Inactive SiteSetting
        /// </summary>
        /// <param name="siteSettings">SiteSettings</param>
        public void ActiveAndInactiveSwitchUpdateForSS(RequestResponseServiceContext requestResponseServiceContext, SiteSettings siteSettings)
        {
            SiteSettingsRepository _SiteSettingsRepository = new SiteSettingsRepository();

            try
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                _SiteSettingsRepository.ActiveAndInactiveSwitchUpdateForSS(siteSettings);
            }
            catch (Exception ex)
            {
                _ILogger.Error("SiteSettingService=>ActiveAndInactiveSwitchUpdateForSS: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }
        }

        /// <summary>
        /// Add or Update SiteSetting
        /// </summary>
        /// <param name="siteSettings">SiteSettings</param>
        /// <returns> integer </returns>
        public int AddorUpdateSiteSetting(RequestResponseServiceContext requestResponseServiceContext, SiteSettings siteSettings)
        {

            SiteSettingsRepository _SiteSettingsRepository = new SiteSettingsRepository();
            int result = 0;

            try
            {
                result = _SiteSettingsRepository.AddorUpdateSiteSetting(siteSettings);
                if (result > 0)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
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
                _ILogger.Error("SiteSettingService=>AddorUpdateSiteSetting: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return 0;
            }
        }

        /// <summary>
        /// Delete SiteSetting ById
        /// </summary>
        /// <param name="Id">Id</param>
        public void DeleteSiteSettingById(RequestResponseServiceContext requestResponseServiceContext, long Id)
        {
            SiteSettingsRepository _SiteSettingsRepository = new SiteSettingsRepository();
            try
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                _SiteSettingsRepository.DeleteSiteSettingById(Id);
            }
            catch (Exception ex)
            {
                _ILogger.Error("SiteSettingService=>DeleteSiteSettingById: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }
        }

        /// <summary>
        /// Grid of Site Setting
        /// </summary>
        /// <param name="p_GridParams">GridParams</param>
        /// <param name="p_SearchRequest">SiteSettingsRequest</param>
        /// <returns></returns>
        public GridDataResponse GetAllSiteSettingBySearchRequest(RequestResponseServiceContext requestResponseServiceContext, GridParams p_GridParams, SiteSettingsRequest p_SearchRequest)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            SiteSettingsRepository _SiteSettingsRepository = new SiteSettingsRepository();

            try
            {
                var _Country = _SiteSettingsRepository.GetAllSiteSettingBySearchRequest(p_GridParams, p_SearchRequest);

                if (_Country != null && _Country.Count > 0)
                {
                    _GridDataResponse.recordsTotal = _Country.FirstOrDefault().RecordsTotal;
                    _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
                    _GridDataResponse.data = _Country;

                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not found." };
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("SiteSettingService=>GetAllSiteSettingBySearchRequest: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }

            return _GridDataResponse;
        }

        /// <summary>
        /// Get SiteSetting ById
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns> Object of SiteSettings </returns>
        public SiteSettings GetSiteSettingById(RequestResponseServiceContext requestResponseServiceContext, long Id)
        {
            SiteSettingsRepository _SiteSettingsRepository = new SiteSettingsRepository();
            SiteSettings _SiteSettings = new SiteSettings();

            try
            {
                _SiteSettings = _SiteSettingsRepository.GetSiteSettingRecordById(Id);
                if (_SiteSettings != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _SiteSettings;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not found." };
                    return null;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("SiteSettingService=>GetAllSiteSettingBySearchRequest: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }

        }

        public SiteSettings GetSiteSettingRecordForFront(RequestResponseServiceContext requestResponseServiceContext, long Id)
        {
            SiteSettingsRepository _SiteSettingsRepository = new SiteSettingsRepository();
            SiteSettings _SiteSettings = new SiteSettings();

            try
            {
                _SiteSettings = _SiteSettingsRepository.GetSiteSettingRecordForFront(Id);
                if (_SiteSettings != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _SiteSettings;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not found." };
                    return null;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("SiteSettingService=>GetAllSiteSettingBySearchRequest: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }


        }
        
    }
}
