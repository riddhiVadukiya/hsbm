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
    public class AccountStatementService
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public GridDataResponse GetAccountStatementBySearchRequest(RequestResponseServiceContext requestResponseServiceContext, GridParams p_GridParams, AccountStatementRequest p_Request)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            AccountStatementRepository _AccountStatementRepository = new AccountStatementRepository();

            try
            {
                _GridDataResponse = _AccountStatementRepository.GetAccountStatementBySearchRequest(p_GridParams, p_Request);
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
                _ILogger.Error("AccountStatementService=>GetAccountStatementBySearchRequest: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

    }
}