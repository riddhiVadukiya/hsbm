using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.CMSPageMaster;
using HSBM.EntityModel.Common;
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
    public class CMSPageController : BaseController
    {
        CMSPageService _CMSPageService = new CMSPageService();

        [CustomAuthorizeAction(Module.CMSPages, ModuleAccess.CanView)]
        public ActionResult Index()
        {
            if (TempData["ToastrMSG"] != null)
            {
                ViewBag.ToastrMSG = (ToastrMSG)TempData["ToastrMSG"];
            }

            return View();
        }

        [CustomAuthorizeAction(Module.CMSPages, ModuleAccess.CanView)]
        public JsonResult GetAllCMSPageMasterBySearchRequest(CMSPageMasterRequest p_SearchRequest)
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);
                GridDataResponse _GridDataResponse = _CMSPageService.GetAllCMSPageMasterBySearchRequest(WebRequestResponseServiceContext, p_GridParams, p_SearchRequest);

                if (_GridDataResponse.data != null && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return Json(_GridDataResponse, JsonRequestBehavior.AllowGet);
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

        [CustomAuthorizeAction(Module.CMSPages, ModuleAccess.CanAdd)]
        public ActionResult AddCMSPage()
        {
            ViewBag.Message = "";
            return PartialView("AddUpdateCMSPage", new CMSPageMaster());
        }

        [CustomAuthorizeAction(Module.CMSPages, ModuleAccess.CanUpdate)]
        public ActionResult UpdateCMSPage(long Id)
        {
            ViewBag.Message = "";
            CMSPageMaster cmsMaster = new CMSPageMaster();

            try
            {
                cmsMaster = _CMSPageService.GetCMSPageById(WebRequestResponseServiceContext, Id);

                if (cmsMaster != null && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return PartialView("AddUpdateCMSPage", cmsMaster);
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
        [ValidateInput(false)]        
        public ActionResult AddUpdateCMSPage(CMSPageMaster cmsPageMaster)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (cmsPageMaster.Id > 0)
                    {
                        cmsPageMaster.UpdatedBy = SessionProxy.UserDetails.Id;
                        cmsPageMaster.UpdateDate = DateTime.Now;
                    }
                    else
                    {
                        cmsPageMaster.CreatedBy = SessionProxy.UserDetails.Id;
                        cmsPageMaster.CreatedDate = DateTime.Now;
                    }

                    int Affected = _CMSPageService.AddorUpdateCMSPage(WebRequestResponseServiceContext, cmsPageMaster);

                    if (Affected == 1 && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                    {
                        TempData["ToastrMSG"] = new ToastrMSG() { Message = "Page has been updated successfully.", Type = "success", ErrorTitle = "Success" };
                        return RedirectToAction("Index");
                    }
                         ViewBag.ToastrMSG = new ToastrMSG() { Message = WebRequestResponseServiceContext.Response.StatusMessage, Type = "error", ErrorTitle = "Error" };                    
                        return PartialView("AddUpdateCMSPage", cmsPageMaster);
                    
                }
                catch
                {
                    ViewBag.ToastrMSG = new ToastrMSG() { Message = "Somthing went wrong!", Type = "error", ErrorTitle = "Error" };
                    return PartialView("AddUpdateCMSPage", cmsPageMaster);
                }
            }
            else
            {
                ViewBag.ToastrMSG = new ToastrMSG() { Message = "Somthing went wrong!", Type = "error", ErrorTitle = "Error" };
                return PartialView("AddUpdateCMSPage", cmsPageMaster);
            }
        }

        [CustomAuthorizeAction(Module.CMSPages, ModuleAccess.CanDelete)]
        public ActionResult DeleteCMSPage(long Id)
        {
            _CMSPageService.DeleteCMSPageById(WebRequestResponseServiceContext, Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [CustomAuthorizeAction(Module.CMSPages, ModuleAccess.CanUpdate)]
        public JsonResult ActiveAndInactiveSwitchUpdate(CMSPageMaster cMSPageMaster)
        {
            try
            {
                _CMSPageService.ActiveAndInactiveSwitchUpdateForCMSPage(WebRequestResponseServiceContext, cMSPageMaster);

                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return JsonErrorResponse("", JsonRequestBehavior.AllowGet);
            }
        }
    }
}