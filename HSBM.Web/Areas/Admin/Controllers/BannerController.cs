using HSBM.Common.Utils;
using HSBM.EntityModel.BannerMaster;
using HSBM.EntityModel.Common;
using HSBM.Service.Contracts;
using HSBM.Service.ServiceContext;
using HSBM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HSBM.Service;
using HSBM.Service.Services;
using HSBM.Common.Enums;

namespace HSBM.Web.Areas.Admin.Controllers
{
    public class BannerController : BaseController
    {
        BannerService _BannerService = new BannerService();

        [CustomAuthorizeAction(Module.Banners, ModuleAccess.CanView)]
        public ActionResult Index()
        {            
            return View();
        }

        [CustomAuthorizeAction(Module.Banners, ModuleAccess.CanView)]
        public JsonResult GetAllBannerBySearchRequest(BannerMasterRequest p_SearchRequest)
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                GridDataResponse _GridDataResponse = _BannerService.GetAllBannerBySearchRequest(WebRequestResponseServiceContext, p_GridParams, p_SearchRequest);

                if (_GridDataResponse.data != null && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return Json(_GridDataResponse.data, JsonRequestBehavior.AllowGet);
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

        [CustomAuthorizeAction(Module.Banners, ModuleAccess.CanAdd)]
        public ActionResult AddBanner()
        {
            return PartialView("AddUpdateBanner", new BannerMaster());
        }

        [CustomAuthorizeAction(Module.Banners, ModuleAccess.CanUpdate)]
        public ActionResult UpdateBanner(long Id)
        {
            BannerMaster banner = new BannerMaster();
            try
            {
                banner = _BannerService.GetBannerById(WebRequestResponseServiceContext, Id);
                if (banner != null && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return PartialView("AddUpdateBanner", banner);
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

        public JsonResult AddUpdateBanner()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    List<BannerMaster> ListofBannerMaster = new List<BannerMaster>();
                    HttpFileCollectionBase file = Request.Files;

                    if (file != null && file.Count > 0)
                    {
                        string _Path = Server.MapPath("~" + MvcApplication.BannerImagePath);
                        if (!System.IO.Directory.Exists(_Path))
                            System.IO.Directory.CreateDirectory(_Path);


                        for (int _IndexFile = 0; _IndexFile < file.Count; _IndexFile++)
                        {
                            HttpPostedFileBase _File = file[_IndexFile];
                            if (_File.ContentLength == 0)
                            {
                                return JsonErrorResponse("Image uploaded is invalid . please upload a valid one!", JsonRequestBehavior.AllowGet);
                            }
                        }

                        for (int _IndexFile = 0; _IndexFile < file.Count; _IndexFile++)
                        {
                            HttpPostedFileBase _File = file[_IndexFile];
                            string fileName = Guid.NewGuid() + "-" + Path.GetFileName(_File.FileName);
                            fileName = fileName.Replace(" ", "");
                            var path = Path.Combine(_Path, fileName);
                            _File.SaveAs(path);

                            ListofBannerMaster.Add(new BannerMaster() { ImageName = fileName, CreatedBy = SessionProxy.UserDetails.Id, CreatedDate = DateTime.Now });
                        }
                    }

                    int Affected = _BannerService.AddorUpdateBanner(WebRequestResponseServiceContext, ListofBannerMaster);
                    if (Affected > 0 && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                    {
                        return Json(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);

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
                return JsonErrorResponse("Data not found", JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAction(Module.Banners, ModuleAccess.CanDelete)]
        public ActionResult DeleteBanner(long Id, string ImageName)
        {
            _BannerService.DeleteBannerById(WebRequestResponseServiceContext, Id);
            if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
            {
                string _ImagePath = Server.MapPath("~" + MvcApplication.BannerImagePath);

                if (System.IO.File.Exists(Path.Combine(_ImagePath, ImageName)))
                {
                    System.IO.File.Delete(Path.Combine(_ImagePath, ImageName));
                }
                return JsonSuccessResponse("Banner has been deleted successfully", JsonRequestBehavior.AllowGet);
            }
            return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorizeAction(Module.Banners, ModuleAccess.CanDelete)]
        public JsonResult ActiveAndInactiveSwitchUpdate(BannerMaster bannerMaster)
        {
            _BannerService.ActiveAndInactiveSwitchUpdateForBanner(WebRequestResponseServiceContext, bannerMaster);

            if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
