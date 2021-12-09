using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.AccountSummary;
using HSBM.Service.Contracts;
using HSBM.Service.ServiceContext;
using HSBM.Service.Services;
using HSBM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Areas.Admin.Controllers
{
    public class AccountSummaryController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAccountSummary()
        {
            try
            {
                AccountSummaryService service = new AccountSummaryService();
                List<AccountSummaryResponse> result = new List<AccountSummaryResponse>();

                AccountSummaryRequest request = new AccountSummaryRequest()
                {
                    IsAdminLogin = SessionProxy.CheckIsAdminLogin(),
                    SystemUserId = 0
                };

                result = service.GetAccountSummary(WebRequestResponseServiceContext, request);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && result != null)
                {
                    return JsonSuccessResponse(result, string.Empty);
                }
                else
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("Flight=>FlightBookingPaypal:", ex);
                return JsonErrorResponse(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetOutstandingSummary()
        {
            try
            {
                AccountSummaryService service = new AccountSummaryService();
                List<OutstandingSummary> result = new List<OutstandingSummary>();

                result = service.GetOutstandingSummary(WebRequestResponseServiceContext);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && result != null)
                {
                    return JsonSuccessResponse(result, string.Empty);
                }
                else
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("Flight=>FlightBookingPaypal:", ex);
                return JsonErrorResponse(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

    }
}

