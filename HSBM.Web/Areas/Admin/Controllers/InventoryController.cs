using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.InventoryMaster;
using HSBM.Service.ServiceContext;
using HSBM.Service.Services;
using HSBM.Web.Areas.Admin.Models;
using HSBM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Areas.Admin.Controllers
{
    public class InventoryController : BaseController
    {
        InventoryMasterService _InventoryMasterService = new InventoryMasterService();
        FarmStaysService _FarmStaysService = new FarmStaysService();

        [CustomAuthorizeAction(Module.Inventory, ModuleAccess.CanView)]
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorizeAction(Module.Inventory, ModuleAccess.CanView)]
        public ActionResult Book(int id)
        {
            if (TempData["ToastrMSG"] != null)
            {
                ViewBag.ToastrMSG = (ToastrMSG)TempData["ToastrMSG"];
            }


            var _FarmStays = _FarmStaysService.GetFarmStayBasicDetailById(WebRequestResponseServiceContext, id);

            var obj = new CheckAvailabilityRequest() { FarmStaysId = id, FarmStaysName = _FarmStays.Name };
            return View(obj);
        }

        [HttpPost]
        public ActionResult Book(InventoryMaster inventoryMaster)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    inventoryMaster.OnSite = true;
                    inventoryMaster.BookingGroupId = Guid.NewGuid();

                    int Affected = _InventoryMasterService.AddInventoryMaster(WebRequestResponseServiceContext, inventoryMaster);
                    if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && Affected > 0)
                    {
                        TempData["ToastrMSG"] = new ToastrMSG() { Message = "Booking has been created successfully.", Type = "success", ErrorTitle = "Success" };
                        return RedirectToAction("Book", new { id = inventoryMaster.FarmstaysId });
                    }
                    ViewBag.ToastrMSG = new ToastrMSG() { Message = WebRequestResponseServiceContext.Response.StatusMessage, Type = "error", ErrorTitle = "Error" };
                }
                catch (Exception ex)
                {
                    ViewBag.ToastrMSG = new ToastrMSG() { Message = "Somthing went wrong!", Type = "error", ErrorTitle = "Error" };
                }

                return PartialView("Book", inventoryMaster);
            }

            return View(new InventoryMaster());
        }

        public JsonResult GetAllInventoryBySearchRequest(InventoryMasterRequest p_SearchRequest)
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "StartDate";

                GridDataResponse _GridDataResponse = _InventoryMasterService.GetAllInventoryMasterBySearchRequest(WebRequestResponseServiceContext, p_GridParams, p_SearchRequest);

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
        public JsonResult DeleteInventory(Guid id)
        {
            try
            {
                _InventoryMasterService.DeleteInventoryMaster(WebRequestResponseServiceContext, id);
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


        [HttpPost]
        public JsonResult GetInventoryAvailableByDate(CheckAvailabilityRequest req)
        {
            try
            {
                var lst = _InventoryMasterService.GetInventoryAvailableByDate(WebRequestResponseServiceContext, req.StartDate.Value, req.EndDate.Value, req.FarmStaysId);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return Json(lst, JsonRequestBehavior.AllowGet);
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

        public JsonResult GetAllUpCommingOrder(InventoryMasterRequest p_SearchRequest)
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "CheckInDate";

                GridDataResponse _GridDataResponse = _InventoryMasterService.GetAllUpCommingOrder(WebRequestResponseServiceContext, p_GridParams, p_SearchRequest);

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