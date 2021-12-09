using HSBM.EntityModel.AccountSummary;
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
    public class AccountSummaryService
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<AccountSummaryResponse> GetAccountSummary(RequestResponseServiceContext requestResponseServiceContext, AccountSummaryRequest request)
        {
            try
            {
                AccountSummaryRepository repo = new AccountSummaryRepository();
                List<AccountSummaryResponse> result = new List<AccountSummaryResponse>();
                result = repo.GetAccountSummary(request);

                if (result != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not Found." };
                }
                return result;

            }
            catch (Exception ex)
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

        public List<OutstandingSummary> GetOutstandingSummary(RequestResponseServiceContext requestResponseServiceContext)
        {
            try
            {
                AccountSummaryRepository repo = new AccountSummaryRepository();
                List<OutstandingSummary> result = new List<OutstandingSummary>();
                result = repo.GetOutstandingSummary();

                if (result != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not Found." };
                }
                return result;

            }
            catch (Exception ex)
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

    }
}