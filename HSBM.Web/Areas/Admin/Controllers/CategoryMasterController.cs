using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.CategoryMaster;
using HSBM.EntityModel.Common;
using HSBM.Service;
using HSBM.Service.Contracts;
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
    public class CategoryMasterController : BaseController
    {
        CategoryMasterService _CategoryMasterService = new CategoryMasterService();

        [CustomAuthorizeAction(Module.Category, ModuleAccess.CanView)]
        public ActionResult Index()
        {
            if (TempData["ToastrMSG"] != null)
            {
                ViewBag.ToastrMSG = (ToastrMSG)TempData["ToastrMSG"];
            }

            return View();
        }

        [CustomAuthorizeAction(Module.Category, ModuleAccess.CanView)]
        public JsonResult GetAllCategoryMastersBySearchRequest(CategoryMasterRequest p_SearchRequest)
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "CategoryName";

                GridDataResponse _GridDataResponse = _CategoryMasterService.GetAllCategoryMastersBySearchRequest(WebRequestResponseServiceContext, p_GridParams, p_SearchRequest);

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

        [CustomAuthorizeAction(Module.Category, ModuleAccess.CanAdd)]
        public ActionResult AddCategoryMaster()
        {
            return PartialView("AddUpdateCategoryMaster", new CategoryMaster() { IsActive = true });
        }

        [CustomAuthorizeAction(Module.Category, ModuleAccess.CanUpdate)]
        public ActionResult UpdateCategoryMaster(int Id)
        {
            CategoryMaster Category = new CategoryMaster();
            try
            {
                Category = _CategoryMasterService.GetCategoryMasterById(WebRequestResponseServiceContext, Id);
                if (Category != null && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return PartialView("AddUpdateCategoryMaster", Category);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrUpdateCategoryMaster(CategoryMaster CategoryMaster)
        {
            if (ModelState.IsValid )
            {
                try
                {
                    CategoryMaster tempCategory = new CategoryMaster();
                    string fileName = string.Empty;
                   
                   

                        if (CategoryMaster.Id > 0)
                        {
                            tempCategory = _CategoryMasterService.GetCategoryMasterById(WebRequestResponseServiceContext, CategoryMaster.Id);
                          
                        }

                        if (CategoryMaster.Id > 0)
                        {
                            CategoryMaster.UpdatedBy = SessionProxy.UserDetails.Id;
                            CategoryMaster.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            CategoryMaster.CreatedBy = SessionProxy.UserDetails.Id;
                            CategoryMaster.CreatedDate = DateTime.Now;
                        }

                        int Affected = _CategoryMasterService.AddOrUpdateCategoryMaster(WebRequestResponseServiceContext, CategoryMaster);
                        if (Affected > 0 && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                        {
                            if (CategoryMaster.Id > 0)
                            {                                
                                TempData["ToastrMSG"] = new ToastrMSG() { Message = "Category has been updated succefully", Type = "success", ErrorTitle = "Success" };
                            }
                            else
                                TempData["ToastrMSG"] = new ToastrMSG() { Message = "Category has been added succefully", Type = "success", ErrorTitle = "Success" };

                            return RedirectToAction("Index");
                        }
                      
                        ViewBag.ToastrMSG = new ToastrMSG() { Message = "Somthing went wrong!", Type = "error", ErrorTitle = "Error" };
                        return PartialView("AddUpdateCategoryMaster", CategoryMaster);
                    
                    
                }
                catch
                {
                    ViewBag.ToastrMSG = new ToastrMSG() { Message = "Somthing went wrong!", Type = "error", ErrorTitle = "Error" };
                    return PartialView("AddUpdateCategoryMaster", CategoryMaster);
                }
            }
            return PartialView("AddUpdateCategoryMaster", CategoryMaster);
            

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorizeAction(Module.Category, ModuleAccess.CanDelete)]
        public JsonResult ActiveAndInactiveSwitchUpdate(CategoryMaster CategoryMaster)
        {
            try
            {
                _CategoryMasterService.ActiveAndInactiveCategoryMaster(WebRequestResponseServiceContext, CategoryMaster);
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

        public JsonResult GetAllCategoryMastersForDropDown()
        {
            List<SelectListItem> _List = _CategoryMasterService.GetAllCategoryMastersForDropDown(WebRequestResponseServiceContext);           
            return Json(_List, JsonRequestBehavior.AllowGet);
        }

    }
}
