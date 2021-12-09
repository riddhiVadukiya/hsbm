using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.AmenityMaster;
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
    public class AmenityMasterController : BaseController
    {
        AmenityMasterService _AmenityMasterService = new AmenityMasterService();

        [CustomAuthorizeAction(Module.Amenity, ModuleAccess.CanView)]
        public ActionResult Index()
        {
            if (TempData["ToastrMSG"] != null)
            {
                ViewBag.ToastrMSG = (ToastrMSG)TempData["ToastrMSG"];
            }

            return View();
        }

        [CustomAuthorizeAction(Module.Amenity, ModuleAccess.CanView)]
        public JsonResult GetAllAmenityMastersBySearchRequest(AmenityMasterRequest p_SearchRequest)
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "AmenityName";

                GridDataResponse _GridDataResponse = _AmenityMasterService.GetAllAmenityMastersBySearchRequest(WebRequestResponseServiceContext, p_GridParams, p_SearchRequest);

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

        [CustomAuthorizeAction(Module.Amenity, ModuleAccess.CanAdd)]
        public ActionResult AddAmenityMaster()
        {
            return PartialView("AddUpdateAmenityMaster", new AmenityMaster() { IsActive=true});
        }

        [CustomAuthorizeAction(Module.Amenity, ModuleAccess.CanUpdate)]
        public ActionResult UpdateAmenityMaster(int Id)
        {
            AmenityMaster amenity = new AmenityMaster();
            try
            {
                amenity = _AmenityMasterService.GetAmenityMasterById(WebRequestResponseServiceContext, Id);
                if (amenity != null && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return PartialView("AddUpdateAmenityMaster", amenity);
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
        public ActionResult AddOrUpdateAmenityMaster(AmenityMaster amenityMaster, HttpPostedFileBase amenityFile)
        {
            if (ModelState.IsValid && (!String.IsNullOrEmpty(amenityMaster.ImageUrl) || amenityFile != null))
            {
                try
                {
                    AmenityMaster tempamenity = new AmenityMaster();
                    string fileName = string.Empty;
                    string OldImage = string.Empty;
                    string _Path = Server.MapPath("~" + MvcApplication.AmenityImagePath);
                                        
                    if (amenityFile != null)
                    {
                        if (amenityFile.ContentLength == 0)
                        {
                            amenityMaster.ImageUrl = "";
                            ViewBag.ToastrMSG = new ToastrMSG() { Message = "Image uploaded is invalid . please upload a valid one!", Type = "error", ErrorTitle = "Error" };
                            return PartialView("AddUpdateAmenityMaster", amenityMaster);
                        }

                        var img = System.Drawing.Image.FromStream(amenityFile.InputStream, true, true);
                        if (img.Width > 100 && img.Height > 100)
                        {
                            amenityMaster.ImageUrl = "";
                            ViewBag.ToastrMSG = new ToastrMSG() { Message = "Image size must be less than or equal to 100x100!", Type = "error", ErrorTitle = "Error" };
                            return PartialView("AddUpdateAmenityMaster", amenityMaster);
                        }

                        if (!System.IO.Directory.Exists(_Path))
                            System.IO.Directory.CreateDirectory(_Path);
                        HttpPostedFileBase _File = amenityFile;
                        fileName = Guid.NewGuid() + "-" + Path.GetFileName(_File.FileName);
                        fileName = fileName.Replace(" ", "");
                        var path = Path.Combine(_Path, fileName);
                        _File.SaveAs(path);
                        
                        amenityMaster.ImageUrl = fileName;
                    }



                    if (amenityMaster.Id > 0 && amenityFile != null)
                    {
                        tempamenity = _AmenityMasterService.GetAmenityMasterById(WebRequestResponseServiceContext, amenityMaster.Id);
                        OldImage = tempamenity.ImageUrl;
                    }

                    if (amenityMaster.Id > 0)
                    {
                        amenityMaster.UpdatedBy = SessionProxy.UserDetails.Id;
                        amenityMaster.UpdatedDate = DateTime.Now;
                    }
                    else
                    {
                        amenityMaster.CreatedBy = SessionProxy.UserDetails.Id;
                        amenityMaster.CreatedDate = DateTime.Now;
                    }

                    int Affected = _AmenityMasterService.AddOrUpdateAmenityMaster(WebRequestResponseServiceContext, amenityMaster);
                    if (Affected > 0 && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                    {
                        if (amenityMaster.Id > 0)
                        {
                            if (!string.IsNullOrEmpty(OldImage))
                            {
                                if (System.IO.File.Exists(Path.Combine(_Path, OldImage)))
                                {
                                    System.IO.File.Delete(Path.Combine(_Path, OldImage));
                                }
                            }

                            TempData["ToastrMSG"] = new ToastrMSG() { Message = "Amenity has been updated succefully", Type = "success", ErrorTitle = "Success" };
                        }
                        else
                            TempData["ToastrMSG"] = new ToastrMSG() { Message = "Amenity has been added succefully", Type = "success", ErrorTitle = "Success" };

                        return RedirectToAction("Index");
                    }

                    
                    
                    //if (amenityFile != null)
                    //{
                    //    if (System.IO.File.Exists(Path.Combine(_Path, fileName)))
                    //    {
                    //        System.IO.File.Delete(Path.Combine(_Path, fileName));
                    //    }
                    //}
                    if (Affected > 0 && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.IS_EXIST)
                    {
                        ModelState.Clear();
                        ViewBag.ToastrMSG = new ToastrMSG() { Message = "Amenity already exist.", Type = "error", ErrorTitle = "Error" };
                        return PartialView("AddUpdateAmenityMaster", amenityMaster);
                    }
                    ViewBag.ToastrMSG = new ToastrMSG() { Message = "Somthing went wrong!", Type = "error", ErrorTitle = "Error" };
                    return PartialView("AddUpdateAmenityMaster", amenityMaster);

                }
                catch
                {
                    ViewBag.ToastrMSG = new ToastrMSG() { Message = "Somthing went wrong!", Type = "error", ErrorTitle = "Error" };
                    return PartialView("AddUpdateAmenityMaster", amenityMaster);
                }
            }
            return PartialView("AddUpdateAmenityMaster", amenityMaster);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorizeAction(Module.Amenity, ModuleAccess.CanDelete)]
        public JsonResult ActiveAndInactiveSwitchUpdate(AmenityMaster amenityMaster)
        {
            try
            {
                _AmenityMasterService.ActiveAndInactiveAmenityMaster(WebRequestResponseServiceContext, amenityMaster);
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

        public JsonResult GetAllAmenityMastersForDropDown()
        {
            List<SelectListItem> _List = _AmenityMasterService.GetAllAmenityMastersForDropDown(WebRequestResponseServiceContext);
            return Json(_List, JsonRequestBehavior.AllowGet);
        }

    }
}
