using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.DiscountMaster;
using HSBM.EntityModel.FarmStays;
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
    public class DiscountController : BaseController
    {
        DiscountService _DiscountMasterService = new DiscountService();

        [CustomAuthorizeAction(Module.Discounts, ModuleAccess.CanView)]
        public ActionResult Index()
        {
            if (TempData["ToastrMSG"] != null)
            {
                ViewBag.ToastrMSG = (ToastrMSG)TempData["ToastrMSG"];
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrUpdateDiscountMaster(DiscountMaster discountMaster)
        {
            CategoryMasterService _CategoryMasterService = new CategoryMasterService();

            if (ModelState.IsValid)
            {
                try
                {

                    int Affected = _DiscountMasterService.AddOrUpdateDiscountMaster(WebRequestResponseServiceContext, discountMaster);
                    if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && Affected > 0)
                    {
                        if (discountMaster.Id > 0)
                            TempData["ToastrMSG"] = new ToastrMSG() { Message = "Discount has been updated successfully.", Type = "success", ErrorTitle = "Success" };
                        else
                            TempData["ToastrMSG"] = new ToastrMSG() { Message = "Discount has been added successfully.", Type = "success", ErrorTitle = "Success" };

                        return RedirectToAction("Index");
                    }

                    ViewBag.ToastrMSG = new ToastrMSG() { Message = WebRequestResponseServiceContext.Response.StatusMessage, Type = "error", ErrorTitle = "Error" };
                }
                catch
                {
                    ViewBag.ToastrMSG = new ToastrMSG() { Message = "Somthing went wrong!", Type = "error", ErrorTitle = "Error" };
                }
            }
            discountMaster.FarmStays = GetDiscountHistory(discountMaster.Id, discountMaster.SelectedFarmStays);
           // discountMaster.FarmStaysCategories = _CategoryMasterService.GetAllCategoryMastersForDropDown(WebRequestResponseServiceContext);   
            return PartialView("AddUpdateDiscount", discountMaster);
        }

        [CustomAuthorizeAction(Module.Discounts, ModuleAccess.CanAdd)]
        public ActionResult AddDiscount()
        {
            CategoryMasterService _CategoryMasterService = new CategoryMasterService();

            DiscountMaster discount = new DiscountMaster();
            discount.FarmStays = GetDiscountHistory(0);
           // discount.FarmStaysCategories = _CategoryMasterService.GetAllCategoryMastersForDropDown(WebRequestResponseServiceContext);     
            return PartialView("AddUpdateDiscount", discount);
        }

        [CustomAuthorizeAction(Module.Discounts, ModuleAccess.CanUpdate)]
        public ActionResult UpdateDiscount(long id)
        {
            DiscountMaster discount = new DiscountMaster();
            CategoryMasterService _CategoryMasterService = new CategoryMasterService();
            try
            {
                discount = _DiscountMasterService.GetDiscountMasterById(WebRequestResponseServiceContext, id);
                if (discount != null && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    discount.FarmStays = GetDiscountHistory(id);
                   // discount.FarmStaysCategories = _CategoryMasterService.GetAllCategoryMastersForDropDown(WebRequestResponseServiceContext);
                    return PartialView("AddUpdateDiscount", discount);
                }
                else
                {
                    ViewBag.ToastrMSG = new ToastrMSG() { Message = WebRequestResponseServiceContext.Response.StatusMessage, Type = "error", ErrorTitle = "Error" };
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ViewBag.ToastrMSG = new ToastrMSG() { Message = "Somthing went wrong!", Type = "error", ErrorTitle = "Error" };
                return RedirectToAction("Index");
            }
        }

        [CustomAuthorizeAction(Module.Discounts, ModuleAccess.CanView)]
        public JsonResult GetAllDiscountBySearchRequest(DiscountMasterRequest p_SearchRequest)
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "Id";

                GridDataResponse _GridDataResponse = _DiscountMasterService.GetAllDiscountMastersBySearchRequest(WebRequestResponseServiceContext, p_GridParams, p_SearchRequest);

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
        public JsonResult DeleteDiscount(int id)
        {
            try
            {
                _DiscountMasterService.DeleteDiscountMaster(WebRequestResponseServiceContext, id);
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

        private List<DiscountHistoryResponse> GetDiscountHistory(long id)
        {
            return _DiscountMasterService.GetDiscountHistoryByid(id);
        }

        private List<DiscountHistoryResponse> GetDiscountHistory(long id, string selectedDiscount)
        {
            selectedDiscount = selectedDiscount ?? "";

            var FarmStays = GetDiscountHistory(id);
            string[] _selectedDiscount = selectedDiscount.Split(',');
            FarmStays.ForEach(t =>
            {
                t.IsApplied = false;
                if (_selectedDiscount.Contains(t.FarmId))
                {
                    t.IsApplied = true;
                }
            });
            return FarmStays ?? new List<DiscountHistoryResponse>();
        }
    }
}