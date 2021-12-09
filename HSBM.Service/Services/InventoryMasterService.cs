using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.InventoryMaster;
using HSBM.Repository.Repositories;
using HSBM.Service.Contracts;
using HSBM.Service.ServiceContext;
using System;
using System.Collections.Generic;
using System.Linq;
namespace HSBM.Service.Services
{
    public class InventoryMasterService
    {
        #region Init
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public InventoryMasterService()
        {

        }

        #endregion

        public int AddInventoryMaster(RequestResponseServiceContext requestResponseServiceContext, InventoryMaster inventoryMaster)
        {
            InventoryMasterRepository _InventoryMasterRepository = new InventoryMasterRepository();
            int result = 0;

            try
            {
                result = _InventoryMasterRepository.AddInventoryMaster(inventoryMaster);
                if (result == 1)
                {
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
                _ILogger.Error("AddInventoryMaster", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }

            return result;
        }

        public GridDataResponse GetAllInventoryMasterBySearchRequest(RequestResponseServiceContext requestResponseServiceContext, GridParams p_GridParams, InventoryMasterRequest p_Request)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            InventoryMasterRepository _InventoryMasterRepository = new InventoryMasterRepository();
            try
            {
                _GridDataResponse = _InventoryMasterRepository.GetAllInventoryMasterBySearchRequest(p_GridParams, p_Request);
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
                _ILogger.Error("GetAllInventoryMasterBySearchRequest: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

        public void DeleteInventoryMaster(RequestResponseServiceContext requestResponseServiceContext, Guid id)
        {
            InventoryMasterRepository _InventoryMasterRepository = new InventoryMasterRepository();
            try
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                _InventoryMasterRepository.DeleteInventoryMaster(id);
            }
            catch (Exception ex)
            {
                _ILogger.Error("DeleteInventoryMaster", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }
        }

        public void DeleteInventoryByOrderId( int Id)
        {
            InventoryMasterRepository _InventoryMasterRepository = new InventoryMasterRepository();
            try
            {
                _InventoryMasterRepository.DeleteInventoryByOrderId(Id);
            }
            catch (Exception ex)
            {
                _ILogger.Error("DeleteInventoryMaster", ex);
            }
        }

        public List<InventoryAvailable> GetInventoryAvailableByDate(RequestResponseServiceContext requestResponseServiceContext, DateTime StartDate, DateTime EndDate, long FarmStaysId)
        {

            InventoryMasterRepository _InventoryMasterRepository = new InventoryMasterRepository();
            List<InventoryAvailable> lst = new List<InventoryAvailable>();

            try
            {
                lst = _InventoryMasterRepository.GetInventoryAvailableByDate(StartDate, EndDate, FarmStaysId);
                if (lst.Any())
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return lst;
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
                _ILogger.Error("GetInventoryAvailableByDate Error :- ", ex);                
            }

            return lst;
        }

        public GridDataResponse GetAllUpCommingOrder(RequestResponseServiceContext requestResponseServiceContext, GridParams p_GridParams, InventoryMasterRequest p_Request)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            InventoryMasterRepository _InventoryMasterRepository = new InventoryMasterRepository();
            try
            {
                _GridDataResponse = _InventoryMasterRepository.GetAllUpCommingOrder(p_GridParams, p_Request);
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
                _ILogger.Error("GetAllInventoryMasterBySearchRequest: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

    }
}