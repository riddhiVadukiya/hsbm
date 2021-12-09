using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.DiscountMaster;
using HSBM.Repository.Repositories;
using HSBM.Service.ServiceContext;
using System;
using System.Collections.Generic;
using System.Linq;
namespace HSBM.Service.Services
{
    public class DiscountService
    {
        #region Init
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public DiscountService()
        {

        }

        public GridDataResponse GetAllDiscountMastersBySearchRequest(RequestResponseServiceContext requestResponseServiceContext, GridParams p_GridParams, DiscountMasterRequest p_Request)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            DiscountMasterRepository _DiscountMasterRepository = new DiscountMasterRepository();
            try
            {
                _GridDataResponse = _DiscountMasterRepository.GetAllDiscountMastersBySearchRequest(p_GridParams, p_Request);
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
                _ILogger.Error("GetAllDiscountMastersBySearchRequest: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

        public int AddOrUpdateDiscountMaster(RequestResponseServiceContext requestResponseServiceContext, DiscountMaster discountMaster)
        {
            DiscountMasterRepository _DiscountMasterRepository = new DiscountMasterRepository();
            int result = 0;

            try
            {
                result = _DiscountMasterRepository.AddOrUpdateDiscountMaster(discountMaster);
                if (result == 1)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                }
                else if (result == 2)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.IS_EXIST;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Discount already exist." };
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not found." };
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("AddOrUpdateDiscountMaster", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }

            return result;
        }

        public DiscountMaster GetDiscountMasterById(RequestResponseServiceContext requestResponseServiceContext, long Id)
        {
            DiscountMaster _DiscountMaster = new DiscountMaster();
            DiscountMasterRepository _DiscountMasterRepository = new DiscountMasterRepository();
            try
            {
                _DiscountMaster = _DiscountMasterRepository.GetDiscountMasterById(Id);
                if (_DiscountMaster != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _DiscountMaster;
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
                _ILogger.Error("GetDiscountMasterById", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }

        }

        public void DeleteDiscountMaster(RequestResponseServiceContext requestResponseServiceContext, int id)
        {
            DiscountMasterRepository _DiscountMasterRepository = new DiscountMasterRepository();
            try
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                _DiscountMasterRepository.DeleteDiscountMaster(id);
            }
            catch (Exception ex)
            {
                _ILogger.Error("DeleteDiscountMaster", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }
        }

        public List<DiscountHistoryResponse> GetDiscountHistoryByid(long DiscountId)
        {
            List<DiscountHistoryResponse> _DiscountHistory = new List<DiscountHistoryResponse>();
            DiscountMasterRepository _DiscountMasterRepository = new DiscountMasterRepository();
            try
            {
                _DiscountHistory = _DiscountMasterRepository.GetDiscountHistoryByid(DiscountId);
            }
            catch (Exception ex)
            {
                _ILogger.Error("GetDiscountHistoryByid", ex);
                return null;
            }
            return _DiscountHistory;
        }
    }
}