using HSBM.Common.Logging;
using HSBM.Common.Utils;
using HSBM.EntityModel.AmenityMaster;
using HSBM.EntityModel.Common;
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
    public class AmenityMasterService
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get amenity master by search request
        /// </summary>
        /// <param name="requestResponseServiceContext"></param>
        /// <param name="p_GridParams"></param>
        /// <param name="p_Request"></param>
        /// <returns></returns>
        public GridDataResponse GetAllAmenityMastersBySearchRequest(RequestResponseServiceContext requestResponseServiceContext, GridParams p_GridParams, AmenityMasterRequest p_Request)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            AmenityMasterRepository _AmenityMasterRepository = new AmenityMasterRepository();
            try
            {
                _GridDataResponse = _AmenityMasterRepository.GetAllAmenityMastersBySearchRequest(p_GridParams, p_Request);
                if (_GridDataResponse.data != null)
                {
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
                _ILogger.Error("AmenityMasterService=>GetAllAmenityMastersBySearchRequest: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

        /// <summary>
        /// Add or update amenity master
        /// </summary>
        /// <param name="requestResponseServiceContext"></param>
        /// <param name="amenity"></param>
        /// <returns></returns>
        public int AddOrUpdateAmenityMaster(RequestResponseServiceContext requestResponseServiceContext, AmenityMaster amenity)
        {
            AmenityMasterRepository _AmenityMasterRepository = new AmenityMasterRepository();
            int result = 0;

            try
            {
                result = _AmenityMasterRepository.AddOrUpdateAmenityMaster(amenity);
                if (result == 1)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                }
                else if (result == 2)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.IS_EXIST;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Amenity already exist." };
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not found." };
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("AmenityMasterService=>AddOrUpdateAmenityMaster: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }

            return result;
        }

        /// <summary>
        /// Get amenity master by id
        /// </summary>
        /// <param name="requestResponseServiceContext"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AmenityMaster GetAmenityMasterById(RequestResponseServiceContext requestResponseServiceContext, int Id)
        {
            AmenityMaster _AmenityMaster = new AmenityMaster();
            AmenityMasterRepository _AmenityMasterRepository = new AmenityMasterRepository();
            try
            {
                _AmenityMaster = _AmenityMasterRepository.GetAmenityMasterById(Id);
                if (_AmenityMaster != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _AmenityMaster;
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
                _ILogger.Error("AmenityMasterService=>GetAllAmenityMastersBySearchRequest: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
            
        }

        /// <summary>
        /// Active or inactive amenity master
        /// </summary>
        /// <param name="requestResponseServiceContext"></param>
        /// <param name="amenity"></param>
        public void ActiveAndInactiveAmenityMaster(RequestResponseServiceContext requestResponseServiceContext, AmenityMaster amenity)
        {
            AmenityMasterRepository _AmenityMasterRepository = new AmenityMasterRepository();
            try
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                _AmenityMasterRepository.ActiveAndInactiveAmenityMaster(amenity);
            }
            catch (Exception ex)
            {
                _ILogger.Error("AmenityMasterService=>ActiveAndInactiveAmenityMaster: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }
        }

        /// <summary>
        /// Get all amenity master for drop down
        /// </summary>
        /// <param name="requestResponseServiceContext"></param>
        /// <returns></returns>
        public List<SelectListItem> GetAllAmenityMastersForDropDown(RequestResponseServiceContext requestResponseServiceContext)
        {
            AmenityMasterRepository _AmenityMasterRepository = new AmenityMasterRepository();
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                list = _AmenityMasterRepository.GetAllAmenityMastersForDropDown();
                if (list.Any())
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return list;
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
                _ILogger.Error("AmenityMasterService=>GetAllAmenityMastersForDropDown: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

    }
}
