using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.AccountSummary;
using HSBM.EntityModel.Common;
using HSBM.Service.Contracts;
using HSBM.Service.ServiceContext;
using HSBM.Service.Services;
using HSBM.Web.Areas.Admin.Models;
using HSBM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace HSBM.Web.Areas.Admin.Controllers
{
    public class AccountStatementController : BaseController
    {
        AccountStatementService _AccountStatementService = new AccountStatementService();

        [CustomAuthorizeAction(Module.AccountStatement, ModuleAccess.CanView)]
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorizeAction(Module.AccountStatement, ModuleAccess.CanView)]
        public JsonResult GetAccountStatementBySearchRequest()
        {
            try
            {
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                AccountStatementRequest p_SearchRequest = new AccountStatementRequest();
                p_SearchRequest.BookingFrom = startDate.ToString("dd-MMM-yyyy");
                p_SearchRequest.BookingTo = endDate.ToString("dd-MMM-yyyy");
                p_SearchRequest.FarmStyasId = 0;

                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "BookingYear";
                else if (Convert.ToInt16(Request.Params["order[0][column]"]) == 3)
                    p_GridParams.DefaultOrderBy = "TotalEarning" + " " + Request.Params["order[0][dir]"];

                GridDataResponse _GridDataResponse = _AccountStatementService.GetAccountStatementBySearchRequest(WebRequestResponseServiceContext, p_GridParams, p_SearchRequest);

                if (_GridDataResponse.data != null && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return Json(_GridDataResponse, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        public ActionResult Export(AccountStatementHistoryRequest Search, int ExportTypeExcel)
        {
            DateTime now = DateTime.Now;
            var startDate = new DateTime(Search.startYear, Search.startMonth, 1);
            var endDate = new DateTime(Search.endYear, Search.endMonth, 1).AddMonths(1).AddDays(-1);

            AccountStatementRequest req = new AccountStatementRequest();
            req.BookingFrom = startDate.ToString("dd-MMM-yyyy");
            req.BookingTo = endDate.ToString("dd-MMM-yyyy");
            req.FarmStyasId = Search.FarmstyaId;


            GridParams p_GridParams = new GridParams();
            p_GridParams.take = int.MaxValue;

            if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                p_GridParams.DefaultOrderBy = "BookingDate";

            GridView gv = new GridView();
            GridDataResponse _GridDataResponse = new GridDataResponse();
            _GridDataResponse = _AccountStatementService.GetAccountStatementBySearchRequest(WebRequestResponseServiceContext, p_GridParams, req);
            var data = (from s in ((List<AccountStatement>)_GridDataResponse.data) select new { FarmstaysName = s.FarmstaysName, BookingMonthandYear = s.BookingDate, TotalBooking = s.TotalBooking, TotalEarningString = "INR" + (s.TotalEarning) });
            gv.DataSource = data;
            gv.DataBind();


            if (Convert.ToBoolean(ExportTypeExcel))
            {
                return new ExportToExcel((GridView)gv, DateTime.Now.ToString("yyyyMMdd_hhmmss") + "_AccountStatement");
            }
            else
            {

                List<object> _data = new List<object>();
                _data.AddRange(data);

                List<string> _header = new List<string>();
                _header.Add("Farm/Home Stay");
                _header.Add("Month/Year");
                _header.Add("Total Booking");
                _header.Add("Total Earning");

                // string _CSVString = covertToCsv<object>(_data, _header);
                return new ExportToCSV(_data, _header, DateTime.Now.ToString("yyyyMMdd_hhmmss") + "_AccountStatement");
            }
        }


        [CustomAuthorizeAction(Module.AccountStatement, ModuleAccess.CanView)]
        public ActionResult History()
        {
            FrontFarmStaysSearchService _farmStaysService = new FrontFarmStaysSearchService();
            List<SelectListItem> _ListFrontFarmStays = _farmStaysService.GetFarmStaysForDropDown();
            ViewBag.FrontFarmStays = _ListFrontFarmStays;

            return View();
        }

        [CustomAuthorizeAction(Module.AccountStatement, ModuleAccess.CanView)]
        public JsonResult GetAccountStatementHistoryBySearchRequest(AccountStatementRequest p_SearchRequest)
        {
            try
            {
               // DateTime now = DateTime.Now;
                //var startDate = new DateTime(p_SearchRequest.startYear, p_SearchRequest.startMonth, 1);
                //var endDate = new DateTime(p_SearchRequest.endYear, p_SearchRequest.endMonth, 1).AddMonths(1).AddDays(-1);
                //var checkinFromDate = new DateTime(p_SearchRequest.checkinFromMonth, p_SearchRequest.checkinFromMonth, 1);
                //var checkinToDate = new DateTime(p_SearchRequest.checkinToYear, p_SearchRequest.checkinToMonth, 1).AddMonths(1).AddDays(-1);

                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "TotalEarning";
                else if (Convert.ToInt16(Request.Params["order[0][column]"]) == 4)
                    p_GridParams.DefaultOrderBy = "TotalEarning" + " " + Request.Params["order[0][dir]"];

                GridDataResponse _GridDataResponse = _AccountStatementService.GetAccountStatementBySearchRequest(WebRequestResponseServiceContext, p_GridParams, p_SearchRequest);

                if (_GridDataResponse.data != null && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return Json(_GridDataResponse, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }


    }
}

