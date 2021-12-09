using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.HomeMaster;
using HSBM.Service.Contracts;
using HSBM.Service.ServiceContext;
using HSBM.Service.Services;
using HSBM.Web.Areas.Admin.Models;
using HSBM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace HSBM.Web.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (TempData["ToastrMSG"] != null)
            {
                ViewBag.ToastrMSG = (ToastrMSG)TempData["ToastrMSG"];
            }
            List<SelectListItem> _ListFrontFarmStays = new List<SelectListItem>();
            FrontFarmStaysSearchService _farmStaysService = new FrontFarmStaysSearchService();
            _ListFrontFarmStays = _farmStaysService.GetFarmStaysForDropDown();
            ViewBag.FrontFarmStays = _ListFrontFarmStays;
            return View();
        }
        public JsonResult NewCustomers()
        {
            try
            {
                HomeService _HomeService = new HomeService();
                List<NewCustomersResponse> _ListNewCustomersResponse = new List<NewCustomersResponse>();
                _ListNewCustomersResponse = _HomeService.GetNewCustomersList(WebRequestResponseServiceContext);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return Json(_ListNewCustomersResponse, JsonRequestBehavior.AllowGet);
                }
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage);
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }


        public JsonResult RecentOrders()
        {
            try
            {
                HomeService _HomeService = new HomeService();
                List<RecentOrder> _ListRecentOrder = new List<RecentOrder>();
                _ListRecentOrder = _HomeService.RecentOrders(WebRequestResponseServiceContext);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return Json(_ListRecentOrder, JsonRequestBehavior.AllowGet);
                }
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage);
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        public JsonResult BindHighestBooked()
        {
            try
            {
                HomeService _HomeService = new HomeService();
                List<HighestBookedFarmStay> _List = new List<HighestBookedFarmStay>();
                _List = _HomeService.BindHighestBooked(WebRequestResponseServiceContext);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return Json(_List, JsonRequestBehavior.AllowGet);
                }
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage);
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        public ActionResult ExportHighestBooked(int ExportTypeExcel)
        {
            try
            {
                HomeService _HomeService = new HomeService();
                List<HighestBookedFarmStay> _List = new List<HighestBookedFarmStay>();
                GridView gv = new GridView();
                GridDataResponse _GridDataResponse = new GridDataResponse();
                _List = _HomeService.BindHighestBooked(WebRequestResponseServiceContext);
                _GridDataResponse.data = _List;
                var data = (from s in ((List<HighestBookedFarmStay>)_GridDataResponse.data) select new { Name = s.label, Count = s.value });
                gv.DataSource = data;
                gv.DataBind();

                if (ExportTypeExcel != -1)
                {
                    if (Convert.ToBoolean(ExportTypeExcel))
                    {
                        return new ExportToExcel((GridView)gv, DateTime.Now.ToString("yyyyMMdd_hhmmss") + "_HighestBooked");
                    }
                    else
                    {

                        List<object> _data = new List<object>();
                        _data.AddRange(data);

                        List<string> _header = new List<string>();
                        _header.Add("Name");
                        _header.Add("Count");

                        return new ExportToCSV(_data, _header, DateTime.Now.ToString("yyyyMMdd_hhmmss") + "_HighestBooked");
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }
        public JsonResult BindFarmStaySummary(int FarmstayId = 0)
        {
            try
            {
                HomeService _HomeService = new HomeService();
                List<FarmStaySummary> _List = new List<FarmStaySummary>();
                _List = _HomeService.BindFarmStaySummary(WebRequestResponseServiceContext, FarmstayId);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return Json(_List, JsonRequestBehavior.AllowGet);
                }
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage);
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        public ActionResult ExportFarmStaySummary(int ExportTypeExcel,FormCollection form)
        {
            try
            {
                HomeService _HomeService = new HomeService();
                List<FarmStaySummary> _List = new List<FarmStaySummary>();
                _List = _HomeService.BindFarmStaySummary(WebRequestResponseServiceContext, Convert.ToInt32(form["ddlFrontFarmStays"]));
                GridView gv = new GridView();
                GridDataResponse _GridDataResponse = new GridDataResponse();
                _GridDataResponse.data = _List;
                var data = (from s in ((List<FarmStaySummary>)_GridDataResponse.data) select new { Date = s.MonthYear, Cancelled = s.TotalCancel, Completed = s.TotalComplete, Earning = s.TotalEarning });
                gv.DataSource = data;
                gv.DataBind();

                if (ExportTypeExcel != -1)
                {
                    if (Convert.ToBoolean(ExportTypeExcel))
                    {
                        return new ExportToExcel((GridView)gv, DateTime.Now.ToString("yyyyMMdd_hhmmss") + "_FarmStaySummary");
                    }
                    else
                    {

                        List<object> _data = new List<object>();
                        _data.AddRange(data);

                        List<string> _header = new List<string>();
                        _header.Add("Date");
                        _header.Add("Cancelled");
                        _header.Add("Completed");
                        _header.Add("Earning");

                        return new ExportToCSV(_data, _header, DateTime.Now.ToString("yyyyMMdd_hhmmss") + "_FarmStaySummary");
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        #region Total Booking
        public JsonResult BindTotalBooking(int Frequency, string dateRange)
        {
            try
            {
                DateTime FromDate = new DateTime();
                DateTime ToDate = new DateTime();
                HomeService _HomeService = new HomeService();
                List<TotalBooking> _List = new List<TotalBooking>();
                if (!string.IsNullOrEmpty(dateRange))
                {
                    string[] fromto = dateRange.Split('-');
                    FromDate = Convert.ToDateTime(fromto[0].Trim());
                    ToDate = Convert.ToDateTime(fromto[1].Trim());
                }
                _List = _HomeService.BindTotalBooking(WebRequestResponseServiceContext, Frequency, FromDate, ToDate);
                if (_List.Count <= 0 && !string.IsNullOrEmpty(dateRange))
                {
                    for (DateTime date = FromDate; date <= ToDate; date = date.AddDays(1))
                    {
                        //allDates.Add(date);
                        TotalBooking _TotalBooking = new TotalBooking();
                        _TotalBooking.IsCustom = "1";
                        _TotalBooking.Month = date.ToString("dd MMM yyyy");
                        _TotalBooking.Booked = 0;
                        _TotalBooking.Cancelled = 0;
                        _List.Add(_TotalBooking);
                    }

                }
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return Json(_List, JsonRequestBehavior.AllowGet);
                }
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage);
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        public ActionResult ExportTotalBooking(int ExportTypeExcel, int Frequency = 4, string daterangepicker_start = "", string daterangepicker_end = "")
        {
            try
            {
                DateTime FromDate = new DateTime();
                DateTime ToDate = new DateTime();
                HomeService _HomeService = new HomeService();
                List<TotalBooking> _List = new List<TotalBooking>();
                if (Frequency == 5)
                {
                    FromDate = Convert.ToDateTime(daterangepicker_start.Trim());
                    ToDate = Convert.ToDateTime(daterangepicker_end.Trim());
                }
                _List = _HomeService.BindTotalBooking(WebRequestResponseServiceContext, Frequency, FromDate, ToDate); GridView gv = new GridView();
                if (_List.Count <= 0 && !string.IsNullOrEmpty(daterangepicker_start) && !string.IsNullOrEmpty(daterangepicker_end))
                {
                    for (DateTime date = FromDate; date <= ToDate; date = date.AddDays(1))
                    {
                        //allDates.Add(date);
                        TotalBooking _TotalBooking = new TotalBooking();
                        _TotalBooking.IsCustom = "1";
                        _TotalBooking.Month = date.ToString("dd MMM yyyy");
                        _TotalBooking.Booked = 0;
                        _TotalBooking.Cancelled = 0;
                        _List.Add(_TotalBooking);
                    }

                }
                GridDataResponse _GridDataResponse = new GridDataResponse();
                _GridDataResponse.data = _List;
                var data = (from s in ((List<TotalBooking>)_GridDataResponse.data) select new { Date = s.strDate, TotalCancelled = s.Cancelled, TotalBooking = s.Booked });
                gv.DataSource = data;
                gv.DataBind();

                if (Convert.ToBoolean(ExportTypeExcel))
                {
                    return new ExportToExcel((GridView)gv, DateTime.Now.ToString("yyyyMMdd_hhmmss") + "_TotalBooking");
                }
                else
                {

                    List<object> _data = new List<object>();
                    _data.AddRange(data);

                    List<string> _header = new List<string>();
                    _header.Add("Date");
                    _header.Add("TotalCancelled");
                    _header.Add("TotalBooking");

                    return new ExportToCSV(_data, _header, DateTime.Now.ToString("yyyyMMdd_hhmmss") + "_TotalBooking");
                }
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }
        #endregion

        #region Total Earning
        public JsonResult BindTotalEarning(int Frequency, string dateRange)
        {
            try
            {
                DateTime FromDate = new DateTime();
                DateTime ToDate = new DateTime();
                HomeService _HomeService = new HomeService();
                List<TotalEarnings> _List = new List<TotalEarnings>();
                if (!string.IsNullOrEmpty(dateRange))
                {
                    string[] fromto = dateRange.Split('-');
                    FromDate = Convert.ToDateTime(fromto[0].Trim());
                    ToDate = Convert.ToDateTime(fromto[1].Trim());
                }
                _List = _HomeService.BindTotalEarning(WebRequestResponseServiceContext, Frequency, FromDate, ToDate);
                if (_List.Count <= 0 && !string.IsNullOrEmpty(dateRange))
                {
                    for (DateTime date = FromDate; date <= ToDate; date = date.AddDays(1))
                    {
                        //allDates.Add(date);
                        TotalEarnings _TotalEarnings = new TotalEarnings();
                        _TotalEarnings.IsCustom = "1";
                        _TotalEarnings.Month = date.ToString("dd MMM yyyy");
                        _TotalEarnings.TotalEarning = 0;
                        _List.Add(_TotalEarnings);
                    }

                }
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return Json(_List, JsonRequestBehavior.AllowGet);
                }
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage);
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        public ActionResult ExportTotalEarning(int ExportTypeExcelEarning, int frequencyEarning = 4, string daterangepickerEarning_start = "", string daterangepickerEarning_end = "")
        {
            try
            {
                DateTime FromDate = new DateTime();
                DateTime ToDate = new DateTime();
                HomeService _HomeService = new HomeService();
                List<TotalEarnings> _List = new List<TotalEarnings>();                
                if (frequencyEarning == 5)
                {
                    FromDate = Convert.ToDateTime(daterangepickerEarning_start.Trim());
                    ToDate = Convert.ToDateTime(daterangepickerEarning_end.Trim());
                }                
                _List = _HomeService.BindTotalEarning(WebRequestResponseServiceContext, frequencyEarning, FromDate, ToDate);
                if (_List.Count <= 0 && !string.IsNullOrEmpty(daterangepickerEarning_start) && !string.IsNullOrEmpty(daterangepickerEarning_start))
                {
                    for (DateTime date = FromDate; date <= ToDate; date = date.AddDays(1))
                    {
                        //allDates.Add(date);
                        TotalEarnings _TotalEarnings = new TotalEarnings();
                        _TotalEarnings.IsCustom = "1";
                        _TotalEarnings.Month = date.ToString("dd MMM yyyy");
                        _TotalEarnings.TotalEarning = 0;
                        _List.Add(_TotalEarnings);
                    }

                }
                GridView gv = new GridView();
                GridDataResponse _GridDataResponse = new GridDataResponse();
                _GridDataResponse.data = _List;
                var data = (from s in ((List<TotalEarnings>)_GridDataResponse.data) select new { Date = s.strDate, TotalEarning = s.TotalEarning});
                gv.DataSource = data;
                gv.DataBind();

                if (Convert.ToBoolean(ExportTypeExcelEarning))
                {
                    return new ExportToExcel((GridView)gv, DateTime.Now.ToString("yyyyMMdd_hhmmss") + "_TotalEarning");
                }
                else
                {
                    List<object> _data = new List<object>();
                    _data.AddRange(data);

                    List<string> _header = new List<string>();
                    _header.Add("Date");
                    _header.Add("TotalEarning");                    

                    return new ExportToCSV(_data, _header, DateTime.Now.ToString("yyyyMMdd_hhmmss") + "_TotalEarning");
                }
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }
        #endregion

        #region General Calculation
        public JsonResult BindGeneralCalculation()
        {
            try
            {
                HomeService _HomeService = new HomeService();
                GeneralCalculation _GeneralCalculation = new GeneralCalculation();
                _GeneralCalculation = _HomeService.BindGeneralCalculation(WebRequestResponseServiceContext);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return Json(_GeneralCalculation, JsonRequestBehavior.AllowGet);
                }
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage);
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }
        #endregion

        #region Export New Customers
        public ActionResult ExportNewCustomers(int ExportTypeExcel)
        {
            HomeService _HomeService = new HomeService();
            DateTime now = DateTime.Now;

            GridView gv = new GridView();
            GridDataResponse _GridDataResponse = new GridDataResponse();
            _GridDataResponse = _HomeService.ExportNewCustomersList(WebRequestResponseServiceContext);
            var data = (from s in ((List<NewCustomersResponse>)_GridDataResponse.data) select new { Date = s.MonthYear, Total = s.TotalCount });
            gv.DataSource = data;
            gv.DataBind();


            if (Convert.ToBoolean(ExportTypeExcel))
            {
                return new ExportToExcel((GridView)gv, DateTime.Now.ToString("yyyyMMdd_hhmmss") + "_NewCustomers");
            }
            else
            {

                List<object> _data = new List<object>();
                _data.AddRange(data);

                List<string> _header = new List<string>();
                _header.Add("Date");
                _header.Add("Total");

                return new ExportToCSV(_data, _header, DateTime.Now.ToString("yyyyMMdd_hhmmss") + "_NewCustomers");
            }
        }
        #endregion

    }
}