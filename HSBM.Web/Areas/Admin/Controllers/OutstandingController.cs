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
    public class OutstandingController : BaseController
    {
        OutstandingService _OutstandingService = new OutstandingService();

        [CustomAuthorizeAction(Module.Outstanding, ModuleAccess.CanView)]
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorizeAction(Module.Outstanding, ModuleAccess.CanView)]
        public JsonResult GetOutstandingBySearchRequest()
        {
            try
            {
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                OutstandingRequest p_SearchRequest = new OutstandingRequest();
                p_SearchRequest.StartDate = startDate.ToString("dd-MMM-yyyy");
                p_SearchRequest.EndDate = endDate.ToString("dd-MMM-yyyy");
                p_SearchRequest.FarmStyasId = 0;

                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "BookingYear";
                else if (Convert.ToInt16(Request.Params["order[0][column]"]) == 3)
                    p_GridParams.DefaultOrderBy = "TotalOutstanding" + " " + Request.Params["order[0][dir]"];

                GridDataResponse _GridDataResponse = _OutstandingService.GetOutstandingBySearchRequest(WebRequestResponseServiceContext, p_GridParams, p_SearchRequest);

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

        [CustomAuthorizeAction(Module.Outstanding, ModuleAccess.CanView)]
        public ActionResult History()
        {
            FrontFarmStaysSearchService _farmStaysService = new FrontFarmStaysSearchService();
            List<SelectListItem> _ListFrontFarmStays = _farmStaysService.GetFarmStaysForDropDown();
            ViewBag.FrontFarmStays = _ListFrontFarmStays;

            return View();
        }

        [CustomAuthorizeAction(Module.Outstanding, ModuleAccess.CanView)]
        public JsonResult GetOutstandingHistoryBySearchRequest(OutstandingHistoryRequest p_SearchRequest)
        {
            try
            {
                DateTime now = DateTime.Now;
                var startDate = new DateTime(p_SearchRequest.startYear, p_SearchRequest.startMonth, 1);
                var endDate = new DateTime(p_SearchRequest.endYear, p_SearchRequest.endMonth, 1).AddMonths(1).AddDays(-1);

                OutstandingRequest req = new OutstandingRequest();
                req.StartDate = startDate.ToString("dd-MMM-yyyy");
                req.EndDate = endDate.ToString("dd-MMM-yyyy");
                req.FarmStaysName = p_SearchRequest.FarmStaysName;

                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "BookingYear";
                else if (Convert.ToInt16(Request.Params["order[0][column]"]) == 4)
                    p_GridParams.DefaultOrderBy = "TotalOutstanding" + " " + Request.Params["order[0][dir]"];

                GridDataResponse _GridDataResponse = _OutstandingService.GetOutstandingBySearchRequest(WebRequestResponseServiceContext, p_GridParams, req);

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

        public ActionResult Export(OutstandingHistoryRequest Search, int ExportTypeExcel)
        {
            DateTime now = DateTime.Now;
            var startDate = new DateTime(Search.startYear, Search.startMonth, 1);
            var endDate = new DateTime(Search.endYear, Search.endMonth, 1).AddMonths(1).AddDays(-1);

            OutstandingRequest req = new OutstandingRequest();
            req.StartDate = startDate.ToString("dd-MMM-yyyy");
            req.EndDate = endDate.ToString("dd-MMM-yyyy");
            req.FarmStaysName = Search.FarmStaysName;


            GridParams p_GridParams = new GridParams();
            p_GridParams.take = int.MaxValue;

            if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                p_GridParams.DefaultOrderBy = "BookingYear";

            GridView gv = new GridView();
            GridDataResponse _GridDataResponse = new GridDataResponse();
            _GridDataResponse = _OutstandingService.GetOutstandingBySearchRequest(WebRequestResponseServiceContext, p_GridParams, req);
            var data = (from s in ((List<Outstanding>)_GridDataResponse.data) select new { FarmstaysName = s.FarmstaysName, BookingMonthandYear = s.BookingMonthandYear, TotalBooking = s.TotalBooking, TotalOutstandingString = "INR" + (s.TotalOutstanding) , Status=s.IsCleared?"Cleared":"Pending" });
            gv.DataSource = data;
            gv.DataBind();


            if (Convert.ToBoolean(ExportTypeExcel))
            {
                return new ExportToExcel((GridView)gv, DateTime.Now.ToString("yyyyMMdd_hhmmss") + "_Outstanding");
            }
            else
            {

                List<object> _data = new List<object>();
                _data.AddRange(data);

                List<string> _header = new List<string>();
                _header.Add("Farm/Home Stay");
                _header.Add("Month/Year");
                _header.Add("Total Booking");
                _header.Add("Outstanding");
                _header.Add("Status");

                // string _CSVString = covertToCsv<object>(_data, _header);
                return new ExportToCSV(_data, _header, DateTime.Now.ToString("yyyyMMdd_hhmmss") + "_Outstanding");
            }
        }


        [HttpPost]
        [CustomAuthorizeAction(Module.Outstanding, ModuleAccess.CanUpdate)]
        public JsonResult OutstandingUpdate(long Farmstaysid, long BookingYear, long BookingMonth, bool Status, long OutstandingId)
        {
            try
            {
                var userid = SessionProxy.UserDetails.Id;

                _OutstandingService.AddOrUpdateOutstanding(WebRequestResponseServiceContext, Farmstaysid, BookingYear, BookingMonth, Status, OutstandingId, userid);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return Json(string.Empty, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
        }

    }
}

