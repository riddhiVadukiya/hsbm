using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.FarmStays;
using HSBM.Service;
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
    public class FarmStaysRatingsAndReviewController : BaseController
    {
        FarmStaysRatingsAndReviewService _FarmStaysRatingsAndReviewService = new FarmStaysRatingsAndReviewService();
        // GET: Admin/FarmStaysRatingsAndReview
        public ActionResult Index()
        {
            List<SelectListItem> _ListFrontFarmStays = new List<SelectListItem>();

            FrontFarmStaysSearchService _farmStaysService = new FrontFarmStaysSearchService();
            _ListFrontFarmStays = _farmStaysService.GetFarmStaysForDropDown();
            ViewBag.FrontFarmStays = _ListFrontFarmStays;            
            return View();
        }

        public JsonResult GetAllFarmStaysRatingsAndReviewBySearchRequest(FarmStaysRatingsAndReviewRequest p_SearchRequest)
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "CreatedDate desc";

                GridDataResponse _GridDataResponse = _FarmStaysRatingsAndReviewService.GetAllFarmStaysRatingsAndReviewBySearchRequest(WebRequestResponseServiceContext, p_GridParams, p_SearchRequest);

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

        public ActionResult ViewRatingsAndReviewDetail(long id)
        {
            FarmStaysRatingsAndReviewResponse ordersMaster = _FarmStaysRatingsAndReviewService.GetRatingsAndReviewDetailByKey(id);
            return View(ordersMaster);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorizeAction(Module.FarmStaysRatingsReviews, ModuleAccess.CanUpdate)]
        public JsonResult AprroveAndUnapproveRatingsAndReview(int Id)
        {
            try
            {
                _FarmStaysRatingsAndReviewService.AprroveAndUnapproveRatingsAndReview(WebRequestResponseServiceContext, Id, SessionProxy.UserDetails.Id);
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