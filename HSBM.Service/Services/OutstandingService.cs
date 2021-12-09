using HSBM.Common.Utils;
using HSBM.EntityModel.AccountSummary;
using HSBM.EntityModel.Common;
using HSBM.Repository.Contracts;
using HSBM.Repository.Repositories;
using HSBM.Service.Contracts;
using HSBM.Service.ServiceContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HSBM.Service.Services
{
    public class OutstandingService
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public GridDataResponse GetOutstandingBySearchRequest(RequestResponseServiceContext requestResponseServiceContext, GridParams p_GridParams, OutstandingRequest p_Request)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            OutstandingRepository _OutstandingRepository = new OutstandingRepository();

            try
            {
                _GridDataResponse = _OutstandingRepository.GetOutstandingBySearchRequest(p_GridParams, p_Request);
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
                _ILogger.Error("OutstandingService=>GetOutstandingBySearchRequest: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

        public int AddOrUpdateOutstanding(RequestResponseServiceContext requestResponseServiceContext, long Farmstaysid, long BookingYear, long BookingMonth, bool Status, long OutstandingId, long userid)
        {
            OutstandingRepository _OutstandingRepository = new OutstandingRepository();
            int result = 0;

            try
            {
                result = _OutstandingRepository.AddOrUpdateOutstanding(Farmstaysid, BookingYear, BookingMonth, Status, OutstandingId, userid);
                if (result == 1)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                }                
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("OutstandingService=>AddOrUpdateOutstanding: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }

            return result;
        }
    }
}