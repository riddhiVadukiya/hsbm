using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.AmenityMaster;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.DiscountMaster;
using HSBM.EntityModel.FarmStays;
using HSBM.Service.ServiceContext;
using HSBM.Service.Services;
using HSBM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Areas.Admin.Controllers
{
    public class FarmStaysController : BaseController
    {
        FarmStaysService _FarmStaysService = new FarmStaysService();

        [CustomAuthorizeAction(Module.FarmStays, ModuleAccess.CanView)]
        public ActionResult Index()
        {
            if (TempData["ToastrMSG"] != null)
            {
                ViewBag.ToastrMSG = (ToastrMSG)TempData["ToastrMSG"];
            }

            return View();
        }

        [CustomAuthorizeAction(Module.FarmStays, ModuleAccess.CanAdd)]
        public ActionResult AddFarmStays()
        {
            ViewBag.FarmStayId = 0;
            ViewBag.Title = "Add Farm/Home Stay";
            return PartialView("AddUpdateFarmStays");
        }

        [CustomAuthorizeAction(Module.FarmStays, ModuleAccess.CanUpdate)]
        public ActionResult UpdateFarmStays(int Id)
        {

            ViewBag.FarmStayId = Id;
            ViewBag.Title = "Update Farm/Home Stay";
            return PartialView("AddUpdateFarmStays");
        }

        [CustomAuthorizeAction(Module.FarmStays, ModuleAccess.CanView)]
        public JsonResult GetAllFarmStaysBySearchRequest(FarmStaysRequest p_SearchRequest)
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "Name";

                GridDataResponse _GridDataResponse = _FarmStaysService.GetAllFarmStaysBySearchRequest(WebRequestResponseServiceContext, p_GridParams, p_SearchRequest);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorizeAction(Module.FarmStays, ModuleAccess.CanDelete)]
        public JsonResult ActiveAndInactiveFarmStay(FarmStays FarmStays)
        {
            try
            {
                _FarmStaysService.ActiveAndInactiveFarmStay(WebRequestResponseServiceContext, FarmStays);
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

        #region BasicDetail
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveFarmStayBasicDetail(FarmStaysBasicDetail FarmStaysBasicDetail)
        {
                try
                {
                    if (FarmStaysBasicDetail.Id > 0)
                    {
                        FarmStaysBasicDetail.UpdatedBy = SessionProxy.UserDetails.Id;
                        FarmStaysBasicDetail.UpdatedDate = DateTime.Now;
                    }
                    else
                    {
                        FarmStaysBasicDetail.CreatedBy = SessionProxy.UserDetails.Id;
                        FarmStaysBasicDetail.CreatedDate = DateTime.Now;
                        FarmStaysBasicDetail.IsActive = true;
                    }

                    int Affected = _FarmStaysService.AddOrUpdateFarmStayBasicDetail(WebRequestResponseServiceContext, FarmStaysBasicDetail);
                    if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && Affected > 0)
                    {
                        return JsonSuccessResponse(Affected, WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                    }
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
            
        }

        [HttpGet]
        public ActionResult GetFarmStayBasicDetailById(int Id)
        {
            FarmStays FarmStayDetail = new FarmStays();
            try
            {
                FarmStayDetail = _FarmStaysService.GetFarmStayBasicDetailById(WebRequestResponseServiceContext, Id);
                if (FarmStayDetail != null && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return Json(FarmStayDetail, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return JsonErrorResponse("Not Found", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Amenity
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveFarmStayAmenities(List<FarmStaysAmenities> Amenities)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    int Affected = _FarmStaysService.SaveFarmStayAmenities(WebRequestResponseServiceContext, Amenities);
                    if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && Affected > 0)
                    {
                        return JsonSuccessResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                    }
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Policy
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveFarmStayPolicy(FarmStaysPolicyDetail FarmStaysPolicyDetail)
        {

            if (ModelState.IsValid)
            {
                try
                {


                    int Affected = _FarmStaysService.SaveFarmStayPolicy(WebRequestResponseServiceContext, FarmStaysPolicyDetail);
                    if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && Affected > 0)
                    {
                        return JsonSuccessResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                    }
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }

                catch
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Image
        public JsonResult UploadImages()
        {
            List<FarmStaysImages> _ListOfAlbumImagesAndVideosDto = new List<FarmStaysImages>();
            try
            {
                if (Request.Files.Count > 0)
                {
                    string _Path = Server.MapPath("~" + MvcApplication.FarmStayImagePath);
                    if (!System.IO.Directory.Exists(_Path))
                        System.IO.Directory.CreateDirectory(_Path);

                    HttpFileCollectionBase file = Request.Files;
                    if (file.Count > 0)
                    {
                        for (int _IndexFile = 0; _IndexFile < file.Count; _IndexFile++)
                        {
                            HttpPostedFileBase _File = file[_IndexFile];

                            if (_File.ContentLength == 0)
                            {
                                return JsonErrorResponse("Image uploaded is invalid . please upload a valid one!", JsonRequestBehavior.AllowGet);
                            }

                        }


                        bool _IsImageSizeProper = true;
                        string errorMsg = string.Empty;
                        for (int _IndexFile = 0; _IndexFile < file.Count; _IndexFile++)
                        {
                            HttpPostedFileBase _File = file[_IndexFile];


                            string _FileName = DateTime.Now.Ticks + "-" + Path.GetFileName(_File.FileName);
                            _FileName = _FileName.Replace(" ", "");
                            var path = Path.Combine(_Path, _FileName);
                            _File.SaveAs(path);


                            if (!string.IsNullOrEmpty(_FileName))
                            {
                                _ListOfAlbumImagesAndVideosDto.Add(new FarmStaysImages()
                                {
                                    ImageName = _FileName,
                                    IsDeleted = false
                                });
                            }
                            else
                            {
                                _IsImageSizeProper = false;
                            }
                        }
                        if (_IsImageSizeProper)
                        {
                            return Json(new { Data = _ListOfAlbumImagesAndVideosDto, Message = string.Empty }, JsonRequestBehavior.AllowGet);
                        }

                        return Json(new { Data = _ListOfAlbumImagesAndVideosDto, Message = "Invalid Image" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
            return Json(new { Data = _ListOfAlbumImagesAndVideosDto, Message = "No Image found" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveFarmStayImages(List<FarmStaysImages> FarmStaysImages)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    List<FarmStaysImages> _ListofFarmStaysImages = _FarmStaysService.SaveFarmStayImages(WebRequestResponseServiceContext, FarmStaysImages);
                    if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && _ListofFarmStaysImages.Count() > 0)
                    {
                        string _ImagePath = Server.MapPath("~" + MvcApplication.FarmStayImagePath);
                        foreach (var image in FarmStaysImages.Where(x => x.IsDeleted))
                        {
                            if (System.IO.File.Exists(Path.Combine(_ImagePath, image.ImageName)))
                            {
                                System.IO.File.Delete(Path.Combine(_ImagePath, image.ImageName));
                            }
                        }
                        return JsonSuccessResponse(_ListofFarmStaysImages.Where(x => !x.IsDeleted), WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                    }
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Season
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveFarmStaySeason(FarmStaysSeasons FarmStaysSeasons)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    int Affected = _FarmStaysService.SaveFarmStaySeason(WebRequestResponseServiceContext, FarmStaysSeasons);
                    if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && Affected > 0)
                    {
                        return JsonSuccessResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                    }
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetSeasonByRoomId(int RoomId)
        {

            try
            {
                List<FarmStaysSeasons> _ListofFarmSeason = _FarmStaysService.GetSeasonByRoomId(WebRequestResponseServiceContext, RoomId);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return JsonSuccessResponse(_ListofFarmSeason, WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult DeleteFarmStaySeason(Guid GroupId)
        {

            try
            {
                int Affected = _FarmStaysService.DeleteFarmStaySeason(WebRequestResponseServiceContext, GroupId);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && Affected > 0)
                {
                    return JsonSuccessResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Discount
        [HttpGet]
        public ActionResult GetDiscountByFarmStayId(int FarmStayId)
        {

            try
            {
                List<DiscountMaster> _ListofFarmDiscount = _FarmStaysService.GetDiscountByFarmStayId(WebRequestResponseServiceContext, FarmStayId);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return JsonSuccessResponse(_ListofFarmDiscount, WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Room
        public JsonResult GetRoomTypeForDropDown()
        {

            List<SelectListItem> _List = new List<SelectListItem>();
            foreach (RoomType item in Enum.GetValues(typeof(RoomType)))
            {
                _List.Add(new SelectListItem() { Text = Helper.GetEnumDescription(item), Value = Convert.ToString((int)item) });
            }
            return Json(_List, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveFarmStayRoom(FarmStaysRooms FarmStaysRooms)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    int Affected = _FarmStaysService.SaveFarmStayRoom(WebRequestResponseServiceContext, FarmStaysRooms);
                    if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && Affected > 0)
                    {
                        return JsonSuccessResponse(Affected, WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                    }
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetRoomByFarmStayId(int FarmStayId)
        {

            try
            {
                List<FarmStaysRooms> _ListofFarmRoom = _FarmStaysService.GetRoomByFarmStayId(WebRequestResponseServiceContext, FarmStayId);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return JsonSuccessResponse(_ListofFarmRoom, WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult DeleteFarmStayRoom(int Id)
        {

            try
            {
                int Affected = _FarmStaysService.DeleteFarmStayRoom(WebRequestResponseServiceContext, Id);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && Affected > 0)
                {
                    return JsonSuccessResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetNextSevenDays(DateTime? currentDate)
        {
            List<DayClass> next7Days = new List<DayClass>();
            if (currentDate == null)
            {
                currentDate = DateTime.Now;
            }
            for (int i = 0; i < 7; i++)
            {
                //string _date;
                DayClass _DayClass = new DayClass();
                DateTime _NewDate = new DateTime();
                _NewDate = currentDate.Value.AddDays(i);
                _DayClass.ShortDate = _NewDate.Date.ToShortDateString();
                _DayClass.DD = _NewDate.ToString("dd");
                _DayClass.MMM = _NewDate.ToString("MMM");
                _DayClass.YYYY = _NewDate.ToString("yyyy");

                next7Days.Add(_DayClass);
            }
            return Json(next7Days, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PreviousClick(DateTime selectedDate)
        {
            List<DayClass> previous7Days = new List<DayClass>();
            for (int i = 0; i < 7; i++)
            {
                //string _date;
                DayClass _DayClass = new DayClass();
                DateTime _NewDate = new DateTime();
                _NewDate = selectedDate.AddDays(-i);
                _DayClass.ShortDate = _NewDate.Date.ToShortDateString();
                _DayClass.DD = _NewDate.ToString("dd");
                _DayClass.MMM = _NewDate.ToString("MMM");
                _DayClass.YYYY = _NewDate.ToString("yyyy");

                previous7Days.Add(_DayClass);
            }
            previous7Days.OrderByDescending(x=>x.ShortDate);
            previous7Days.Reverse();
            return Json(previous7Days, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NextClick(DateTime selectedDate)
        {
            List<DayClass> next7Days = new List<DayClass>();
            for (int i = 0; i < 7; i++)
            {
                //string _date;
                DayClass _DayClass = new DayClass();
                DateTime _NewDate = new DateTime();
                _NewDate = selectedDate.AddDays(i);
                _DayClass.ShortDate = _NewDate.Date.ToShortDateString();
                _DayClass.DD = _NewDate.ToString("dd");
                _DayClass.MMM = _NewDate.ToString("MMM");
                _DayClass.YYYY = _NewDate.ToString("yyyy");

                next7Days.Add(_DayClass);
                //next7Days.OrderByDescending(x => x.FirstOrDefault());
                //next7Days.Reverse();
            }
            return Json(next7Days, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetByDate(SeasonRequest p_SeasonRequest)
        {

            //List<DateTime> allDates = new List<DateTime>();
            //for (DateTime date = _SeasonRequest.SeasonStartDate; date <= _SeasonRequest.SeasonEndDate; date = date.AddDays(1))
            //    allDates.Add(date);
            //allDates.OrderByDescending(x => x);
            //allDates.Reverse();
            //List<Plans> _ListPlan = new List<Plans>();
            //foreach (PlanEnum item in Enum.GetValues(typeof(PlanEnum)))
            //{
            //    List<PriceWithDates> _PriceDate = new List<PriceWithDates>();
            //    for(var i = 0; i<7;i++){
            //        decimal p = i +1;
            //        DateTime _NewDate = DateTime.Now;
            //        PriceWithDates _pd = new PriceWithDates();
            //        _pd.Price = p;
            //        _pd.Date = _NewDate.AddDays(i);
            //        _PriceDate.Add(_pd);
            //    }
            //    _ListPlan.Add(new Plans() { PlanName = Helper.GetEnumDescription(item), ListOfPriceDates = _PriceDate });
            //}

            //List<RatePlan> _ListRatePlan = new List<RatePlan>();
            //foreach (RatePlanEnum item in Enum.GetValues(typeof(RatePlanEnum)))
            //{
            //    _ListRatePlan.Add(new RatePlan() { RatePlanName = Helper.GetEnumDescription(item), ListOfPlans = _ListPlan });
            //}

            //TestingClass _TestingClass = new TestingClass();
            //DateTime _NewCurrentDate = DateTime.Now;
            //_TestingClass.CurrentDate = _NewCurrentDate.Date.ToShortDateString();
            //_TestingClass.ListOfRatePlan = _ListRatePlan;
            //return Json(_TestingClass, JsonRequestBehavior.AllowGet);

            try
            {
                SeasonListResponse _SeasonListResponse = _FarmStaysService.GetSeasonByBookingDateRoomId(WebRequestResponseServiceContext, p_SeasonRequest);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return JsonSuccessResponse(_SeasonListResponse, WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateSeason(SeasonListResponse p_SeasonListResponse)
        {
            try
            {
                int Affected = _FarmStaysService.UpdateSeason(WebRequestResponseServiceContext, p_SeasonListResponse);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && Affected > 0)
                {
                    return JsonSuccessResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllRatePlansandPlans(int p_RoomId)
        {
            AddSeasonResponse _AddSeasonResponse = new AddSeasonResponse();

            List<Plans> _ListPlan = new List<Plans>();
            foreach (PlanEnum item in Enum.GetValues(typeof(PlanEnum)))
            {
                _ListPlan.Add(new Plans() { PlanName = Helper.GetEnumDescription(item) });
            }

            List<RatePlan> _ListRatePlan = new List<RatePlan>();
            foreach (RatePlanEnum item in Enum.GetValues(typeof(RatePlanEnum)))
            {
                _ListRatePlan.Add(new RatePlan() { RatePlanName = Helper.GetEnumDescription(item), ListOfPlans = _ListPlan });
            }
            _AddSeasonResponse.RoomId = p_RoomId;
            _AddSeasonResponse.ListOfRatePlan = _ListRatePlan;
            _AddSeasonResponse.ListOfPlan = _ListPlan;

            return Json(_AddSeasonResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddSeason(AddSeasonResponse p_AddSeasonResponse)
        {
            try
            {
                int Affected = _FarmStaysService.AddSeason(WebRequestResponseServiceContext, p_AddSeasonResponse);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && Affected > 0)
                {
                    return JsonSuccessResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}