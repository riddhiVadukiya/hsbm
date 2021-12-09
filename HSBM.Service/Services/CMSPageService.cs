using HSBM.Common.Utils;
using HSBM.EntityModel.CMSPageMaster;
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
    public class CMSPageService
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Active/Inactive CMSPage
        /// </summary>
        /// <param name="cMSPageMaster">CMSPageMaster</param>
        public void ActiveAndInactiveSwitchUpdateForCMSPage(RequestResponseServiceContext requestResponseServiceContext, EntityModel.CMSPageMaster.CMSPageMaster cMSPageMaster)
        {
            CMSPageMasterRepository _CMSPageMasterRepository = new CMSPageMasterRepository();

            try
            {
                _CMSPageMasterRepository.ActiveAndInactiveSwitchUpdateForCMSPage(cMSPageMaster);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
            }
            catch (Exception ex)
            {
                _ILogger.Error("CMSPageService=>ActiveAndInactiveSwitchUpdateForCMSPage: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }
        }

        /// <summary>
        /// Add or Update CMSPage
        /// </summary>
        /// <param name="cmsPageMaster">CMSPageMaster</param>
        /// <returns> integer </returns>
        public int AddorUpdateCMSPage(RequestResponseServiceContext requestResponseServiceContext, CMSPageMaster cmsPageMaster)
        {
            CMSPageMasterRepository _CMSPageMasterRepository = new CMSPageMasterRepository();
            int result = 0;

            try
            {
                result = _CMSPageMasterRepository.AddorUpdateCMSPage(cmsPageMaster);
                if (result == 1)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                }
                else
                {
                    if (result == 2)
                    {
                        requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.IS_EXIST;
                        requestResponseServiceContext.Response.StatusParameters = new string[] { "Page already exist." };
                    }
                    else
                    {
                        requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                        requestResponseServiceContext.Response.StatusParameters = new string[] { "Not found." };
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("CMSPageService=>AddorUpdateCMSPage: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }

            return result;

        }

        /// <summary>
        /// Delete CMSPage By Id
        /// </summary>
        /// <param name="Id">Id</param>
        public void DeleteCMSPageById(RequestResponseServiceContext requestResponseServiceContext, long Id)
        {
            CMSPageMasterRepository _CMSPageMasterRepository = new CMSPageMasterRepository();

            try
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                _CMSPageMasterRepository.DeleteCMSPageById(Id);
            }
            catch (Exception ex)
            {
                _ILogger.Error("CMSPageService=>DeleteCMSPageById: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }
        }

        /// <summary>
        /// Grid of CMSPageMaster
        /// </summary>
        /// <param name="p_GridParams">GridParams</param>
        /// <param name="p_SearchRequest">CMSPageMasterRequest</param>
        /// <returns> Object of GridDataResponse </returns>
        public GridDataResponse GetAllCMSPageMasterBySearchRequest(RequestResponseServiceContext requestResponseServiceContext, GridParams p_GridParams, CMSPageMasterRequest p_SearchRequest)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            CMSPageMasterRepository _CMSPageMasterRepository = new CMSPageMasterRepository();

            try
            {
                var _Country = _CMSPageMasterRepository.GetAllCMSPageMasterBySearchRequest(p_GridParams, p_SearchRequest);

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
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not found." };
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("CMSPageService=>DeleteCMSPageById: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }
            return null;
        }

        /// <summary>
        /// Get CMS PageRecord By Id
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns> Object of CMSPageMaster </returns>
        public CMSPageMaster GetCMSPageById(RequestResponseServiceContext requestResponseServiceContext, long Id)
        {
            CMSPageMaster _CMSPageMaster = new CMSPageMaster();
            CMSPageMasterRepository _CMSPageMasterRepository = new CMSPageMasterRepository();

            try
            {
                _CMSPageMaster = _CMSPageMasterRepository.GetCMSPageRecordById(Id);
                if (_CMSPageMaster != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _CMSPageMaster;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not found." };
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("CMSPageService=>DeleteCMSPageById: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }
            return null;
        }

        public CMSPageMaster GetCMSPageByIdForFront(RequestResponseServiceContext requestResponseServiceContext, long Id)
        {
            CMSPageMaster _CMSPageMaster = new CMSPageMaster();
            CMSPageMasterRepository _CMSPageMasterRepository = new CMSPageMasterRepository();

            try
            {
                _CMSPageMaster = _CMSPageMasterRepository.GetCMSPageRecordByIdForFront(Id);
                if (_CMSPageMaster != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _CMSPageMaster;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not found." };
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("CMSPageService=>DeleteCMSPageById: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }
            return null;
        }

    }
}
